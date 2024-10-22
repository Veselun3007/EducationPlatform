namespace CourseService.Application.Abstractions.Errors {
    public class Error {
        public string Code { get; }
        public string Message { get; }
        public int HttpCode { get; }

        public Error(string code, string message, int httpCode) {
            Code = code;
            Message = message;
            HttpCode = httpCode;
        }
    }
}
