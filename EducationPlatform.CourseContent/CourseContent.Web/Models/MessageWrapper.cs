using Identity.Core.Models;

namespace Identity.Web.Models
{
    public class MessageWrapper<T>
    {
        public T Result { get; }
        public string? ErrorMessage { get; }
        public DateTime TimeGenerated { get; }
        public int StatusCode { get; }

        protected internal MessageWrapper(T result, string? errorMessage, int statusCode = 200)
        {
            Result = result;
            ErrorMessage = errorMessage;
            TimeGenerated = DateTime.UtcNow;
            StatusCode = statusCode;
        }
    }

    public sealed class MessageWrapper : MessageWrapper<string?>
    {
        private MessageWrapper(string errorMessage, int statusCode = 200) : base(null, errorMessage, statusCode) { }

        public static MessageWrapper<T?> Ok<T>(T result, int statusCode = 200)
        {
            return new MessageWrapper<T?>(result, null, statusCode);
        }

        public static MessageWrapper Error(Error error, int statusCode = 400)
        {
            return error.Code switch
            {
                "record.not.found" => new MessageWrapper(error.Message, 404),
                "record.not.exist" => new MessageWrapper(error.Message, 400),
                "record.not.added" => new MessageWrapper(error.Message, 500),
                _ => new MessageWrapper(error.Message, statusCode),
            };
        }
    }
}
