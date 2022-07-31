using System.ComponentModel.DataAnnotations;

namespace AuthenticationApi.Model.ApiRequest;

public class SignIn
{
    [Required(ErrorMessage = "Email не указан")]
    [EmailAddress(ErrorMessage = "Неверный формат Email")]
    public string Email { get; set; }

    public string UserName { get; set; }

    [Required(ErrorMessage = "Введите пароль")]
    public string Password { get; set; }
}