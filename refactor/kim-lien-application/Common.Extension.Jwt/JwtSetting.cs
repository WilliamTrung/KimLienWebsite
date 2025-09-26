namespace Common.Extension.Jwt
{
    public sealed class JwtSettings
    {
        public required string Issuer { get; init; }
        public required string Audience { get; init; }
        public required string KeyB64 { get; init; }     // base64 symmetric key
        public int AccessMinutes { get; init; } = 10;
        public int RefreshDays { get; init; } = 14;
    }
}
