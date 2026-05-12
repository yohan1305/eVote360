using Evote365.Core.Application.Interfaces;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Evote365.Infrastructure.Shared
{
    public class SmtpEmailService : IEmailService
    {
        private readonly EmailOptions _options;

        public SmtpEmailService(IOptions<EmailOptions> options)
        {
            _options = options.Value;
        }

        public async Task<bool> SendAsync(string toEmail, string subject, string htmlBody)
        {
            if (string.IsNullOrWhiteSpace(toEmail))
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(_options.Host) || string.IsNullOrWhiteSpace(_options.FromEmail))
            {
                return await SaveFallbackAsync(toEmail, subject, htmlBody);
            }

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_options.FromName, _options.FromEmail));
            message.To.Add(MailboxAddress.Parse(toEmail));
            message.Subject = subject;
            message.Body = new BodyBuilder { HtmlBody = htmlBody }.ToMessageBody();

            using var client = new SmtpClient();
            await client.ConnectAsync(_options.Host, _options.Port, _options.UseSsl);

            if (!string.IsNullOrWhiteSpace(_options.Username))
            {
                await client.AuthenticateAsync(_options.Username, _options.Password);
            }

            await client.SendAsync(message);
            await client.DisconnectAsync(true);
            return true;
        }

        private static async Task<bool> SaveFallbackAsync(string toEmail, string subject, string htmlBody)
        {
            var folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "emails");
            Directory.CreateDirectory(folder);

            var safeFileName = $"{DateTime.Now:yyyyMMddHHmmss}_{Guid.NewGuid():N}.html";
            var content = $"""
                <html>
                <body>
                    <h2>{subject}</h2>
                    <p><strong>Para:</strong> {toEmail}</p>
                    {htmlBody}
                </body>
                </html>
                """;

            await File.WriteAllTextAsync(Path.Combine(folder, safeFileName), content);
            return true;
        }
    }
}
