using System.Security.Claims;
using System.Threading.Tasks;
using AuthenticationApi.Model.ApiRequest;
using AuthenticationApi.Model.ApiResponse;

namespace AuthenticationApi.Common.Contracts.Services;

public interface IAuthenticationService
{
    public Task<SignUpFirstStepResponse> SignUp(SignUpFirstStepRequest request);
    Task<AuthenticateTokenResponse> SignIn(SignIn request);
    bool IsSignedIn(ClaimsPrincipal identity);
    Task<bool> DoesUserNameExists(string username);
}