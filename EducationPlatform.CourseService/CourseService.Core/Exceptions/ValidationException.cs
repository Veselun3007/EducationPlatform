namespace CourseService.Application.Exceptions {
    public sealed class ValidationException : ApplicationException {
        public ValidationException(IReadOnlyDictionary<string, string[]> errorsDictionary)
            : base("Validation Failure")
            => ErrorsDictionary = errorsDictionary;

        public IReadOnlyDictionary<string, string[]> ErrorsDictionary { get; }
    }
}
