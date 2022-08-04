using System;

namespace AuthenticationApi.Model.Security;

public class AccessTokenModel
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public string TokenType { get; set; }
    public DateTime ExpirationDate { get; set; }
}