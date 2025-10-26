using InventoryManagementSystem.DTOs;
using InventoryManagementSystem.Models;
using InventoryManagementSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly OrderService _orderService;

        public OrderController(OrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<List<OrderDto>>> GetAll()
        {
            var orders = await _orderService.GetAllAsync();
            return Ok(new
            {
                success = true,
                message = "Orders retrieved successfully.",
                data = orders
            });
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<OrderDto>> GetById(int id)
        {
            var order = await _orderService.GetByIdAsync(id);
            if (order == null) return NotFound();
            return Ok(new
            {
                success = true,
                message = "Order retrieved successfully.",
                data = order
            });
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<OrderDto>> Create(CreateOrderDto dto)
        {
            var order = await _orderService.CreateAsync(dto);
            if (order == null)
                return BadRequest("Invalid product or insufficient stock.");

            return CreatedAtAction(nameof(GetById), new { id = order.OrderId }, new
            {
                success = true,
                message = "Order created successfully.",
                data = order
            });
        }

        [HttpGet("getRecentOrders")]
        [Authorize]
        public async Task<IActionResult> GetRecent([FromQuery] int count = 5)
        {
            var recent = await _orderService.GetRecentAsync(count);
            
            return Ok(new { success = true, data = recent });
        }

        [HttpPut("{id}/status")]
        [Authorize]
        public async Task<IActionResult> UpdateStatus(int id, [FromQuery] string status)
        {
            if (string.IsNullOrWhiteSpace(status))
                return BadRequest(new { success = false, message = "Status is required." });

            var updated = await _orderService.UpdateStatusAsync(id, status);
            if (!updated)
                return NotFound(new { success = false, message = "Order not found." });

            return Ok(new { success = true, message = $"Order marked as {status}." });
        }

    }
}
