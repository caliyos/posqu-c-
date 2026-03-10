using Npgsql;
using POS_qu.Controllers;
using POS_qu.Core;
using POS_qu.DTO;
using POS_qu.Helpers;
using POS_qu.Models;
using POS_qu.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace POS_qu.services
{
    public class TransactionService
    {

        private readonly IActivityService _activityService;
        public readonly TransactionRepo _repo;

        public TransactionService(
    TransactionRepo repo,
    IActivityService activityService)
        {
            _repo = repo;
            _activityService = activityService;
        }

        public decimal CalculateChangePreview(InvoiceData invoice, decimal paymentAmount)
        {
            if (invoice == null || invoice.Items == null || !invoice.Items.Any())
                return 0;

            decimal grandTotal = CalculateGrandTotal(invoice);

            decimal change = paymentAmount - grandTotal;

            return change > 0 ? change : 0;
        }

        public decimal CalculateGrandTotal(InvoiceData invoice)
        {
            decimal subtotal = invoice.Items.Sum(x => x.Total);

            decimal discountAmount = subtotal * invoice.GlobalDiscountPercent / 100;

            decimal afterDiscount = subtotal - discountAmount;

            decimal finalTotal = afterDiscount + invoice.DeliveryAmount;

            return finalTotal;
        }
        public TransactionResult ProcessPayment(
       InvoiceData invoice,
       decimal paymentAmount,
       string paymentMethod)
        {
            if (invoice == null || !invoice.Items.Any())
                return TransactionResult.Fail("Invoice empty.");

            decimal grandTotal = CalculateGrandTotal(invoice);

            if (paymentAmount < grandTotal)
                return TransactionResult.Fail("Insufficient payment.");

            decimal change = paymentAmount - grandTotal;

            invoice.PaymentAmount = paymentAmount;
            invoice.ChangeAmount = change;
            invoice.Status = "Paid";
            invoice.PaymentMethod = paymentMethod;


            return TransactionResult.Success(change);
        }
        public TransactionResult ProcessPaymentAndSave(
            InvoiceData invoice,
            decimal paymentAmount,
            string paymentMethod)
        {
            var paymentResult = ProcessPayment(invoice, paymentAmount, paymentMethod);

            if (!paymentResult.IsSuccess)
                return paymentResult;

            var sessionUser = SessionUser.GetCurrentUser();

            using (var con = new NpgsqlConnection(DbConfig.ConnectionString))
            {
                con.Open();

                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        // ===============================
                        // BUILD TRANSACTION OBJECT
                        // ===============================

                        Transactions transaction = new Transactions
                        {
                            TsNumbering = Utility.GenerateTransactionNumber(),
                            TsCode = Utility.getTrxNumbering(),
                            TsTotal = invoice.Items.Sum(x => x.Total),
                            TsPaymentAmount = invoice.PaymentAmount,
                            TsCashback = invoice.ChangeAmount,
                            TsMethod = invoice.PaymentMethod,
                            TsStatus = TransactionStatus.Paid,
                            TsChange = invoice.ChangeAmount,
                            TsInternalNote = "Processed via POS system",
                            TsNote = invoice.GlobalNote ?? string.Empty,
                            TsDiscountTotal = 0,
                            TsGrandTotal = CalculateGrandTotal(invoice),
                            TsCustomer = null,
                            TsDelivery = invoice.DeliveryAmount,
                            TsFreename = "Guest",
                            UserId = sessionUser.UserId,
                            CreatedBy = sessionUser.UserId,
                            TerminalId = sessionUser.TerminalId,
                            ShiftId = sessionUser.ShiftId,
                            CreatedAt = DateTime.Now,
                            OrderId = null,
                            CartSessionCode = invoice.CartSessionCode
                        };

                        // ===============================
                        // INSERT HEADER
                        // ===============================

                        int transactionId = _repo.InsertTransaction(con, tran, transaction);

                        if (transactionId <= 0)
                            throw new Exception("Failed to insert transaction header");

                        // ===============================
                        // BUILD DETAIL LIST
                        // ===============================

                        var details = invoice.Items.Select(i => new TransactionDetail
                        {
                            TsId = transactionId,
                            ItemId = i.ItemId,
                            Barcode = i.Barcode,
                            Name = i.Name,
                            TsdSellPrice = i.Price,
                            TsdBuyPrice = i.CostPrice,
                            TsdQuantity = i.Qty,
                            TsdUnit = i.Unit,
                            TsdConversionRate = i.ConversionRate,
                            TsdPricePerUnit = i.Price,
                            TsdUnitVariant = i.Unit,
                            TsdDiscountPerItem = i.Qty > 0 ? i.DiscountAmount / i.Qty : 0,
                            TsdDiscountPercentage = i.DiscountPercent,
                            TsdDiscountTotal = i.DiscountAmount,
                            TsdTax = i.Tax,
                            TsdTotal = i.Total,
                            TsdNote = i.Note,
                            CreatedBy = sessionUser.UserId,
                            CreatedAt = DateTime.Now,
                            CartSessionCode = invoice.CartSessionCode
                        }).ToList();

                        if (details.Count > 0)
                            _repo.InsertTransactionDetails(con, tran, details);

                        // ===============================
                        // UPDATE STOCK (ANTI RACE CONDITION)
                        // ===============================

                        foreach (var item in details)
                        {
                            decimal stockNeeded = item.TsdQuantity * item.TsdConversionRate;

                            int affected = _repo.ReduceStock(con, tran, item.ItemId, stockNeeded);

                            if (affected == 0)
                                throw new Exception($"Stock update failed for item {item.ItemId}");

                            decimal newStock = _repo.GetCurrentStock(con, tran, item.ItemId);

                            _repo.InsertStockLog(con, tran, new StockLog
                            {
                                ProductId = item.ItemId,
                                TipeTransaksi = "payment",
                                QtyMasuk = 0,
                                QtyKeluar = item.TsdQuantity,
                                SisaStock = newStock,
                                Keterangan = $"Payment Transaction #{transaction.TsNumbering}",
                                UserId = sessionUser.UserId,
                                CreatedAt = DateTime.Now,
                                LoginId = sessionUser.LoginId
                            });
                        }

                        // ===============================
                        // DELETE PENDING TRANSACTION
                        // ===============================

                        _repo.DeletePendingTransactions(con, tran, invoice.CartSessionCode);

                        //_repo.MarkPendingOrderPaid(con, tran, invoice.CartSessionCode);

                        // ===============================
                        // COMMIT
                        // ===============================

                        tran.Commit();

                        // Log AFTER commit (non critical)
                        _activityService.LogAction(
                            userId: sessionUser.UserId.ToString(),
                            actionType: "Transaction_Complete",
                            referenceId: transactionId,
                            details: new
                            {
                                Invoice = invoice,
                                Transaction = transaction,
                                Items = details
                            });

                        return TransactionResult.Success(invoice.ChangeAmount);
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        return TransactionResult.Fail(ex.Message);
                    }
                }
            }
        }




        /////////////////////////////////// CICILAN /////////////////////
        ///
        ///
        public TransactionResult ProcessInstallmentAndSave(
        InvoiceData invoice,
        decimal paymentAmount,
        string customerName,
        string note)
        {
            if (paymentAmount <= 0)
                return TransactionResult.Fail("Nominal tidak valid");

            decimal grandTotal = CalculateGrandTotal(invoice);

            if (paymentAmount > grandTotal)
                return TransactionResult.Fail("Nominal melebihi total");

            var sessionUser = SessionUser.GetCurrentUser();

            using (var con = new NpgsqlConnection(DbConfig.ConnectionString))
            {
                con.Open();

                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        decimal dueAmount = grandTotal - paymentAmount;

                        // ===============================
                        // BUILD TRANSACTION HEADER
                        // ===============================

                        Transactions transaction = new Transactions
                        {
                            TsNumbering = Utility.GenerateTransactionNumber(),
                            TsCode = Utility.getTrxNumbering(),
                            TsTotal = invoice.Items.Sum(x => x.Total),
                            TsPaymentAmount = paymentAmount,
                            TsCashback = 0,
                            TsMethod = "INSTALLMENT",
                            TsStatus = TransactionStatus.Partial,
                            TsChange = 0,
                            TsInternalNote = "Installment Transaction",
                            TsNote = note ?? "",
                            TsDiscountTotal = 0,
                            TsGrandTotal = grandTotal,
                            TsCustomer = null,
                            TsDelivery = invoice.DeliveryAmount,
                            TsFreename = customerName ?? "Guest",
                            UserId = sessionUser.UserId,
                            CreatedBy = sessionUser.UserId,
                            TerminalId = sessionUser.TerminalId,
                            ShiftId = sessionUser.ShiftId,
                            CreatedAt = DateTime.Now,
                            CartSessionCode = invoice.CartSessionCode,
                            TsDueAmount = dueAmount
                        };

                        int transactionId = _repo.InsertTransaction(con, tran, transaction);

                        if (transactionId <= 0)
                            throw new Exception("Gagal simpan transaksi cicilan");

                        // ===============================
                        // INSERT DETAILS
                        // ===============================

                        var details = invoice.Items.Select(i => new TransactionDetail
                        {
                            TsId = transactionId,
                            ItemId = i.ItemId,
                            Barcode = i.Barcode,
                            Name = i.Name,
                            TsdSellPrice = i.Price,
                            TsdBuyPrice = i.CostPrice,
                            TsdQuantity = i.Qty,
                            TsdUnit = i.Unit,
                            TsdConversionRate = i.ConversionRate,
                            TsdPricePerUnit = i.Price,
                            TsdUnitVariant = i.Unit,
                            TsdDiscountPerItem = i.Qty > 0 ? i.DiscountAmount / i.Qty : 0,
                            TsdDiscountPercentage = i.DiscountPercent,
                            TsdDiscountTotal = i.DiscountAmount,
                            TsdTax = i.Tax,
                            TsdTotal = i.Total,
                            TsdNote = i.Note,
                            CreatedBy = sessionUser.UserId,
                            CreatedAt = DateTime.Now,
                            CartSessionCode = invoice.CartSessionCode
                        }).ToList();

                        if (details.Count > 0)
                            _repo.InsertTransactionDetails(con, tran, details);

                        // ===============================
                        // INSERT CICILAN PERTAMA
                        // ===============================

                        _repo.InsertInstallment(
                            con, tran,
                            transactionId,
                            paymentAmount,
                            note,
                            sessionUser.UserId
                        );

                        // ===============================
                        // UPDATE STOCK + INSERT STOCK LOG
                        // ===============================

                        foreach (var item in details)
                        {
                            decimal stockNeeded = item.TsdQuantity * item.TsdConversionRate;

                            int affected = _repo.ReduceStock(con, tran, item.ItemId, stockNeeded);

                            if (affected == 0)
                                throw new Exception($"Stock gagal update item {item.ItemId}");

                            // 🔹 INSERT STOCK LOG
                            decimal newStock = _repo.GetCurrentStock(con, tran, item.ItemId);
                            _repo.InsertStockLog(con, tran, new StockLog
                            {
                                ProductId = item.ItemId,
                                TipeTransaksi = "installment",
                                QtyMasuk = 0,
                                QtyKeluar = item.TsdQuantity,
                                SisaStock = newStock,
                                Keterangan = $"Installment Transaction #{transaction.TsNumbering}",
                                UserId = sessionUser.UserId,
                                CreatedAt = DateTime.Now,
                                LoginId = sessionUser.LoginId
                            });

                        }

                        // ===============================
                        // DELETE PENDING TRANSACTIONS
                        // ===============================

                        _repo.DeletePendingTransactions(con, tran, invoice.CartSessionCode);

                        // ===============================
                        // COMMIT
                        // ===============================

                        tran.Commit();

                        // 🔹 LOG ACTIVITY
                        _activityService.LogAction(
                            userId: sessionUser.UserId.ToString(),
                            actionType: "Installment_Processed",
                            referenceId: transactionId,
                            details: new
                            {
                                Invoice = invoice,
                                Transaction = transaction,
                                Items = details
                            });

                        return TransactionResult.Success(0);
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        return TransactionResult.Fail(ex.Message);
                    }
                }
            }
        }

        public TransactionResult ProcessSplitPaymentAndSave(
            InvoiceData invoice,
            List<(string Method, decimal Amount)> parts
        )
        {
            if (invoice == null || !invoice.Items.Any())
                return TransactionResult.Fail("Invoice kosong.");
            if (parts == null || parts.Count == 0)
                return TransactionResult.Fail("Rincian pembayaran kosong.");

            decimal grandTotal = CalculateGrandTotal(invoice);
            decimal totalPaid = parts.Sum(p => p.Amount);
            if (totalPaid < grandTotal)
                return TransactionResult.Fail("Total pembayaran kurang dari grand total.");

            invoice.PaymentAmount = totalPaid;
            invoice.ChangeAmount = totalPaid - grandTotal;
            invoice.Status = "Paid";
            invoice.PaymentMethod = "SPLIT";

            var sessionUser = SessionUser.GetCurrentUser();
            using var con = new NpgsqlConnection(DbConfig.ConnectionString);
            con.Open();
            using var tran = con.BeginTransaction();
            try
            {
                Transactions transaction = new Transactions
                {
                    TsNumbering = Utility.GenerateTransactionNumber(),
                    TsCode = Utility.getTrxNumbering(),
                    TsTotal = invoice.Items.Sum(x => x.Total),
                    TsPaymentAmount = invoice.PaymentAmount,
                    TsCashback = invoice.ChangeAmount,
                    TsMethod = "SPLIT",
                    TsStatus = TransactionStatus.Paid,
                    TsChange = invoice.ChangeAmount,
                    TsInternalNote = "Split payment",
                    TsNote = invoice.GlobalNote ?? string.Empty,
                    TsDiscountTotal = 0,
                    TsGrandTotal = grandTotal,
                    TsCustomer = null,
                    TsDelivery = invoice.DeliveryAmount,
                    TsFreename = "Guest",
                    UserId = sessionUser.UserId,
                    CreatedBy = sessionUser.UserId,
                    TerminalId = sessionUser.TerminalId,
                    ShiftId = sessionUser.ShiftId,
                    CreatedAt = DateTime.Now,
                    OrderId = null,
                    CartSessionCode = invoice.CartSessionCode
                };

                int transactionId = _repo.InsertTransaction(con, tran, transaction);
                if (transactionId <= 0)
                    throw new Exception("Gagal insert transaksi split.");

                var details = invoice.Items.Select(i => new TransactionDetail
                {
                    TsId = transactionId,
                    ItemId = i.ItemId,
                    Barcode = i.Barcode,
                    Name = i.Name,
                    TsdSellPrice = i.Price,
                    TsdBuyPrice = i.CostPrice,
                    TsdQuantity = i.Qty,
                    TsdUnit = i.Unit,
                    TsdConversionRate = i.ConversionRate,
                    TsdPricePerUnit = i.Price,
                    TsdUnitVariant = i.Unit,
                    TsdDiscountPerItem = i.Qty > 0 ? i.DiscountAmount / i.Qty : 0,
                    TsdDiscountPercentage = i.DiscountPercent,
                    TsdDiscountTotal = i.DiscountAmount,
                    TsdTax = i.Tax,
                    TsdTotal = i.Total,
                    TsdNote = i.Note,
                    CreatedBy = sessionUser.UserId,
                    CreatedAt = DateTime.Now,
                    CartSessionCode = invoice.CartSessionCode
                }).ToList();

                if (details.Count > 0)
                    _repo.InsertTransactionDetails(con, tran, details);

                foreach (var item in details)
                {
                    decimal stockNeeded = item.TsdQuantity * item.TsdConversionRate;
                    int affected = _repo.ReduceStock(con, tran, item.ItemId, stockNeeded);
                    if (affected == 0)
                        throw new Exception($"Stock update failed for item {item.ItemId}");
                    decimal newStock = _repo.GetCurrentStock(con, tran, item.ItemId);
                    _repo.InsertStockLog(con, tran, new StockLog
                    {
                        ProductId = item.ItemId,
                        TipeTransaksi = "payment",
                        QtyMasuk = 0,
                        QtyKeluar = item.TsdQuantity,
                        SisaStock = newStock,
                        Keterangan = $"Split Payment #{transaction.TsNumbering}",
                        UserId = sessionUser.UserId,
                        CreatedAt = DateTime.Now,
                        LoginId = sessionUser.LoginId
                    });
                }

                _repo.DeletePendingTransactions(con, tran, invoice.CartSessionCode);
                tran.Commit();
                _activityService.LogAction(
                    userId: sessionUser.UserId.ToString(),
                    actionType: "Transaction_Complete_Split",
                    referenceId: transactionId,
                    details: new { Invoice = invoice, Parts = parts }
                );
                return TransactionResult.Success(invoice.ChangeAmount);
            }
            catch (Exception ex)
            {
                tran.Rollback();
                return TransactionResult.Fail(ex.Message);
            }
        }


        public List<InstallmentDto> GetAllInstallments()
        {
            var list = new List<InstallmentDto>();
            using var conn = new NpgsqlConnection(DbConfig.ConnectionString);
            conn.Open();

            using var cmd = new NpgsqlCommand(@"
        SELECT 
            ti.id AS TiId,
            t.ts_id AS TransactionId,
            t.ts_numbering,
            t.ts_code,
            t.ts_due_amount,
            t.ts_total,
            ti.amount AS PaidAmount,
            ti.note AS Note,
            ti.created_by,
            ti.created_at
        FROM transactions t
        LEFT JOIN transaction_installments ti 
            ON ti.transaction_id = t.ts_id
        WHERE t.ts_method = 'INSTALLMENT' 
          AND t.ts_status = 3
        ORDER BY t.created_at DESC, ti.created_at ASC
    ", conn);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new InstallmentDto
                {
                    Id = reader["TiId"] != DBNull.Value ? Convert.ToInt32(reader["TiId"]) : 0,
                    TransactionId = Convert.ToInt32(reader["TransactionId"]),
                    TransactionNumber = reader["ts_numbering"].ToString(),
                    TransactionCode = reader["ts_code"].ToString(),
                    DueAmount = reader["ts_due_amount"] != DBNull.Value ? Convert.ToDecimal(reader["ts_due_amount"]) : 0,
                    TotalAmount = reader["ts_total"] != DBNull.Value ? Convert.ToDecimal(reader["ts_total"]) : 0,
                    Amount = reader["PaidAmount"] != DBNull.Value ? Convert.ToDecimal(reader["PaidAmount"]) : 0,
                    Note = reader["Note"]?.ToString(),
                    CreatedByName = reader["created_by"] != DBNull.Value ? Utility.GetUserName(Convert.ToInt32(reader["created_by"])) : "",
                    CreatedAt = reader["created_at"] != DBNull.Value ? Convert.ToDateTime(reader["created_at"]) : DateTime.MinValue
                });
            }

            return list;
        }

        public List<InstallmentDto> GetInstallments(int transactionId)
        {
            var list = new List<InstallmentDto>();

            using var conn = new NpgsqlConnection(DbConfig.ConnectionString);
            conn.Open();

            // Ambil transaksi dulu untuk mendapatkan info total dan sisa
            Transactions transaction = null;
            using (var cmdTrx = new NpgsqlCommand(
                "SELECT ts_id, ts_numbering, ts_code, ts_grand_total, ts_due_amount " +
                "FROM transactions WHERE ts_id = @ts_id AND ts_method = 'INSTALLMENT'", conn))
            {
                cmdTrx.Parameters.AddWithValue("@ts_id", transactionId);
                using var reader = cmdTrx.ExecuteReader();
                if (reader.Read())
                {
                    transaction = new Transactions
                    {
                        TsId = reader.GetInt32(0),
                        TsNumbering = reader.GetString(1),
                        TsCode = reader.GetString(2),
                        TsGrandTotal = reader.GetDecimal(3),
                        TsDueAmount = reader.GetDecimal(4)
                    };
                }
            }

            if (transaction == null)
                return list; // transaksi cicilan tidak ditemukan

            // Ambil semua cicilan untuk transaksi ini
            using (var cmd = new NpgsqlCommand(
                "SELECT id, transaction_id, amount, note, created_by, created_at " +
                "FROM transaction_installments " +
                "WHERE transaction_id = @ts_id ORDER BY created_at ASC", conn))
            {
                cmd.Parameters.AddWithValue("@ts_id", transactionId);
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(new InstallmentDto
                    {
                        Id = reader.GetInt32(0),
                        TransactionId = reader.GetInt32(1),
                        Amount = reader.GetDecimal(2),
                        Note = reader["note"].ToString(),
                        CreatedByName = Utility.GetUserName(reader.GetInt32(4)), // helper cari nama user
                        CreatedAt = reader.GetDateTime(5),

                        // Tambahkan info transaksi untuk grid detail
                        TransactionNumber = transaction.TsNumbering,
                        TransactionCode = transaction.TsCode,
                        TotalAmount = transaction.TsGrandTotal,
                        DueAmount = transaction.TsDueAmount
                    });
                }
            }

            return list;
        }


        public void PayInstallment(int transactionId, decimal amount, string note, int userId)
        {
            // 1️⃣ Ambil transaksi
            var tx = _repo.GetTransactionById(transactionId);
            if (tx == null)
                throw new Exception("Transaksi tidak ditemukan");

            if (amount <= 0)
                throw new Exception("Nominal cicilan tidak valid");

            // 2️⃣ Insert cicilan baru
            _repo.InsertInstallment(transactionId, amount, note, userId);

            // 3️⃣ Update amount_paid & due_amount
            decimal newPaid = tx.AmountPaid + amount;
            decimal due = tx.TotalAmount - newPaid;
            TransactionStatus status = due <= 0
                ? TransactionStatus.Paid
                : (newPaid > 0 ? TransactionStatus.Partial : TransactionStatus.Unpaid);


            _repo.UpdateTransactionPayment(transactionId, newPaid, due, status);
        }


        //////////////////////////////////// END CICILAN //////////////







    }






    public class TransactionResult
    {
        public bool IsSuccess { get; private set; }
        public string Message { get; private set; }
        public decimal Change { get; private set; }

        public static TransactionResult Success(decimal change)
        {
            return new TransactionResult
            {
                IsSuccess = true,
                Change = change
            };
        }

        public static TransactionResult Fail(string message)
        {
            return new TransactionResult
            {
                IsSuccess = false,
                Message = message
            };
        }
    }


}
