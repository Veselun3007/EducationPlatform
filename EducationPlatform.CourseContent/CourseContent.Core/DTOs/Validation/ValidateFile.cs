﻿using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace CourseContent.Core.DTOs.Validation
{

    internal class ValidateFile(string[] fileExtensions) : ValidationAttribute
    {
        private readonly string[] _fileExtensions = fileExtensions;

        public override bool IsValid(object? value)
        {
            if (value is List<IFormFile> formFiles && formFiles is not null)
            {
                foreach (var formFile in formFiles)
                {
                    return _fileExtensions.Contains(Path.GetExtension(formFile.FileName));
                }
            }

            return false;
        }
    }

}
