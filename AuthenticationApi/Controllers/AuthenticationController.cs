using AuthenticationApi.Common.Contracts.Services;
using AuthenticationApi.Model.ApiRequest;
using AuthenticationApi.Model.ApiResponse;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationApi.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class AuthenticationController : ControllerBase
    {

        private readonly ILogger<AuthenticationController> _logger;
        private IUserService _userService;

        public AuthenticationController(ILogger<AuthenticationController> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        [HttpPost]
        [Route("/sign-in")]
        public async Task<ActionResult<AuthenticateTokenResponse>> SignIn(AuthenticateTokenRequest request)
        {
            var response = await _userService.AuthenticateWithToken(request);

            if (response.Errors.Any())
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        //[HttpPost(Name = "/sign-up")]
        [HttpPost]
        [Route("/sign-up")]
        public async Task<ActionResult<AuthenticateTokenResponse>> SignUp(SignUpFirstStepRequest request)
        {
            var response = await _userService.CreateUser(request);

            if (response.Errors.Any())
            {
                return BadRequest(response);
            }
            
            return Created(request.ProviderUrl, response);
        }
        
        [HttpGet]
        [Route("/check-username-exists")]
        public async Task<ActionResult<CheckUsernameExistsResponse>> CheckUsernameExists(string username)
        {
            var response = new CheckUsernameExistsResponse
            {
                Username = username,
                Exists = false
            };
            var user = await _userService.GetUserByUserName(username);

            if (user != null)
            {
                response.Username = user.UserName;
                response.Exists = true;
                return Ok(response);
            }
            
            return Ok(response);
        }

        [HttpGet]
        [Route("/check-email-exists")]
        public async Task<ActionResult<CheckEmailExistsResponse>> CheckEmailExists(string email)
        {
            var response = new CheckEmailExistsResponse
            {
                Email = email,
                Exists = false
            };
            var user = await _userService.GetUserByEmail(email);

            if (user != null)
            {
                response.Email = user.Email;
                response.Exists = true;
                return Ok(response);
            }
            
            return Ok(response);
        }

        [HttpPost(Name = "refresh-token")]
        public async Task<RefreshTokenRequest> RefreshToken(RefreshTokenRequest request)
        {
            return await _userService.RefreshToken(request);
        }
    }
}