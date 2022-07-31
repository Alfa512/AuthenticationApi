using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace AuthenticationApi.Model.Security;

public class AuthOptions
{
    public const string ISSUER = "AuthenticationApi";
    public const string AUDIENCE = "api";
    public const string AUTHORITY= "https://alfalife.site/";
    const string KEY = "1B6210940E539B234A1ACED9C2A1404BC512F88608427619F0A99FB19F4D3AA2";
    public const string IssuerSigningKey = "5fc9f618-0100-a512-8729-79986f1eceb8";
    public const string SecretCode = "28257430-a467-4070-bad9-83a8cbb92c5a";
    public const int AccessTokenLifeTimeMinutes = 20;
    public const int RefreshTokenLifeTimeMinutes = 10;
    public const string ClaimAuthTypeKey = "access_token";
    public const string ClaimTokenTypeKey = "token_type";
    public const string ClaimExpiresInKey = "expires_in";
    public const string PasswordSalt = "de62b21a-5ae4-418c-917d-ee37f81dd96c";
    public static SymmetricSecurityKey GetSymmetricSecurityKey()
    {
        return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
    }
}