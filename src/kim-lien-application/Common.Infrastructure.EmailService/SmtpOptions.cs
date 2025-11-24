namespace Common.Infrastructure.EmailService
{
    public sealed class SmtpOptions
    {
        public string Host { get; set; } = default!;
        public int Port { get; set; } = 587;
        public bool EnableSsl { get; set; } = true;
        public string User { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string FromEmail { get; set; } = default!;
        public string FromName { get; set; } = "MyApp";
    }
}
