using System;
using Microsoft.Extensions.Configuration;
using AuthenticationApi.Common.Contracts.Services;

namespace AuthenticationApi.Business.Services
{
    public class ConfigurationService : IConfigurationService
    {
        private IConfiguration _configuration;
        public ConfigurationService(IConfiguration configuration)
        {
            _configuration = configuration;

            MainConnection = _configuration.GetSection("ConnectionStrings")["MainConnection"];
            AuthScopes = _configuration.GetSection("Auth")["Scopes"];
        }
        private readonly string _basePath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;

        public string BaseUrl { get; }
        public string MainConnection { get; }
        public string RestorePasswordMailTemplate { get; }

        public string RestorePasswordMailSubject { get; }
        public string RestorePasswordUrl { get; }
        public string WelcomeMailTemplate { get; }

        public string WelcomeMailSubject { get; }
        public string WelcomeMailBccAddress { get; }

        public string MailGunUrl { get; }
        public string MailGunApi { get; }
        public string MailGunDomain { get; }
        public string MailMessageFrom { get; }
        public string AuthScopes { get; }
    }
}