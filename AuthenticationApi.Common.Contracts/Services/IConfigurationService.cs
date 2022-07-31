namespace AuthenticationApi.Common.Contracts.Services
{
    public interface IConfigurationService
    {
        string BaseUrl { get; }
        string MainConnection { get; }
        string RestorePasswordMailTemplate { get; }
        string RestorePasswordMailSubject { get; }
        string RestorePasswordUrl { get; }
        string WelcomeMailTemplate { get; }
        string WelcomeMailSubject { get; }
        string WelcomeMailBccAddress { get; }
        string MailGunUrl { get; }
        string MailGunApi { get; }
        string MailGunDomain { get; }
        string MailMessageFrom { get; }
        string AuthScopes { get; }
    }
}