using InventoryManagementSystem.DTOs;
using InventoryManagementSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        [Authorize(Roles = "Admin")]
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
        public async TaskActionResult<UserDto>> Create(CreateUserDto dto)
        {
            var createdUser = await _userService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = createdUser.UserId }, new
            {
                success = true,
                message = "User created successfully.",
                data = createdUser
            });
        }

        [HttpPut("{id}")]
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
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _userService.DeleteAsync(id);
            if (!deleted)
                return NotFound(new { success = false, message = "User not found." });

            return Ok(new { success = true, message = "User deleted successfully." });
        }
    }
}
