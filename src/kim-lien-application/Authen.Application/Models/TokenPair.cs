namespace Authen.Application.Models
{
    public class TokenPair
    {
        public string AccessToken { get; init; } = default!;
        public DateTime ExpiresAtUtc { get; init; }
        public string RefreshToken { get; init; } = default!;
    }
}
