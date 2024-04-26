﻿using CourseService.Application.Abstractions.Errors;

namespace CourseService.Application {
    public static partial class Errors {
        public static class CourseuserError {
            public static Error TestError() =>
                new Error("test.error", $"Test error", 400);
        }
    }
}