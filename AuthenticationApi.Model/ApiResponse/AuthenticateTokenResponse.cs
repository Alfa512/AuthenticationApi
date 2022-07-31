using System.Collections.Generic;
using AuthenticationApi.Model.Data;

namespace AuthenticationApi.Model.ApiResponse;

public class AuthenticateTokenResponse
{
    public long Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public Dictionary<string, string> Errors { get; set; }

    public AuthenticateTokenResponse()
    {
        AccessToken = string.Empty;
        RefreshToken = string.Empty;
        Errors = new Dictionary<string, string>();
    }

    public AuthenticateTokenResponse(User user, string accessToken)
    {
        Id = user.Id;
        FirstName = user.FirstName;
        LastName = user.LastName;
        Email = user.Email;
        Username = user.GetUserName();
        AccessToken = accessToken;
        Errors = new Dictionary<string, string>();
    }

    public void InitUser(User user)
    {
        Id = user.Id;
        FirstName = user.FirstName;
        LastName = user.LastName;
        Username = user.GetUserName();
        Email = user.NormalizedEmail;
    }
}