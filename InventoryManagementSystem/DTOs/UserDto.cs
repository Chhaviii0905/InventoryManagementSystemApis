namespace InventoryManagementSystem.DTOs
{
    public class UserDto
    {
        public int UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public int? RoleId { get; set; }
    }

    public class CreateUserDto
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public int? RoleId { get; set; }
    }

    public class UpdateUserDto
    {
        public int UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string? Password { get; set; }
        public int? RoleId { get; set; }
    }
}
