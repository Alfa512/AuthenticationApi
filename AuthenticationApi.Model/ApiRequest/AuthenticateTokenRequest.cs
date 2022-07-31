using System.ComponentModel.DataAnnotations;

namespace AuthenticationApi.Model.ApiRequest;

public class AuthenticateTokenRequest : ApiRequest
{
    public string Username { get; set; }
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }
}