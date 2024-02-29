using CSharpFunctionalExtensions;

namespace Identity.Core.DTO.Responses
{
    public class HttpResponseResult<T>(int httpStatusCode, Result<T?> result)
    {
        public int HttpStatusCode { get; set; } = httpStatusCode;

        public T? Result { get; set; } = result.Value;
    }
}
