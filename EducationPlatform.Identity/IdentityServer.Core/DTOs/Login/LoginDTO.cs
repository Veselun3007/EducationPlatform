using System.ComponentModel.DataAnnotations;

namespace IdentityServer.Web.DTOs.Login
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "Поле 'Електронна пошта' обов'язкове")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
            ErrorMessage = "Не коректний формат електронної пошти.")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Поле 'Пароль' обов'язкове")]
        public required string UserPassword { get; set; }
    }
}
