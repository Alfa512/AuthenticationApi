using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using AuthenticationApi.Common.Contracts.Data;
using AuthenticationApi.Common.Contracts.Services;
using AuthenticationApi.Model.ApiRequest;
using AuthenticationApi.Model.ApiResponse;
using AuthenticationApi.Model.Data;
using AuthenticationApi.Model.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using AuthenticationApi.Model.Security;

namespace AuthenticationApi.Business.Services
{
    public class UserService : IUserService
    {
        private readonly IDataContext _context;
        private readonly ICryptoService _crypto;
        private readonly IConfigurationService _config;
        private readonly ITemplateService _templateService;
        private readonly IMailService _mailService;
        private readonly IConfigurationService _configurationService;
        private readonly IDatabaseConfigurationService _dbConfigurationService;

        private readonly UserManager<User> _userManager;

        //private readonly IUserRepository _userRepository;

        public UserService(IDataContext context, ICryptoService crypto, IConfigurationService config, ITemplateService templateService, IMailService mailService, IConfigurationService configurationService, 
            IDatabaseConfigurationService dbConfigurationService, UserManager<User> userManager)
        {
            _context = context;
            _crypto = crypto;
            _config = config;
            _templateService = templateService;
            _mailService = mailService;
            _configurationService = configurationService;
            _dbConfigurationService = dbConfigurationService;
            _userManager = userManager;
        }

        public async Task<AuthenticateTokenResponse> AuthenticateWithToken(AuthenticateTokenRequest request)
        {
            var passwordSecret = await _dbConfigurationService.GetPasswordSecret();
            var passwordHash = _crypto.EncryptStringAes(request.Password + AuthOptions.PasswordSalt, passwordSecret);

            var response = new AuthenticateTokenResponse();

            User user = null;
            if (!request.Username.IsNullOrEmpty())
            {
                user = _context.Users.GetAll().SingleOrDefault(u => u.NormalizedUserName == request.Username.ToLower() && u.PasswordHash == passwordHash);
            }
            else if(!request.Email.IsNullOrEmpty())
            {
                user = _context.Users.GetAll().SingleOrDefault(u => u.NormalizedEmail == request.Email.ToLower() && u.PasswordHash == passwordHash);
            }

            
            //var user = _context.Users.GetAll().SingleOrDefault(u => string.Equals(u.UserName, request.Username, StringComparison.CurrentCultureIgnoreCase) && string.Equals(u.PasswordHash, passwordHash, StringComparison.CurrentCulture));

            if (user == null)
            {
                response.Errors.Add("LoginFailed", "Неверное имя пользователя или пароль");
                return response;
            };

            // authentication successful so generate jwt token
            var token = await AuthUser(user, request.ProviderCode);
            response.AccessToken = token.AccessToken;

            return response;
        }

        public async Task<User> AuthenticateByEmail(string email, string password)
        {
            var passwordSecret = await _dbConfigurationService.GetPasswordSecret();
            var passwordHash = _crypto.EncryptStringAes(password, passwordSecret);

            var user = await _context.Users.GetAll().SingleOrDefaultAsync(x => x.NormalizedEmail == email && x.PasswordHash == passwordHash);

            return user;
        }

        public async Task<User> AuthenticateByUsername(string userName, string password)
        {
            var passwordSecret = await _dbConfigurationService.GetPasswordSecret();
            var passwordHash = _crypto.EncryptStringAes(password, passwordSecret);

            var user = _context.Users.GetAll().FirstOrDefault(u => string.Equals(u.UserName, userName, StringComparison.CurrentCultureIgnoreCase) && string.Equals(u.PasswordHash, passwordHash, StringComparison.CurrentCulture));
            if (user == null)
                return null;
            return user;
        }

        public IEnumerable<User> GetUsers()
        {
            return _context.Users.GetAll();
        }

        public User GetUserByNameAndHash(string name, string hash)
        {
            return _context.Users.GetAll().FirstOrDefault(t => t.GetUserName().ToUpper().Equals(name.ToUpper(), StringComparison.CurrentCultureIgnoreCase) && t.PasswordHash.Equals(hash));
        }

