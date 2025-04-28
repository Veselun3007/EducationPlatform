namespace Chat.Web.Models
{
    internal class Error
    {
        public int Code { get; }
        public string Message { get; }

        internal Error(int code, string message)
        {
            Code = code;
            Message = message;
        }
    }

    internal static class Errors
    {
        public static Error ValidationFailed()
        {
            return new(ErrorCodes.ValidationFailed, "Validation failed. One or more fields are invalid.");
        }

        public static Error NotFound()
        {
            return new(ErrorCodes.NotFound, "The requested record was not found.");
        }

        public static Error NotExist()
        {
            return new(ErrorCodes.NotExist, "No records were provided for removal.");
        }

        public static Error Unpredictable()
        {
            return new(ErrorCodes.Unpredictable, "Something went wrong, contact support team to resolve the problem.");
        }
    }


    internal static class ErrorCodes
    {
        public const int ValidationFailed = 400;
        public const int NotFound = 404;
        public const int NotExist = 400;
        public const int Unpredictable = 500;
    }
}
