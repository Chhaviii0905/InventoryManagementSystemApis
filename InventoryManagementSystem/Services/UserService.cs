using InventoryManagementSystem.DTOs;
using InventoryManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace InventoryManagementSystem.Services
{
    public class UserService
    {
        private readonly InventoryDbContext _context;

        public UserService(InventoryDbContext context)
        {
            _context = context;
        }

        // Hash password helper (simple SHA256 here for demo)
        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        public async Task<User?> ValidateUserAsync(string username, string password)
        {
            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Username == username);

            if (user == null)
                return null;

            var hashedInput = HashPassword(password);

            return user.PasswordHash == hashedInput ? user : null;
        }

        public async Task<List<UserDto>> GetAllAsync()
        {
            return await _context.Users
                .Select(u => new UserDto
                {
                    UserId = u.UserId,
                    Username = u.Username,
                    RoleId = u.RoleId
                }).ToListAsync();
        }

        public async Task<UserDto?> GetByIdAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return null;

            return new UserDto
            {
                UserId = user.UserId,
                Username = user.Username,
                RoleId = user.RoleId
            };
        }

        public async Task<UserDto> CreateAsync(CreateUserDto dto)
        {
            var user = new User
            {
                Username = dto.Username,
                PasswordHash = HashPassword(dto.Password),
                RoleId = dto.RoleId
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new UserDto
            {
                UserId = user.UserId,
                Username = user.Username,
                RoleId = user.RoleId
            };
        }

        public async Task<bool> UpdateAsync(UpdateUserDto dto)
        {
            var user = await _context.Users.FindAsync(dto.UserId);
            if (user == null) return false;

            user.Username = dto.Username;
            if (!string.IsNullOrEmpty(dto.Password))
            {
                user.PasswordHash = HashPassword(dto.Password);
            }
            user.RoleId = dto.RoleId;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
