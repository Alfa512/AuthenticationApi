using AuthenticationApi.Model.Data;

namespace AuthenticationApi.Model.ApiResponse;

public class CheckUsernameExistsResponse
{
    public string Username { get; set; }
    public bool Exists { get; set; }
}