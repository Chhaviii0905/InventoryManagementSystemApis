using InventoryManagementSystem.DTOs;
using InventoryManagementSystem.Models; // EF models
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementSystem.Services
{
    public class ProductService
    {
        private readonly InventoryDbContext _context;
        private readonly ActivityLogService _logService;

        public ProductService(InventoryDbContext context, ActivityLogService logService)
        {
            _context = context;
            _logService = logService;
        }

        public async Task<List<ProductDto>> GetAllAsync()
        {
            return await _context.Products
                .Select(p => new ProductDto
                {
                    ProductId = p.ProductId,
                    Name = p.Name,
                    Quantity = p.Quantity,
                    Price = p.Price
                })
                .ToListAsync();
        }

        public async Task<ProductDto?> GetByIdAsync(int id)
        {
            var p = await _context.Products.FindAsync(id);
            if (p == null) return null;

            return new ProductDto
            {
                ProductId = p.ProductId,
                Name = p.Name,
                Quantity = p.Quantity,
                Price = p.Price
            };
        }

        public async Task<ProductDto> CreateAsync(CreateProductDto dto, string performedBy)
        {
            var product = new Product
            {
                Name = dto.Name,
                Quantity = dto.Quantity,
                Price = dto.Price
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            // Logging
            await _logService.LogAsync(
                action: "Product Created",
                performedBy: performedBy,
                entity: "Product",
                entityId: product.ProductId.ToString()
            );

            return new ProductDto
            {
                ProductId = product.ProductId,
                Name = product.Name,
                Quantity = product.Quantity,
                Price = product.Price
            };
        }

        public async Task<bool> UpdateAsync(UpdateProductDto dto, string performedBy)
        {
            var p = await _context.Products.FindAsync(dto.ProductId);
            if (p == null) return false;

            p.Name = dto.Name;
            p.Quantity = dto.Quantity;
            p.Price = dto.Price;

            await _context.SaveChangesAsync();

            // Logging
            await _logService.LogAsync(
                action: "Product Updated",
                performedBy: performedBy,
                entity: "Product",
                entityId: p.ProductId.ToString()
            );

            return true;
        }

        public async Task<bool> DeleteAsync(int id, string performedBy)
        {
            var p = await _context.Products.FindAsync(id);
            if (p == null) return false;

            _context.Products.Remove(p);
            await _context.SaveChangesAsync();

            // Logging
            await _logService.LogAsync(
                action: "Product Deleted",
                performedBy: performedBy,
                entity: "Product",
                entityId: id.ToString()
            );

            return true;
        }

        public async Task<bool> UpdateStockAsync(UpdateStockDto dto, string performedBy)
        {
            var product = await _context.Products.FindAsync(dto.ProductId);
            if (product == null) return false;

            product.Stock += dto.StockToAdd;
            await _context.SaveChangesAsync();

            // Logging
            await _logService.LogAsync(
                action: "Product Stock Updated",
                performedBy: performedBy,
                entity: "Product",
                entityId: product.ProductId.ToString()
            );

            return true;
        }
    }
}
