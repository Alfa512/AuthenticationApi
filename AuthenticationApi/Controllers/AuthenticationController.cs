using AuthenticationApi.Common.Contracts.Services;
using AuthenticationApi.Model.ApiRequest;
using AuthenticationApi.Model.ApiResponse;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationApi.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthenticationController : ControllerBase
    {

        private readonly ILogger<AuthenticationController> _logger;
        private IUserService _userService;

        public AuthenticationController(ILogger<AuthenticationController> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        [HttpPost(Name = "sign-in")]
        public async Task<ActionResult<AuthenticateTokenResponse>> SignIn(AuthenticateTokenRequest request)
        {
            var response = await _userService.AuthenticateWithToken(request);

            if (response.Errors.Any())
            {
                return BadRequest(response);
            }

            return Created(request.ProviderUrl, response);
        }

        [HttpPost(Name = "sign-up")]
        public async Task<ActionResult<AuthenticateTokenResponse>> SignUp(SignUpFirstStepRequest request)
        {
            var response = await _userService.CreateUser(request);

            if (response.Errors.Any())
            {
                return BadRequest(response);
            }
            
            return Created(request.ProviderUrl, response);
        }

        [HttpPost(Name = "refresh-token")]
        public async Task<RefreshTokenRequest> RefreshToken(RefreshTokenRequest request)
        {
            return await _userService.RefreshToken(request);
        }
    }
}