        public User GetUserById(Guid id)
        {
            return _context.Users.GetAll().FirstOrDefault(t => t.Id.Equals(id));
        }

        public async Task<User> GetUserByUserName(string username)
        {
            return await _context.Users.GetAll().FirstOrDefaultAsync(t => t.GetUserName().Equals(username, StringComparison.CurrentCultureIgnoreCase));
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await _context.Users.GetAll().FirstOrDefaultAsync(t => t.NormalizedEmail == email);
        }

        public async Task<bool> DoesUserNameExists(string username)
        {
            username = username.ToLower();
            return await _context.Users.GetAll().AnyAsync(t => t.NormalizedUserName == username);
        }

        public async Task<User> CreateUser(string name, string hash, string email)
        {
            var newUser = new User() { FirstName = name, Email = email, PasswordHash = hash/*, TokenValidTo = DateTime.Now*/ };
            newUser.SetDefaultValues();
            var passwordSecret = await _dbConfigurationService.GetPasswordSecret();
            var passwordHash = _crypto.EncryptStringAes(hash, passwordSecret);
            newUser.PasswordHash = passwordHash;

            newUser = _context.Users.Create(newUser);
            _context.Commit();
            SendWelcomeMail(name);
            return newUser;
        }

        public async Task<AuthenticateTokenResponse> CreateUser(SignUpFirstStepRequest request)
        {
            var user = new User();
            user.SetDefaultValues();
            user.UserName = request.UserName;
            user.NormalizedUserName = request.UserName.ToLower();
            user.Email = request.Email;
            user.NormalizedEmail = request.Email.ToLower();

            var response = new AuthenticateTokenResponse(user, string.Empty);

            if (request.Password != request.PasswordConfirm)
            {
                response.Errors.Add("ConfirmPassDoNotMatch", "Пароль и подтверждение пароля не совпадают");
            }

            if (request.UserName.IsNullOrEmpty())
            {
                response.Errors.Add("InvalidUserName", "Неверный формат имени пользователя");
            }
            else if (await UserNameExists(request.UserName))
            {
                response.Errors.Add("UserNameAlreadyExists", "Этот логин уже занят");
            }

            if (request.Email.IsNullOrEmpty())
            {
                response.Errors.Add("InvalidEmail", "Неверный формат имени пользователя");
            }
            else if (await UserNameExists(request.Email))
            {
                response.Errors.Add("EmailAlreadyExists", "Данный Email уже зарегистрирован");
            }

            User newUser = null;

            if (!response.Errors.Any())
            {
                newUser = await CreateUser(user, request.Password);
            }

            if (newUser != null)
            {
                response.AccessToken = (await AuthUser(newUser, request.ProviderCode)).AccessToken;
                response.InitUser(newUser);
            }

            return response;
            

            //var newUser = _context.Users.Create(user);
            //_context.Commit();
            ////SendWelcomeMail(newUser.FirstName);
            //return newUser;
        }


        public async Task<string> GenerateEmailConfirmationTokenAsync(User user)
        {
            return await _userManager.GenerateEmailConfirmationTokenAsync(user);
        }

        public bool IsUserExists(string name)
        {
            return _context.Users.GetAll().FirstOrDefault(t => t.FirstName.Equals(name, StringComparison.OrdinalIgnoreCase)) != null;
        }

        public User GetUserByToken(string token)
        {
            return _context.Users.GetAll().FirstOrDefault(t => t.RestorePasswordToken == token);
        }

        public string CreateToken(string userName)
        {
            var token = _crypto.Hash(Guid.NewGuid().ToString());
            var user =
                _context.Users.GetAll().FirstOrDefault(t => t.FirstName.Equals(userName, StringComparison.OrdinalIgnoreCase));
            if (user == null)
            {
                throw new Exception(string.Format("User with name {0} not found", userName));
            }

            user.RestorePasswordToken = token;
            user.TokenValidTo = DateTime.Now.AddHours(24);
            _context.Users.Update(user);
            _context.Commit();
            return token;
        }

