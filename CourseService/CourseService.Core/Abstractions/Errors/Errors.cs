using CourseService.Application.Abstractions.Errors;

namespace CourseService.Application {
    public static partial class Errors {
        public static class Global {
            public static Error TestError() =>
                new Error("test.error", $"Test error", 400);
            public static Error Error403() =>
                new Error("403.error", $"403 error", 403);
            public static Error Error404() =>
                new Error("404.error", $"404 error", 404);
            public static Error Error409() =>
                new Error("409.error", $"409 error", 409);
            public static Error EmailIsTaken(string email) =>
                new Error("student.email.is.taken", $"Student email '{email}' is taken", 404);
        }
    }
}
