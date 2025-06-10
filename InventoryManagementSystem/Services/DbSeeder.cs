using InventoryManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace InventoryManagementSystem.Services
{
    public class DbSeeder
    {
        private readonly InventoryDbContext _context;

        public DbSeeder(InventoryDbContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            // Seed roles if none exist
            if (!await _context.Roles.AnyAsync())
            {
                _context.Roles.AddRange(
                    new Role { Name = "Admin" },
                    new Role { Name = "Manager" },
                    new Role { Name = "Staff" }
                );
                await _context.SaveChangesAsync();
            }

            // Seed users if none exist
            if (!await _context.Users.AnyAsync())
            {
                var adminRole = await _context.Roles.FirstAsync(r => r.Name == "Admin");
                var managerRole = await _context.Roles.FirstAsync(r => r.Name == "Manager");
                var staffRole = await _context.Roles.FirstAsync(r => r.Name == "Staff");

                _context.Users.AddRange(
                    new User
                    {
                        Username = "admin",
                        PasswordHash = HashPassword("admin123"),
                        RoleId = adminRole.RoleId
                    },
                    new User
                    {
                        Username = "manager",
                        PasswordHash = HashPassword("manager123"),
                        RoleId = managerRole.RoleId
                    },
                    new User
                    {
                        Username = "staff",
                        PasswordHash = HashPassword("staff123"),
                        RoleId = staffRole.RoleId
                    }
                );

                await _context.SaveChangesAsync();
            }
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}
