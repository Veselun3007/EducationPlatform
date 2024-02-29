using Identity.Core.Models;

namespace Identity.Web.Utils
{
    public class Envelope<T>
    {
        public T Result { get; }
        public string ErrorMessage { get; }
        public DateTime TimeGenerated { get; }
        public int StatusCode { get; }

        protected internal Envelope(T result, string errorMessage, int statusCode = 200)
        {
            Result = result;
            ErrorMessage = errorMessage;
            TimeGenerated = DateTime.UtcNow;
            StatusCode = statusCode;
        }
    }

    public sealed class Envelope : Envelope<string>
    {
        private Envelope(string errorMessage, int statusCode = 200) : base(null, errorMessage, statusCode) { }

        public static Envelope<T> Ok<T>(T result, int statusCode = 200)
        {
            return new Envelope<T>(result, null, statusCode);
        }


        public static Envelope Error(Error error, int statusCode = 400)
        {
            return error.Code switch
            {
                "identity.username.exist" => new Envelope(error.Message, 409),
                "record.not.found" => new Envelope(error.Message, 404),
                "not.authorized" => new Envelope(error.Message, 401),
                "identity.code.mismatch" or "identity.invalid.password" or "identity.code.expired" => new Envelope(error.Message, 400),
                _ => new Envelope(error.Message, statusCode),
            };
        }

    }
}
