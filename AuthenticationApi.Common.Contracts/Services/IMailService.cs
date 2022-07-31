using AuthenticationApi.Model.Enums;

namespace AuthenticationApi.Common.Contracts.Services
{
    public interface IMailService
    {
        void SendMail(string email, string subject, string body, MailType? type = null, string emailBcc = null);
        //IRestResponse SendTextMessage(string to, string subject, string text);
        //IRestResponse SendHtmlMessage(string to, string subject, string html);
    }
}