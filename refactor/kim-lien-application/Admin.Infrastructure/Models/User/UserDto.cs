namespace Admin.Application.Models.User
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = null!;
        public string? DisplayName { get; set; }
        public string Region { get; set; } = null!;
    }
}
