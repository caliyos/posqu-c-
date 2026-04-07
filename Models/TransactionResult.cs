namespace POS_qu.Models
{
    public class TransactionResult
    {
        public bool IsSuccess { get; private set; }
        public string Message { get; private set; }
        public decimal Change { get; private set; }

        public static TransactionResult Success(decimal change)
        {
            return new TransactionResult { IsSuccess = true, Message = "Success", Change = change };
        }

        public static TransactionResult Fail(string message)
        {
            return new TransactionResult { IsSuccess = false, Message = message, Change = 0 };
        }
    }
}