using System.Security.Claims;
using System.Threading.Tasks;
using AuthenticationApi.Model.ApiRequest;
using AuthenticationApi.Model.ApiResponse;

namespace AuthenticationApi.Common.Contracts.Application;

public interface IAuthService
{
    Task<AuthenticateTokenResponse> SignInSubmit(SignIn request);
    Task<SignUpFirstStepRequest> SignUpSubmit(SignUpFirstStepRequest request);
    Task<bool> IfUserNameExists(string username);
    bool IsSignedIn(ClaimsPrincipal user);
}