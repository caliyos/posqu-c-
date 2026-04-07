using System.Collections.Generic;
using POS_qu.Models;
using POS_qu.DTO;

namespace POS_qu.Core.Interfaces
{
    public interface ITransactionService
    {
        decimal CalculateChangePreview(InvoiceData invoice, decimal paymentAmount);
        decimal CalculateGrandTotal(InvoiceData invoice);
        TransactionResult ProcessPayment(InvoiceData invoice, decimal cashAmount, string paymentMethod);
        TransactionResult ProcessPaymentAndSave(InvoiceData invoice, decimal paymentAmount, string paymentMethod);
        TransactionResult ProcessInstallmentAndSave(InvoiceData invoice, decimal paymentAmount, string customerName, string note);
        TransactionResult ProcessSplitPaymentAndSave(InvoiceData invoice, List<(string Method, decimal Amount)> parts);
        TransactionResult ProcessReturnAndSave(InvoiceData invoice, string note = "");
        List<InstallmentDto> GetAllInstallments();
        TransactionResult CancelTransaction(int tsId);
        List<InstallmentDto> GetInstallments(int transactionId);
        void PayInstallment(int transactionId, decimal amount, string note, int userId);
    }
}