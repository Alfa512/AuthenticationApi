using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AuthenticationApi.Model.ApiRequest;
using AuthenticationApi.Model.ApiResponse;
using AuthenticationApi.Model.Data;

namespace AuthenticationApi.Common.Contracts.Services
{
    public interface IUserService
    {
        Task<AuthenticateTokenResponse> AuthenticateWithToken(AuthenticateTokenRequest request);
        Task<User> AuthenticateByEmail(string email, string password);
        IEnumerable<User> GetUsers();
        Task<User> AuthenticateByUsername(string userName, string password);


        User GetUserById(Guid id);

        Task<User> GetUserByUserName(string login);
        Task<User> GetUserByEmail(string email);
        User GetUserByNameAndHash(string name, string hash);
        Task<bool> DoesUserNameExists(string username);
        Task<User> CreateUser(string name, string hash, string email);
        Task<AuthenticateTokenResponse> CreateUser(SignUpFirstStepRequest request);
        Task<string> GenerateEmailConfirmationTokenAsync(User user);
        Task<RefreshTokenRequest> RefreshToken(RefreshTokenRequest refreshTokenRequest);

        User GetUserByToken(string token);

        string CreateToken(string userName);
        Task<Login> GetLastUserLogin(string username);
        void SendMailRestorePassword(string userName);
        void SendWelcomeMail(string userName);
        void RestorePassword(string token, string password);
        bool CheckToken(string token);
        void ChangePassword(long userId, string oldPasswordHash, string newPassword);
    }
}