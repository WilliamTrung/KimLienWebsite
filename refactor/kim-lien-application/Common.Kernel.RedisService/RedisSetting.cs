namespace Common.Kernel.RedisService
{
    public class RedisSetting
    {
        public string ConnectionString { get; set; } = null!;
        public string EndPoint { get; set; } = null!;
        public string Password { get; set; } = null!;
        public bool Ssl { get; set; }
    }
}
