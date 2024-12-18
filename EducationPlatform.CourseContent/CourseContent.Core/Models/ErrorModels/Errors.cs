﻿namespace CourseContent.Core.Models.ErrorModels
{
    public static class Errors
    {
        public static class General
        {
            public static Error NotFound() =>
                new("record.not.found", "No object with this key was found");

            public static Error NotRecords() =>
                new("record.not.exist", "No entities provided for removal.");

            public static Error NotAdded() =>
                new("record.not.added", "Entities not added.");
        }
    }
}
