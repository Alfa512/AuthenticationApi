using AuthenticationApi.Model.Data;

namespace AuthenticationApi.Model.ApiResponse;

public class CheckEmailExistsResponse
{
    public string Email { get; set; }
    public bool Exists { get; set; }
}