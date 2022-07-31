using AuthenticationApi.Model.Shared;

namespace AuthenticationApi.Model.ApiResponse;

public class SignUpFirstStepResponse
{
    public string Email { get; set; }

    public string UserName { get; set; }
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public ValidationErrorMessages ValidationErrorMessages { get; set; }
}