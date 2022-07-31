namespace AuthenticationApi.Model.ApiRequest;

public class SignUpFirstStepRequest : ApiRequest
{
    public string Email { get; set; }

    public string UserName { get; set; }

    public string Password { get; set; }

    public string PasswordConfirm { get; set; }

}