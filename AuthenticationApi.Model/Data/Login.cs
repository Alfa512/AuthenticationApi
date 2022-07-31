using System;
using System.Collections.Generic;

namespace AuthenticationApi.Model.Data;

public class Login
{
    public long Id { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime ChangeDate { get; set; }
    public string ChangedBy { get; set; }
    public long UserId { get; set; }
    public int LoginProviderId { get; set; }
    public string ProviderCode { get; set; }
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public DateTime AccessTokenExpiryTime { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }
}