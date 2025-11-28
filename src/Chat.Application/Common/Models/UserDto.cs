using Common.Kernel.Models.Implementations;

namespace Chat.Application.Common.Models
{
    public class UserDto
    {
        public string DisplayName { get; set; } = null!;
        public List<AssetDto>? Assets { get; set; }
        public DateTime? LastActiveAt { get; set; }
        public bool IsOnline { get; set; }
    }
}