        public async Task<Login> GetLastUserLogin(string username)
        {
            var user = await GetUserByUserName(username);
            if (user == null)
            {
                return null;
            }

            var login = await _context.Logins.GetAll().Where(r => r.UserId == user.Id).OrderByDescending(r => r.ChangeDate)
                .FirstOrDefaultAsync();

            return login;
        }

        public Login UpdateUserLogin(Login login)
        {
            if (login == null)
            {
                return null;
            }

            return _context.Logins.Update(login);
        }

        public void SendMailRestorePassword(string userName)
        {
            var token = CreateToken(userName);
            var restoreUrl = _config.RestorePasswordUrl.Replace("{token}", token);
            var user =
                _context.Users.GetAll().FirstOrDefault(t => t.FirstName.Equals(userName, StringComparison.OrdinalIgnoreCase));
            if (user != null)
            {
                var items = new Dictionary<string, string>
                            {
                                {"{restorepasswordurl}", restoreUrl},
                                {"{userName}", userName},
                                {"{validDate}", string.Format("{0:U}",user.TokenValidTo)}
                            };
                var template = _templateService.Process(_config.RestorePasswordMailTemplate, items);
                _mailService.SendMail(user.Email, _config.RestorePasswordMailSubject, template);
            }
        }

        public void SendWelcomeMail(string userName)
        {
            var user =
                _context.Users.GetAll().FirstOrDefault(t => t.FirstName.Equals(userName, StringComparison.OrdinalIgnoreCase));
            if (user != null)
            {
                var items = new Dictionary<string, string>
                            {
                                {"{userName}", userName},
                                {"{userEmail}", user.Email},
                                {"{year}", DateTime.Now.Year.ToString()},
                            };
                var bccEmail = _config.WelcomeMailBccAddress;
                var template = _templateService.Process(_config.WelcomeMailTemplate, items);
                _mailService.SendMail(user.Email, _config.WelcomeMailSubject, template, MailType.Welcome, bccEmail);
            }
        }

        public void RestorePassword(string token, string hash)
        {
            if (string.IsNullOrEmpty(token)) { throw new Exception("AccessToken not found"); }

            var user = _context.Users.GetAll().FirstOrDefault(t => t.RestorePasswordToken == token);
            if (user != null)
            {
                if (user.TokenValidTo > DateTime.Now)
                {
                    try
                    {
                        user.PasswordHash = hash;
                        user.RestorePasswordToken = string.Empty;
                        _context.Users.Update(user);
                        _context.Commit();
                        return;
                    }
                    catch (Exception)
                    {
                        throw new Exception("Internal error");
                    }
                }
            }
            throw new Exception("AccessToken not found");
        }

        public bool CheckToken(string token)
        {
            if (string.IsNullOrEmpty(token)) return false;
            return _context.Users.GetAll().FirstOrDefault(t => t.RestorePasswordToken == token && t.TokenValidTo > DateTime.Now) != null;
        }

        public void ChangePassword(long userId, string oldPasswordHash, string newPasswordHash)
        {
            var user = _context.Users.GetAll().FirstOrDefault(t => t.Id == userId && t.PasswordHash == oldPasswordHash);
            if (user == null)
            {
                throw new Exception("Old password is incorrect");
            }
            user.PasswordHash = newPasswordHash;
            _context.Users.Update(user);
            _context.Commit();
        }

        public async Task<RefreshTokenRequest> RefreshToken(RefreshTokenRequest refreshTokenRequest)
        {
            var result = new RefreshTokenRequest
            {
                AccessToken = null,
                RefreshToken = null,
                RefreshTokenSuccess = false
            };

            if (refreshTokenRequest is null)
            {
                return result;
            }

            string accessToken = refreshTokenRequest.AccessToken;
            string refreshToken = refreshTokenRequest.RefreshToken;

            var principal = GetPrincipalFromExpiredToken(accessToken);
            if (principal == null)
            {
                return result;
            }

            string username = principal.Identity?.Name;

            if (username.IsNullOrEmpty())
            {
                return result;
            }

            var login = await GetLastUserLogin(username);

            if (login == null || login.AccessToken != accessToken || login.RefreshToken != refreshToken || login.RefreshTokenExpiryTime <= DateTime.Now)
            {
                return result;
            }

            var newAccessToken = await GenerateJwtTokenByUserNameAsync(username);
            var newRefreshToken = Guid.NewGuid().ToString();

            login.AccessToken = newAccessToken.AccessToken;
            login.RefreshToken = newRefreshToken;
            login.AccessTokenExpiryTime = newAccessToken.ExpirationDate;
            login.RefreshTokenExpiryTime =
                newAccessToken.ExpirationDate.AddMinutes(AuthOptions.RefreshTokenLifeTimeMinutes);
            UpdateUserLogin(login);

            result.AccessToken = newAccessToken.AccessToken;
            result.RefreshToken = newRefreshToken;
            result.RefreshTokenSuccess = true;

            return result;
        }

