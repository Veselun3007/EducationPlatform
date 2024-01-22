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
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-zA-Z])(?=.*\d)(?=.*[!@#$%^&*()_+])[A-Za-z\d!@#$%^&*()_+]{8,}$",
            ErrorMessage = "Пароль повинен містити принаймні одну літеру, одну цифру, один спеціальний символ, " +
            "та бути довжиною не менше 8 символів.")]
        public required string UserPassword { get; set; }

        public string? RefreshToken { get; set; }

        public DateTime? ValidUntil { get; set; }
    }
}
