using Azure.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using Microsoft.Graph;

namespace BurzaFirem2.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailOptions _options;
        private readonly ILogger<EmailSender> _logger;

        public EmailSender(IOptions<EmailOptions> options, ILogger<EmailSender> logger)
        {
            _options = options.Value;
            _logger = logger;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var message = new Message
            {
                Subject = subject,
                Body = new ItemBody
                {
                    ContentType = BodyType.Html,
                    Content = htmlMessage
                },
                ToRecipients = new List<Recipient>()
                {
                    new Recipient {EmailAddress = new EmailAddress {Address = email } }
                }
            };
            string[] scopes = new string[] { "https://graph.microsoft.com/.default" };

            var options = new TokenCredentialOptions
            {
                AuthorityHost = AzureAuthorityHosts.AzurePublicCloud
            };

            var authProvider = new ClientSecretCredential(
                _options.TenantId,
                _options.ClientId,
                _options.ClientSecret,
                options);
            //var authProvider = new ClientCredentialProvider(confidentialClientApplication);
            var graphClient = new GraphServiceClient(authProvider);
            await graphClient.Users[_options.UserId]
                .SendMail(message, false)
                .Request()
                .PostAsync();
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage, string plainMessage)
        {
            var message = new Message
            {
                Subject = subject,
                Body = new ItemBody
                {
                    ContentType = BodyType.Html,
                    Content = htmlMessage
                },
                ToRecipients = new List<Recipient>()
                {
                    new Recipient {EmailAddress = new EmailAddress {Address = email } }
                }
            };
            string[] scopes = new string[] { "https://graph.microsoft.com/.default" };

            var options = new TokenCredentialOptions
            {
                AuthorityHost = AzureAuthorityHosts.AzurePublicCloud
            };

            var authProvider = new ClientSecretCredential(
                _options.TenantId,
                _options.ClientId,
                _options.ClientSecret,
                options);
            //var authProvider = new ClientCredentialProvider(confidentialClientApplication);
            var graphClient = new GraphServiceClient(authProvider);
            await graphClient.Users[_options.UserId]
                .SendMail(message, false)
                .Request()
                .PostAsync();
        }
    }

    public class EmailOptions
    {
        public string TenantId { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string UserId { get; set; }
    }
}
