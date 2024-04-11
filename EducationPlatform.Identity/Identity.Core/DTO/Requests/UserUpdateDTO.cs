using Identity.Core.DTO.Validation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Core.DTO.Requests
{
    public class UserUpdateDTO
    {
        public required string UserName { get; set; }

        public required string Email { get; set; }

        [ValidateFile([".png", ".jpg", ".jpeg"], ErrorMessage = "Зображення має непідтримуване розширення")]
        public IFormFile? UserImage { get; set; }
    }
}
