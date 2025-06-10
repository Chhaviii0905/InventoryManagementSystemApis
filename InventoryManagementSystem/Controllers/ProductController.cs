using InventoryManagementSystem.Services;
using InventoryManagementSystem.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace InventoryManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ProductService _productService;

        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

        // GET: api/Product
        [HttpGet]
        public async Task<ActionResult<List<ProductDto>>> GetAll()
        {
            var products = await _productService.GetAllAsync();
            return Ok(products); // 200 with list of products
        }

        // GET: api/Product/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> Get(int id)
        {
            var product = await _productService.GetByIdAsync(id);
            if (product == null)
                return NotFound(); // 404

            return Ok(product);
        }

        // POST: api/Product
        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult<ProductDto>> Create(CreateProductDto dto)
        {
            var created = await _productService.CreateAsync(dto);
            return CreatedAtAction(nameof(Get), new { id = created.ProductId }, created); // 201
        }

        // PUT: api/Product
        [HttpPut]
        public async Task<IActionResult> Update(UpdateProductDto dto)
        {
            var success = await _productService.UpdateAsync(dto);
            if (!success)
                return NotFound(); // 404

            return NoContent(); // 204
        }

        // DELETE: api/Product/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _productService.DeleteAsync(id);
            if (!success)
                return NotFound(); // 404

            return NoContent(); // 204
        }

        [HttpPatch("{id}/stock")]
        public async Task<IActionResult> UpdateStock(int id, [FromBody] UpdateStockDto dto)
        {
            if (id != dto.ProductId)
                return BadRequest("Product ID mismatch.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var success = await _productService.UpdateStockAsync(dto);
            if (!success) return NotFound("Product not found.");

            return Ok("Stock updated successfully.");
        }

    }
}
