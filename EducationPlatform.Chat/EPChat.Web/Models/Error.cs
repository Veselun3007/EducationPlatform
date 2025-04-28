namespace Chat.Web.Models
{
    internal class Error
    {
        public string Code { get; }
        public string Message { get; }

        internal Error(string code, string message)
        {
            Code = code;
            Message = message;
        }
    }

    internal static class Errors
    {
        public static Error ValidationFailed(string? details = null)
        {
            return new(ErrorCodes.ValidationFailed, details ?? "Validation failed. One or more fields are invalid.");
        }

        public static Error NotFound(string? details = null)
        {
            return new(ErrorCodes.NotFound, details ?? "The requested record was not found.");
        }

        public static Error NotExist(string? details = null)
        {
            return new(ErrorCodes.NotExist, details ?? "No records were provided for removal.");
        }

        public static Error Unpredictable(string? details = null)
        {
            return new(ErrorCodes.Unpredictable, details ?? "An unexpected error occurred. Please try again later.");
        }
    }

    internal static class ErrorCodes
    {
        public const string ValidationFailed = "validation.failed";
        public const string NotFound = "record.not.found";
        public const string NotExist = "record.not.exist";
        public const string Unpredictable = "unpredictable";
    }
}
