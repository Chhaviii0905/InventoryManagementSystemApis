using InventoryManagementSystem.DTOs;
using InventoryManagementSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace InventoryManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<List<UserDto>>> GetAll()
        {
            var users = await _userService.GetAllAsync();
            return Ok(new
            {
                success = true,
                message = "Users retrieved successfully.",
                data = users
            });
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<UserDto>> GetById(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null)
                return NotFound(new { success = false, message = "User not found." });

            return Ok(new
            {
                success = true,
                message = "User retrieved successfully.",
                data = user
            });
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(CreateUserDto dto)
        {
            var createdUser = await _userService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = createdUser.UserId }, new
            {
                success = true,
                message = "User created successfully.",
                data = createdUser
            });
        }

        [HttpGet("getCurrentUser")]
        [Authorize]
        public async Task<IActionResult> GetCurrentUser()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userIdClaim, out int userId))
                return Unauthorized();

            var user = await _userService.GetByIdAsync(userId);
            if (user == null) return NotFound(new { success = false, message = "User not found." });

            return Ok(new { success = true, data = user });
        }



        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, UpdateUserDto dto)
        {
            if (id != dto.UserId)
                return BadRequest(new { success = false, message = "User ID mismatch." });

            var updated = await _userService.UpdateAsync(dto);
            if (!updated)
                return NotFound(new { success = false, message = "User not found." });

            return Ok(new { success = true, message = "User updated successfully." });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _userService.DeleteAsync(id);
            if (!deleted)
                return NotFound(new { success = false, message = "User not found." });

            return Ok(new { success = true, message = "User deleted successfully." });
        }
    }
}
