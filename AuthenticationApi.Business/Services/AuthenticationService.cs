using System;
using System.Security.Claims;
using System.Threading.Tasks;
using AuthenticationApi.Common.Contracts.Services;
using AuthenticationApi.Model.Data;
using AuthenticationApi.Model.ApiRequest;
using AuthenticationApi.Model.ApiResponse;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace AuthenticationApi.Business.Services;

public class AuthenticationService : IAuthenticationService
{
    private IUserService _userService;
    private readonly UserManager<User> _userManager;
    //private readonly SignInManager<User> _signInManager;

    public AuthenticationService(IUserService userService, UserManager<User> userManager)
    {
        _userService = userService;
        _userManager = userManager;
    }

    
    public async Task<SignUpFirstStepResponse> SignUp(SignUpFirstStepRequest request)
    {
        var response = new SignUpFirstStepResponse();
        try
        {
            var result = await _userService.CreateUser(request);

            if (!result.AccessToken.IsNullOrEmpty())
            {
                request.Password = String.Empty;
                request.PasswordConfirm = String.Empty;

                //_logger.LogInformation("User created a new account with password.");
                //ToDo: No IUserTwoFactorTokenProvider<TUser> named 'Default' is registered.
                //var code = await _userService.GenerateEmailConfirmationTokenAsync(user);
                //code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                //var callbackUrl = $"/Account/ConfirmEmail/{user.UserIdentifier}/{code}";

                //Url.Page(
                //    "/Account/ConfirmEmail",
                //    pageHandler: null,
                //    values: new { area = "Identity", userId = user.Id, code = code },
                //    protocol: Request.Scheme);

                //ToDo: Implement Email RegisterConfirmation
                //await _emailSender.SendEmailAsync(Input.Email, "Confirm your email", $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                if (_userManager.Options.SignIn.RequireConfirmedAccount)
                {
                    //return RedirectToPage("RegisterConfirmation", new { email = Input.Email });
                }
                else
                {
                    response.AccessToken = result.AccessToken;
                    //return LocalRedirect(returnUrl);
                }
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    response.ValidationErrorMessages.AddErrorMessage(error.Key, error.Value);
                }
            }

            return response;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return response;
        }

        return response;
    }

    public async Task<AuthenticateTokenResponse> SignIn(SignIn request)
    {
        AuthenticateTokenResponse result = new AuthenticateTokenResponse();
        var authRequest = new AuthenticateTokenRequest()
        {
            Username = request.UserName,
            Password = request.Password,
            Email = request.Email
        };
        try
        {
            if (!string.IsNullOrEmpty(request.UserName) || !string.IsNullOrEmpty(request.Email))
            {
                result = await _userService.AuthenticateWithToken(authRequest);
                return result;
            }
            else
            {
                var user = await _userService.GetUserByEmail(request.Email);
                //result = await _signInManager.PasswordSignInAsync(user, request.Password, false, false);
            }

            //if (result.Succeeded)
            {
                //return true;
            }

            return result;

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        return result;
    }
    public bool IsSignedIn(ClaimsPrincipal identity)
    {
        try
        {
            return false;
            //return _signInManager.IsSignedIn(identity);

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }

        return false;
    }

    public async Task<bool> DoesUserNameExists(string username)
    {
        try
        {
            return await _userService.DoesUserNameExists(username);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return await Task.FromResult(true);
        }
    }
    


    //public async Task<string> GenerateToken(string username, string password)
    //{
    //    var identity = await GetIdentity(username, password);

    //    return GetJwt(identity);
    //}

    //public string UpdateToken(string username)
    //{
    //    var identity = GetClaimsIdentity(username);

    //    return GetJwt(identity);
    //}

    //public string GetJwt(ClaimsIdentity identity)
    //{
    //    var now = DateTime.UtcNow;

    //    var jwt = new JwtSecurityToken(
    //        issuer: AuthOptions.ISSUER,
    //        audience: AuthOptions.AUDIENCE,
    //        notBefore: now,
    //        claims: identity.Claims,
    //        expires: now.Add(TimeSpan.FromMinutes(AuthOptions.AccessTokenLifeTimeMinutes)),
    //        signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
    //    var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

    //    return encodedJwt;
    //}

    //private async Task<ClaimsIdentity> GetIdentity(string username, string password)
    //{
    //    var user = await _userService.GetForAuth(username, password);
    //    if (user == null)
    //        return null; 
    //    return GetClaimsIdentity(user.UserName);

    //}

    //private ClaimsIdentity GetClaimsIdentity(string username)
    //{
    //    if (string.IsNullOrEmpty(username))
    //        return null;
    //    var claims = new List<Claim>
    //        {
    //            new Claim(ClaimsIdentity.DefaultNameClaimType, username),
    //        };
    //    var claimsIdentity =
    //        new ClaimsIdentity(claims, "AccessToken", ClaimsIdentity.DefaultNameClaimType,
    //            ClaimsIdentity.DefaultRoleClaimType);
    //    return claimsIdentity;
    //}
}