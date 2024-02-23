using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Identity.Core.DTO.User
{
    internal class ValidateFile(string[] fileExtensions) : ValidationAttribute
    {
        private readonly string[] _fileExtensions = fileExtensions;

        public override bool IsValid(object? value)
        {
            if (value is not null && value is IFormFile formFile && formFile.FileName is not null)
            {
                return _fileExtensions.Contains(Path.GetExtension(formFile.FileName));
            }
            else
            {
                return true;
            }
        }
    }
}