namespace StudentResult.Application.Abstractions.Errors {
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
            public static Error Error400() =>
                new Error("400.error", $"400 error", 400);
        }
    }
}
