using InventoryManagementSystem.Models;
using InventoryManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ActivityLogController : ControllerBase
    {
        private readonly ActivityLogService _logService;

        public ActivityLogController(ActivityLogService logService)
        {
            _logService = logService;
        }

        // Get all logs
        [HttpGet]
        public async Task<ActionResult<List<ActivityLog>>> GetAllLogs()
        {
            var logs = await _logService.GetAllLogsAsync();
            return Ok(logs);
        }

        // Get logs by user
        [HttpGet("user/{username}")]
        public async Task<ActionResult<List<ActivityLog>>> GetLogsByUser(string username)
        {
            var logs = await _logService.GetLogsByUserAsync(username);
            return Ok(logs);
        }

        // Get logs by entity
        [HttpGet("entity/{entity}/{entityId}")]
        public async Task<ActionResult<List<ActivityLog>>> GetLogsByEntity(string entity, string entityId)
        {
            var logs = await _logService.GetLogsByEntityAsync(entity, entityId);
            return Ok(logs);
        }
    }
}
