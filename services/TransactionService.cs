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


using POS_qu.Core.Interfaces;
using POS_qu.Models;
using POS_qu.DTO;
using POS_qu.Repositories;

namespace POS_qu.Services
{
    public class TransactionService : ITransactionService
    {

        private readonly IActivityService _activityService;
        public readonly TransactionRepo _repo;
        private readonly IStockValuationStrategy _stockValuationStrategy;

        public TransactionService(
    TransactionRepo repo,
    IActivityService activityService,
    IStockValuationStrategy stockValuationStrategy = null)
        {
            _repo = repo;
            _activityService = activityService;
            _stockValuationStrategy = stockValuationStrategy ?? new POS_qu.Services.StockValuation.FifoStrategy();
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
            if (invoice == null || invoice.Items == null) return 0m;

            decimal subTotal = 0m;
            decimal itemDiscount = 0m;
            foreach (var i in invoice.Items)
            {
                decimal lineSub = i.Price * i.Qty;
                if (lineSub < 0m) lineSub = 0m;
                subTotal += lineSub;

                decimal disc = i.DiscountAmount;
                if (disc < 0m) disc = 0m;
                if (disc > lineSub) disc = lineSub;
                itemDiscount += disc;
            }

            decimal net = subTotal - itemDiscount;
            if (net < 0m) net = 0m;

            decimal globalDiscValue;
            if (invoice.GlobalDiscountIsAmount)
            {
                globalDiscValue = invoice.GlobalDiscountValue;
            }
            else
            {
                var p = invoice.GlobalDiscountPercent;
                if (p < 0m) p = 0m;
                if (p > 100m) p = 100m;
                globalDiscValue = Math.Round((net * p) / 100m, 2, MidpointRounding.AwayFromZero);
            }

            if (globalDiscValue < 0m) globalDiscValue = 0m;
            if (globalDiscValue > net) globalDiscValue = net;

            var delivery = invoice.DeliveryAmount;
            if (delivery < 0m) delivery = 0m;

            return (net - globalDiscValue) + delivery;
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
            var promoService = new PromotionService();
            promoService.ApplyBestPromotion(invoice);

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
                        var numberingService = new POS_qu.Services.DocumentNumberingService();
                        var saleNo = numberingService.Generate("SALE", DateTime.Now.Date, con, tran);
                        int warehouseId = invoice != null && invoice.WarehouseId > 0 ? invoice.WarehouseId : (sessionUser.WarehouseId <= 0 ? 1 : sessionUser.WarehouseId);
                        var netAfterItemDiscount = invoice.Items.Sum(x => x.Total);
                        if (netAfterItemDiscount < 0m) netAfterItemDiscount = 0m;
                        decimal globalDiscValue;
                        if (invoice.GlobalDiscountIsAmount)
                        {
                            globalDiscValue = invoice.GlobalDiscountValue;
                        }
                        else
                        {
                            var p = invoice.GlobalDiscountPercent;
                            if (p < 0m) p = 0m;
                            if (p > 100m) p = 100m;
                            globalDiscValue = Math.Round((netAfterItemDiscount * p) / 100m, 2, MidpointRounding.AwayFromZero);
                        }
                        if (globalDiscValue < 0m) globalDiscValue = 0m;
                        if (globalDiscValue > netAfterItemDiscount) globalDiscValue = netAfterItemDiscount;
                        var grandTotal = CalculateGrandTotal(invoice);

                        // ===============================
                        // BUILD TRANSACTION OBJECT
                        // ===============================

                        Transactions transaction = new Transactions
                        {
                            TsNumbering = saleNo,
                            TsCode = Utility.getTrxNumbering(),
                            TsTotal = invoice.Items.Sum(x => x.Total),
                            WarehouseId = warehouseId,
                            TsPaymentAmount = invoice.PaymentAmount,
                            TsCashback = invoice.ChangeAmount,
                            TsMethod = invoice.PaymentMethod,
                            TsStatus = TransactionStatus.Paid,
                            TsChange = invoice.ChangeAmount,
                            TsInternalNote = "Processed via POS system",
                            TsNote = invoice.GlobalNote ?? string.Empty,
                            TsDiscountTotal = globalDiscValue,
                            TsGrandTotal = grandTotal,
                            TsTotalBeforeTax = netAfterItemDiscount,
                            TsTaxMode = "NON",
                            TsTaxRate = 0,
                            TsTaxAmount = 0,
                            TsCustomer = invoice.CustomerId,
                            TsDelivery = invoice.DeliveryAmount,
                            TsFreename = string.IsNullOrWhiteSpace(invoice.CustomerName) ? "Guest" : invoice.CustomerName,
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
                            WarehouseId = warehouseId,
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

                        details = DeductStockFillBuyPriceAndLog(
                            con,
                            tran,
                            details,
                            warehouseId: warehouseId,
                            transactionId: transactionId,
                            transactionNumbering: transaction.TsNumbering,
                            userId: sessionUser.UserId,
                            loginId: sessionUser.LoginId
                        );

                        if (details.Count > 0)
                            _repo.InsertTransactionDetails(con, tran, details);

                        var journal = new JournalService();
                        journal.CreateJournalFromSale(transactionId, con, tran);

                        try
                        {
                            var loyalty = new LoyaltyService();
                            var earn = loyalty.ApplyEarnForSale(
                                con,
                                tran,
                                customerId: invoice.CustomerId,
                                transactionId: transactionId,
                                invoiceNumber: transaction.TsNumbering,
                                eligibleSpend: grandTotal,
                                warehouseId: warehouseId,
                                terminalId: sessionUser.TerminalId,
                                cashierUserId: sessionUser.UserId,
                                loginId: sessionUser.LoginId
                            );
                            invoice.MembershipLevel = earn.MembershipCode;
                            invoice.EarnedPoints = earn.EarnedPoints;
                            invoice.PointBalance = earn.BalanceAfter;
                        }
                        catch
                        {
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

                        var numberingService = new POS_qu.Services.DocumentNumberingService();
                        var saleNo = numberingService.Generate("SALE", DateTime.Now.Date, con, tran);
                        int warehouseId = invoice != null && invoice.WarehouseId > 0 ? invoice.WarehouseId : (sessionUser.WarehouseId <= 0 ? 1 : sessionUser.WarehouseId);
                        var netAfterItemDiscount = invoice.Items.Sum(x => x.Total);
                        if (netAfterItemDiscount < 0m) netAfterItemDiscount = 0m;
                        decimal globalDiscValue;
                        if (invoice.GlobalDiscountIsAmount)
                        {
                            globalDiscValue = invoice.GlobalDiscountValue;
                        }
                        else
                        {
                            var p = invoice.GlobalDiscountPercent;
                            if (p < 0m) p = 0m;
                            if (p > 100m) p = 100m;
                            globalDiscValue = Math.Round((netAfterItemDiscount * p) / 100m, 2, MidpointRounding.AwayFromZero);
                        }
                        if (globalDiscValue < 0m) globalDiscValue = 0m;
                        if (globalDiscValue > netAfterItemDiscount) globalDiscValue = netAfterItemDiscount;

                        // ===============================
                        // BUILD TRANSACTION HEADER
                        // ===============================

                        Transactions transaction = new Transactions
                        {
                            TsNumbering = saleNo,
                            TsCode = Utility.getTrxNumbering(),
                            TsTotal = invoice.Items.Sum(x => x.Total),
                            WarehouseId = warehouseId,
                            TsPaymentAmount = paymentAmount,
                            TsCashback = 0,
                            TsMethod = "INSTALLMENT",
                            TsStatus = TransactionStatus.Partial,
                            TsChange = 0,
                            TsInternalNote = "Installment Transaction",
                            TsNote = note ?? "",
                            TsDiscountTotal = globalDiscValue,
                            TsGrandTotal = grandTotal,
                            TsTotalBeforeTax = netAfterItemDiscount,
                            TsTaxMode = "NON",
                            TsTaxRate = 0,
                            TsTaxAmount = 0,
                            TsCustomer = invoice.CustomerId,
                            TsDelivery = invoice.DeliveryAmount,
                            TsFreename = string.IsNullOrWhiteSpace(invoice.CustomerName) ? (customerName ?? "Guest") : invoice.CustomerName,
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
                            WarehouseId = warehouseId,
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

                        details = DeductStockFillBuyPriceAndLog(
                            con,
                            tran,
                            details,
                            warehouseId: warehouseId,
                            transactionId: transactionId,
                            transactionNumbering: transaction.TsNumbering,
                            userId: sessionUser.UserId,
                            loginId: sessionUser.LoginId
                        );

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
                var numberingService = new POS_qu.Services.DocumentNumberingService();
                var saleNo = numberingService.Generate("SALE", DateTime.Now.Date, con, tran);
                int warehouseId = invoice != null && invoice.WarehouseId > 0 ? invoice.WarehouseId : (sessionUser.WarehouseId <= 0 ? 1 : sessionUser.WarehouseId);
                var netAfterItemDiscount = invoice.Items.Sum(x => x.Total);
                if (netAfterItemDiscount < 0m) netAfterItemDiscount = 0m;
                decimal globalDiscValue;
                if (invoice.GlobalDiscountIsAmount)
                {
                    globalDiscValue = invoice.GlobalDiscountValue;
                }
                else
                {
                    var p = invoice.GlobalDiscountPercent;
                    if (p < 0m) p = 0m;
                    if (p > 100m) p = 100m;
                    globalDiscValue = Math.Round((netAfterItemDiscount * p) / 100m, 2, MidpointRounding.AwayFromZero);
                }
                if (globalDiscValue < 0m) globalDiscValue = 0m;
                if (globalDiscValue > netAfterItemDiscount) globalDiscValue = netAfterItemDiscount;

                Transactions transaction = new Transactions
                {
                    TsNumbering = saleNo,
                    TsCode = Utility.getTrxNumbering(),
                    TsTotal = invoice.Items.Sum(x => x.Total),
                    WarehouseId = warehouseId,
                    TsPaymentAmount = invoice.PaymentAmount,
                    TsCashback = invoice.ChangeAmount,
                    TsMethod = "SPLIT",
                    TsStatus = TransactionStatus.Paid,
                    TsChange = invoice.ChangeAmount,
                    TsInternalNote = "Split payment",
                    TsNote = invoice.GlobalNote ?? string.Empty,
                    TsDiscountTotal = globalDiscValue,
                    TsGrandTotal = grandTotal,
                    TsTotalBeforeTax = netAfterItemDiscount,
                    TsTaxMode = "NON",
                    TsTaxRate = 0,
                    TsTaxAmount = 0,
                    TsCustomer = invoice.CustomerId,
                    TsDelivery = invoice.DeliveryAmount,
                    TsFreename = string.IsNullOrWhiteSpace(invoice.CustomerName) ? "Guest" : invoice.CustomerName,
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
                    WarehouseId = warehouseId,
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

                details = DeductStockFillBuyPriceAndLog(
                    con,
                    tran,
                    details,
                    warehouseId: warehouseId,
                    transactionId: transactionId,
                    transactionNumbering: transaction.TsNumbering,
                    userId: sessionUser.UserId,
                    loginId: sessionUser.LoginId
                );

                if (details.Count > 0)
                    _repo.InsertTransactionDetails(con, tran, details);

                try
                {
                    var loyalty = new LoyaltyService();
                    var earn = loyalty.ApplyEarnForSale(
                        con,
                        tran,
                        customerId: invoice.CustomerId,
                        transactionId: transactionId,
                        invoiceNumber: transaction.TsNumbering,
                        eligibleSpend: grandTotal,
                        warehouseId: warehouseId,
                        terminalId: sessionUser.TerminalId,
                        cashierUserId: sessionUser.UserId,
                        loginId: sessionUser.LoginId
                    );
                    invoice.MembershipLevel = earn.MembershipCode;
                    invoice.EarnedPoints = earn.EarnedPoints;
                    invoice.PointBalance = earn.BalanceAfter;
                }
                catch
                {
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

        public TransactionResult ProcessReturnAndSave(InvoiceData invoice, string note = "")
        {
            if (invoice == null || !invoice.Items.Any())
                return TransactionResult.Fail("Daftar retur kosong.");

            var sessionUser = SessionUser.GetCurrentUser();

            decimal totalRefund = invoice.Items.Sum(i => i.Price * i.Qty);
            decimal grandTotal = totalRefund; // positive sum of items being returned

            using var con = new NpgsqlConnection(DbConfig.ConnectionString);
            con.Open();
            using var tran = con.BeginTransaction();
            try
            {
                var numberingService = new POS_qu.Services.DocumentNumberingService();
                var saleNo = numberingService.Generate("SALE", DateTime.Now.Date, con, tran);
                int warehouseId = invoice != null && invoice.WarehouseId > 0 ? invoice.WarehouseId : (sessionUser.WarehouseId <= 0 ? 1 : sessionUser.WarehouseId);

                Transactions transaction = new Transactions
                {
                    TsNumbering = saleNo,
                    TsCode = Utility.getTrxNumbering(),
                    TsTotal = -grandTotal,
                    WarehouseId = warehouseId,
                    TsPaymentAmount = 0,
                    TsCashback = 0,
                    TsMethod = "RETURN",
                    TsStatus = TransactionStatus.Paid,
                    TsChange = 0,
                    TsInternalNote = "Return Transaction",
                    TsNote = note ?? (invoice.GlobalNote ?? ""),
                    TsDiscountTotal = 0,
                    TsGrandTotal = -grandTotal,
                    TsTotalBeforeTax = -grandTotal,
                    TsTaxMode = "NON",
                    TsTaxRate = 0,
                    TsTaxAmount = 0,
                    TsCustomer = null,
                    TsDelivery = 0,
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
                    throw new Exception("Gagal membuat transaksi retur.");

                var details = invoice.Items.Select(i => new TransactionDetail
                {
                    TsId = transactionId,
                    ItemId = i.ItemId, // 0 untuk custom
                    WarehouseId = warehouseId,
                    Barcode = i.Barcode,
                    Name = i.Name,
                    TsdSellPrice = i.Price,
                    TsdBuyPrice = i.CostPrice,
                    TsdQuantity = i.Qty,
                    TsdUnit = i.Unit ?? "pcs",
                    TsdConversionRate = i.ConversionRate <= 0 ? 1 : i.ConversionRate,
                    TsdPricePerUnit = i.Price,
                    TsdUnitVariant = string.IsNullOrEmpty(i.UnitVariant) ? (i.Unit ?? "pcs") : i.UnitVariant,
                    TsdDiscountPerItem = 0,
                    TsdDiscountPercentage = 0,
                    TsdDiscountTotal = 0,
                    TsdTax = 0,
                    TsdTotal = i.Price * i.Qty,
                    TsdNote = string.IsNullOrWhiteSpace(i.Note) ? "RETURN" : i.Note,
                    CreatedBy = sessionUser.UserId,
                    CreatedAt = DateTime.Now,
                    CartSessionCode = invoice.CartSessionCode
                }).ToList();

                if (details.Count > 0)
                    _repo.InsertTransactionDetails(con, tran, details);

                foreach (var d in details)
                {
                    if (d.ItemId > 0 && _repo.IsInventoryItem(con, tran, d.ItemId))
                    {
                        decimal conv = d.TsdConversionRate <= 0 ? 1 : d.TsdConversionRate;
                        decimal stockIn = d.TsdQuantity * conv;
                        
                        string valMethod = _repo.GetItemValuationMethod(con, tran, d.ItemId);
                        IStockValuationStrategy strategy = CreateStockValuationStrategy(valMethod);

                        decimal unitCostPerBase = stockIn != 0 ? (d.TsdBuyPrice / conv) : 0m;
                        strategy.AddStockIn(d.ItemId, warehouseId, stockIn, unitCostPerBase, con, tran);

                        decimal newStock = _repo.GetCurrentStockInWarehouse(con, tran, d.ItemId, warehouseId);
                        _repo.InsertStockLog(con, tran, new StockLog
                        {
                            ProductId = d.ItemId,
                            TipeTransaksi = "return",
                            QtyMasuk = stockIn,
                            QtyKeluar = 0,
                            SisaStock = newStock,
                            Keterangan = $"Return Transaction #{transaction.TsNumbering}",
                            UserId = sessionUser.UserId,
                            CreatedAt = DateTime.Now,
                            LoginId = sessionUser.LoginId,
                            WarehouseId = warehouseId,
                            RefType = "RETURN",
                            RefId = transactionId,
                            UnitCost = unitCostPerBase,
                            Method = valMethod
                        });
                    }
                }

                tran.Commit();
                _activityService.LogAction(
                    userId: sessionUser.UserId.ToString(),
                    actionType: "Return_Complete",
                    referenceId: transactionId,
                    details: new { Invoice = invoice }
                );
                return TransactionResult.Success(0);
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

        public TransactionResult CancelTransaction(int tsId)
        {
            var sessionUser = SessionUser.GetCurrentUser();
            using var con = new NpgsqlConnection(DbConfig.ConnectionString);
            con.Open();
            using var tran = con.BeginTransaction();
            try
            {
                var txInfo = _repo.GetTransactionById(tsId);
                if (txInfo == null)
                    return TransactionResult.Fail("Transaksi tidak ditemukan.");

                var details = _repo.GetTransactionDetailsById(tsId);
                foreach (System.Data.DataRow r in details.Rows)
                {
                    int itemId = r["item_id"] == DBNull.Value ? 0 : Convert.ToInt32(r["item_id"]);
                    if (itemId > 0 && _repo.IsInventoryItem(con, tran, itemId))
                    {
                        decimal qty = Convert.ToDecimal(r["tsd_quantity"]);
                        decimal conv = r["tsd_conversion_rate"] == DBNull.Value ? 1 : Convert.ToDecimal(r["tsd_conversion_rate"]);
                        decimal stockIn = qty * conv;
                        int warehouseId = details.Columns.Contains("warehouse_id") && r["warehouse_id"] != DBNull.Value
                            ? Convert.ToInt32(r["warehouse_id"])
                            : 1;

                        decimal buyPerSoldUnit = details.Columns.Contains("tsd_buy_price") && r["tsd_buy_price"] != DBNull.Value
                            ? Convert.ToDecimal(r["tsd_buy_price"])
                            : 0m;
                        decimal unitCostPerBase = conv != 0 ? (buyPerSoldUnit / conv) : buyPerSoldUnit;

                        string valMethod = _repo.GetItemValuationMethod(con, tran, itemId);
                        IStockValuationStrategy strategy = CreateStockValuationStrategy(valMethod);
                        strategy.AddStockIn(itemId, warehouseId, stockIn, unitCostPerBase, con, tran);

                        decimal newStock = _repo.GetCurrentStockInWarehouse(con, tran, itemId, warehouseId);
                        _repo.InsertStockLog(con, tran, new StockLog
                        {
                            ProductId = itemId,
                            TipeTransaksi = "cancel",
                            QtyMasuk = stockIn,
                            QtyKeluar = 0,
                            SisaStock = newStock,
                            Keterangan = $"Cancel Transaction #{txInfo.TransactionNumber}",
                            UserId = sessionUser.UserId,
                            CreatedAt = DateTime.Now,
                            LoginId = sessionUser.LoginId,
                            WarehouseId = warehouseId,
                            RefType = "CANCEL",
                            RefId = tsId,
                            UnitCost = unitCostPerBase,
                            Method = valMethod
                        });
                    }
                }

                _repo.SoftCancelTransaction(con, tran, tsId);
                tran.Commit();
                _activityService.LogAction(
                    userId: sessionUser.UserId.ToString(),
                    actionType: "Transaction_Cancelled",
                    referenceId: tsId,
                    details: new { TransactionId = tsId }
                );
                return TransactionResult.Success(0);
            }
            catch (Exception ex)
            {
                tran.Rollback();
                return TransactionResult.Fail(ex.Message);
            }
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

        private static decimal RoundMoney(decimal v) =>
            Math.Round(v, 2, MidpointRounding.AwayFromZero);

        private List<TransactionDetail> DeductStockFillBuyPriceAndLog(
            NpgsqlConnection con,
            NpgsqlTransaction tran,
            List<TransactionDetail> details,
            int warehouseId,
            int transactionId,
            string transactionNumbering,
            int userId,
            int? loginId)
        {
            var resultDetails = new List<TransactionDetail>(details?.Count ?? 0);
            if (details == null || details.Count == 0) return resultDetails;

            foreach (var d in details)
            {
                if (d == null)
                    continue;

                if (d.ItemId <= 0 || !_repo.IsInventoryItem(con, tran, d.ItemId))
                {
                    resultDetails.Add(d);
                    continue;
                }

                decimal conv = d.TsdConversionRate <= 0 ? 1 : d.TsdConversionRate;
                decimal qtyBase = d.TsdQuantity * conv;
                if (qtyBase == 0)
                {
                    resultDetails.Add(d);
                    continue;
                }

                _repo.ReleaseReservedStockInWarehouse(con, tran, d.ItemId, warehouseId, qtyBase);

                string method = _repo.GetItemValuationMethod(con, tran, d.ItemId);
                var strategy = CreateStockValuationStrategy(method);
                var deduction = strategy.CalculateCOGSAndDeductStock(d.ItemId, warehouseId, qtyBase, con, tran);

                decimal newStock = _repo.GetCurrentStockInWarehouse(con, tran, d.ItemId, warehouseId);
                int parentId = _repo.InsertStockLog(con, tran, new StockLog
                {
                    ProductId = d.ItemId,
                    TipeTransaksi = "transaction",
                    QtyMasuk = 0,
                    QtyKeluar = qtyBase,
                    SisaStock = newStock,
                    Keterangan = $"Transaction #{transactionNumbering}",
                    UserId = userId,
                    CreatedAt = DateTime.Now,
                    LoginId = loginId,
                    WarehouseId = warehouseId,
                    RefType = "SALE",
                    RefId = transactionId,
                    Method = method,
                    IsAllocation = false
                });

                foreach (var line in deduction.Lines)
                {
                    if (line.Qty == 0) continue;
                    _repo.InsertStockLog(con, tran, new StockLog
                    {
                        ProductId = d.ItemId,
                        TipeTransaksi = "transaction",
                        QtyMasuk = 0,
                        QtyKeluar = line.Qty,
                        SisaStock = newStock,
                        Keterangan = null,
                        UserId = userId,
                        CreatedAt = DateTime.Now,
                        LoginId = loginId,
                        WarehouseId = warehouseId,
                        RefType = "SALE",
                        RefId = transactionId,
                        UnitCost = line.UnitCost,
                        Method = method,
                        StockLayerId = line.StockLayerId,
                        IsAllocation = true,
                        ParentId = parentId
                    });
                }

                var lines = deduction.Lines
                    .Where(l => l != null && l.Qty != 0)
                    .ToList();

                if (lines.Count == 0)
                {
                    d.TsdBuyPrice = 0m;
                    resultDetails.Add(d);
                    continue;
                }

                var groups = lines
                    .GroupBy(l => l.UnitCost)
                    .Select(g => new { UnitCost = g.Key, QtyBase = g.Sum(x => x.Qty) })
                    .Where(x => x.QtyBase != 0)
                    .OrderBy(x => x.UnitCost)
                    .ToList();

                if (groups.Count <= 1)
                {
                    var one = groups[0];
                    d.TsdBuyPrice = RoundMoney(one.UnitCost * conv);
                    resultDetails.Add(d);
                    continue;
                }

                decimal totalQtyBase = groups.Sum(x => x.QtyBase);
                if (totalQtyBase == 0)
                {
                    decimal unitCostPerSoldUnit = d.TsdQuantity != 0 ? (deduction.TotalCogs / d.TsdQuantity) : 0m;
                    d.TsdBuyPrice = RoundMoney(unitCostPerSoldUnit);
                    resultDetails.Add(d);
                    continue;
                }

                decimal sumDisc = 0m;
                decimal sumTax = 0m;
                decimal sumTotal = 0m;

                for (int i = 0; i < groups.Count; i++)
                {
                    var g = groups[i];
                    bool isLast = i == groups.Count - 1;
                    decimal ratio = g.QtyBase / totalQtyBase;

                    decimal qtyUnit = conv != 0 ? (g.QtyBase / conv) : 0m;
                    if (qtyUnit == 0) continue;

                    decimal disc = isLast ? (d.TsdDiscountTotal - sumDisc) : RoundMoney(d.TsdDiscountTotal * ratio);
                    decimal tax = isLast ? (d.TsdTax - sumTax) : RoundMoney(d.TsdTax * ratio);
                    decimal total = isLast ? (d.TsdTotal - sumTotal) : RoundMoney(d.TsdTotal * ratio);

                    sumDisc += disc;
                    sumTax += tax;
                    sumTotal += total;

                    var nd = new TransactionDetail
                    {
                        TsId = d.TsId,
                        ItemId = d.ItemId,
                        WarehouseId = d.WarehouseId,
                        Name = d.Name,
                        Barcode = d.Barcode,
                        TsdSellPrice = d.TsdSellPrice,
                        TsdBuyPrice = RoundMoney(g.UnitCost * conv),
                        TsdQuantity = qtyUnit,
                        TsdUnit = d.TsdUnit,
                        TsdNote = d.TsdNote,
                        TsdDiscountPercentage = d.TsdDiscountPercentage,
                        TsdDiscountTotal = disc,
                        TsdTax = tax,
                        TsdTotal = total,
                        TsdConversionRate = d.TsdConversionRate,
                        TsdPricePerUnit = d.TsdPricePerUnit,
                        TsdUnitVariant = d.TsdUnitVariant,
                        CreatedBy = d.CreatedBy,
                        CreatedAt = d.CreatedAt,
                        CartSessionCode = d.CartSessionCode
                    };

                    nd.TsdDiscountPerItem = qtyUnit != 0 ? RoundMoney(disc / qtyUnit) : 0m;
                    resultDetails.Add(nd);
                }
            }

            return resultDetails;
        }

        private static IStockValuationStrategy CreateStockValuationStrategy(string method)
        {
            try
            {
                var active = new POS_qu.Controllers.SettingController().GetActiveHppMethods();
                if (active.Count == 1 && string.Equals(active[0], "FIFO", StringComparison.OrdinalIgnoreCase))
                    return new POS_qu.Services.StockValuation.FifoStrategy();
            }
            catch
            {
            }
            method = (method ?? "FIFO").Trim().ToUpperInvariant();
            if (method == "AVG") return new POS_qu.Services.StockValuation.AverageStrategy();
            if (method == "LIFO") return new POS_qu.Services.StockValuation.LifoStrategy();
            if (method == "FEFO") return new POS_qu.Services.StockValuation.FefoStrategy();
            return new POS_qu.Services.StockValuation.FifoStrategy();
        }

    }
}
