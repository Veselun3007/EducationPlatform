using Identity.Core.DTO.Validation;
using Microsoft.AspNetCore.Http;

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
