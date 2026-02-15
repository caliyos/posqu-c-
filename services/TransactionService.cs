using Npgsql;
using POS_qu.Controllers;
using POS_qu.Core;
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
                            TsStatus = 1,
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
