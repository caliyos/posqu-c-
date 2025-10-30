namespace POS_qu.Core
{
    public interface IStockAdjustmentService
    {
        void LogAdjustment(long itemId, string adjustmentType, decimal oldStock, decimal newStock, string reason, long? referenceId = null, string? referenceTable = null, long? userId = null);
    }
}
