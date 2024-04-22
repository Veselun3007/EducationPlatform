using CourseService.Application.Abstractions.Errors;

namespace CourseService.Application {
    public static partial class Errors {
        public static class Global {
            public static Error EmailIsTaken(string email) =>
                new Error("student.email.is.taken", $"Student email '{email}' is taken", 404);
        }
    }
}
