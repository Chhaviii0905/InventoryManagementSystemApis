using InventoryManagementSystem.DTOs;
using InventoryManagementSystem.Services;
using Microsoft.AspNetCore.Authorization;
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
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDto>> GetById(int id)
        {
            var order = await _orderService.GetByIdAsync(id);
            if (order == null) return NotFound();
            return Ok(order);
        }

        [HttpPost]
        [Authorize(Roles = "Manager,Staff")]
        public async Task<ActionResult<OrderDto>> Create(CreateOrderDto dto)
        {
            var order = await _orderService.CreateAsync(dto);
            if (order == null)
                return BadRequest("Invalid product or insufficient stock.");

            return CreatedAtAction(nameof(GetById), new { id = order.OrderId }, order);
        }
    }
}
