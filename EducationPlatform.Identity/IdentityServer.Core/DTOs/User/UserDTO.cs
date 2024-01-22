using IdentityServer.Core.DTOs.User;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace IdentityServer.Web.DTOs.User
{
    public class UserDTO
    {
        [Required(ErrorMessage = "Поле 'Ім'я користувача' обов'язкове")]
        [RegularExpression(@"^[a-zA-Zа-яА-ЯґҐєЄіІїЇ\s]+$", 
            ErrorMessage = "Допустимі тільки англійські й українські літери та пробіли.")]
        public required string UserName { get; set; }

        [Required(ErrorMessage = "Поле 'Електронна пошта' обов'язкове")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
            ErrorMessage = "Не коректний формат електронної пошти.")]
        public required string UserEmail { get; set; }

        [Required(ErrorMessage = "Поле 'Пароль' обов'язкове")]
        public required string UserPassword { get; set; }

        [ValidateFile([".png", ".jpg", ".jpeg"], ErrorMessage = "Зображення має непідтримуване розширення")]
        public IFormFile? UserImage { get; set; }

    }
}



