namespace Evote365.Core.Application.Interfaces
{
    public interface IEmailService
    {
        Task<bool> SendAsync(string toEmail, string subject, string htmlBody);
    }
}
