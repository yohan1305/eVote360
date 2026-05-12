namespace Evote365.Infrastructure.Shared
{
    public class EmailOptions
    {
        public string Host { get; set; } = string.Empty;
        public int Port { get; set; } = 587;
        public string FromName { get; set; } = "eVote365";
        public string FromEmail { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool UseSsl { get; set; } = true;
    }
}
