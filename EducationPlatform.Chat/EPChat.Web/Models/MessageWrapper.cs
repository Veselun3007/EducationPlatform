namespace Chat.Web.Models
{
    internal class MessageWrapper<T>
    {
        public string? ErrorMessage { get; }
        public int StatusCode { get; }

        private MessageWrapper(string? errorMessage, int statusCode)
        {
            ErrorMessage = errorMessage;
            StatusCode = statusCode;
        }

        public static MessageWrapper<string?> Error(Error error)
        {
            var statusCode = error.Code switch
            {
                ErrorCodes.NotFound => 404,
                ErrorCodes.NotExist or ErrorCodes.ValidationFailed => 400,
                ErrorCodes.Unpredictable or _ => 500,
            };
            return new MessageWrapper<string?>(error.Message, statusCode);
        }
    }
}