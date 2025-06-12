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
        [Authorize(Roles = "Manager,Staff")]
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
    }
}