        #region Private Methods


        private async Task<User> CreateUser(User user, string password)
        {

            var passwordSecret = await _dbConfigurationService.GetPasswordSecret();
            user.PasswordHash = _crypto.EncryptStringAes(password + AuthOptions.PasswordSalt, passwordSecret);

            var newUser = _context.Users.Create(user);

            if (newUser == null)
            {
                return null;
            }

            var userRole = new UserRole
            {
                UserId = newUser.Id,
                RoleId = 2 //TODO: Add Enum for Roles
            };

            _context.UserRoles.Create(userRole);

            return newUser;

        }

        private async Task<AccessTokenModel> AuthUser(User user, string providerCode)
        {
            var token = GenerateJwtToken(user);

            var provider = await _context.LoginProviders.GetAll().FirstOrDefaultAsync(r => r.ProviderCode == providerCode);

            if (provider == null)
            {
                return null;
            }

            var login = new Login
            {
                AccessToken = token.AccessToken,
                RefreshToken = GenerateRefreshToken(),
                LoginProviderId = provider.Id,
                ProviderCode = provider.ProviderCode,
                AccessTokenExpiryTime = token.ExpirationDate,
                RefreshTokenExpiryTime = token.ExpirationDate.AddMinutes(AuthOptions.RefreshTokenLifeTimeMinutes),
                UserId = user.Id
            };

            _context.Logins.Create(login);

            return token;
        }

        private async Task<bool> UserWithEmailExists(string email)
        {
            return await _context.Users.GetAll().AnyAsync(r => r.NormalizedEmail == email.Trim().ToLower());
        }
        private async Task<bool> UserNameExists(string username)
        {
            return await _context.Users.GetAll().AnyAsync(r => r.NormalizedUserName == username.Trim().ToLower());
        }

        private async Task<AccessTokenModel> GenerateJwtTokenByUserNameAsync(string username)
        {
            if (username.IsNullOrEmpty())
            {
                return null;
            }

            var user = await GetUserByUserName(username);

            if (user == null)
            {
                return null;
            }

            return GenerateJwtToken(user);
        }

        private AccessTokenModel GenerateJwtToken(User user)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(AuthOptions.SecretCode);
            var now = DateTime.UtcNow;
            var expirationDate = now.Add(TimeSpan.FromMinutes(AuthOptions.AccessTokenLifeTimeMinutes));
            var claims = new List<Claim>();
            claims.Add(new Claim("UserIdentifier", user.UserIdentifier.ToString()));
            claims.Add(new Claim("UserName", user.UserName));
            claims.Add(new Claim("expires", TimeSpan.FromMinutes(AuthOptions.AccessTokenLifeTimeMinutes).ToString()));
            //claims.Add(new Claim("Roles", string.Join(",", user.UserRoles.Select(r => r.RoleId)) ));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = AuthOptions.ISSUER,
                Audience = AuthOptions.AUDIENCE,
                Subject = new ClaimsIdentity(claims),
                Expires = expirationDate,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                NotBefore = now
                //TODO: Add Claims
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return new AccessTokenModel
            {
                AccessToken = tokenHandler.WriteToken(token),
                ExpirationDate = expirationDate,
                TokenType = "Bearer"
            };
        }

        private static string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var payload = jwt.Split('.')[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
            return keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()));
        }

        private static byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }

        private ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AuthOptions.SecretCode)),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;

        }
        #endregion
    }
}