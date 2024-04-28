namespace CourseContent.Core.Models.ErrorModels
{
    public sealed class Error
    {
        public string Code { get; }
        public string Message { get; }

        internal Error(string code, string message)
        {
            Code = code;
            Message = message;
        }
    }
}
