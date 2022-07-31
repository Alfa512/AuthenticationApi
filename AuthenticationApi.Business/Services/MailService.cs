using System;
using System.ComponentModel;
using System.Net.Mail;
using System.Net.Mime;
using RestSharp;
using RestSharp.Authenticators;
using AuthenticationApi.Common.Contracts.Services;
using AuthenticationApi.Model.Enums;

namespace AuthenticationApi.Business.Services
{
    public class MailService : IMailService
    {
        private readonly IConfigurationService _configService;

        public MailService(IConfigurationService configurationService)
        {
            _configService = configurationService;
        }

        public void SendMail(string email, string subject, string body, MailType? type = null, string emailBcc = null)
        {
            var message = new MailMessage();
            message.To.Add(new MailAddress(email));
            if (!string.IsNullOrEmpty(emailBcc))
                message.Bcc.Add(new MailAddress(emailBcc));
            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = true;

            var client = new SmtpClient();
            client.SendCompleted += SendCompletedCallback;

            switch (type)
            {
                case MailType.Welcome:
                    var logo = new LinkedResource(_configService.BaseUrl + "~/img/email/logo.png");
                    logo.ContentId = "logo";

                    var rocket = new LinkedResource(_configService.BaseUrl + "~/img/email/rocket.png");
                    rocket.ContentId = "rocket";


                    AlternateView avHtml = AlternateView.CreateAlternateViewFromString
                        (message.Body, null, MediaTypeNames.Text.Html);

                    avHtml.LinkedResources.Add(logo);
                    avHtml.LinkedResources.Add(rocket);
                    message.AlternateViews.Add(avHtml);


                    Attachment att = new Attachment(_configService.BaseUrl + "~/img/email/logo.png");
                    att.ContentDisposition.Inline = true;
                    break;
            }

            client.Send(message);
        }

        private static void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
            // Get the unique identifier for this asynchronous operation.
            //var token = (string)e.UserState;

            if (e.Cancelled)
            {
                // Console.WriteLine("[{0}] Send canceled.", token);
            }
            if (e.Error != null)
            {
                // Console.WriteLine("[{0}] {1}", token, e.Error.ToString());
            }
            else
            {
                //Console.WriteLine("Message sent.");
            }
        }

        public RestResponse SendTextMessage(string to, string subject = "", string text = "")
        {
            RestClient client = new RestClient();
            
            client.Authenticator = new HttpBasicAuthenticator("api", _configService.MailGunApi);
            RestRequest request = new RestRequest(new Uri(_configService.MailGunUrl), Method.Post);
            request.AddParameter("domain", _configService.MailGunDomain, ParameterType.UrlSegment);
            request.Resource = "{domain}/messages";
            request.AddParameter("from", _configService.MailMessageFrom);
            request.AddParameter("to", to);
            request.AddParameter("subject", subject);
            request.AddParameter("text", text);
            //request.AddFile("attachment", Path.Combine("files", "test.jpg"));
            //request.AddFile("attachment", Path.Combine("files", "test.txt"));
            client.BuildUri(request);
            return client.Execute(request);
        }

        public RestResponse SendHtmlMessage(string to, string subject = "", string html = "")
        {
            RestClient client = new RestClient();
            client.Authenticator = new HttpBasicAuthenticator("api", _configService.MailGunApi);
            RestRequest request = new RestRequest(new Uri(_configService.MailGunUrl), Method.Post);
            request.AddParameter("domain", _configService.MailGunDomain, ParameterType.UrlSegment);
            request.Resource = "{domain}/messages";
            request.AddParameter("from", _configService.MailMessageFrom);
            request.AddParameter("to", to);
            request.AddParameter("subject", subject);
            request.AddParameter("html", html);
            //request.AddFile("attachment", Path.Combine("files", "test.jpg"));
            //request.AddFile("attachment", Path.Combine("files", "test.txt"));
            client.BuildUri(request);
            return client.Execute(request);
        }

    }
}