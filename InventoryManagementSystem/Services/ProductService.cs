using InventoryManagementSystem.DTOs;
using InventoryManagementSystem.Models; // EF models
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementSystem.Services
{
    public class ProductService
    {
        private readonly InventoryDbContext _context;

        public ProductService(InventoryDbContext context)
        {
            _context = context;
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

        public async Task<ProductDto> CreateAsync(CreateProductDto dto)
        {
            var product = new Product
            {
                Name = dto.Name,
                Quantity = dto.Quantity,
                Price = dto.Price
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return new ProductDto
            {
                ProductId = product.ProductId,
                Name = product.Name,
                Quantity = product.Quantity,
                Price = product.Price
            };
        }

        public async Task<bool> UpdateAsync(UpdateProductDto dto)
        {
            var p = await _context.Products.FindAsync(dto.ProductId);
            if (p == null) return false;

            p.Name = dto.Name;
            p.Quantity = dto.Quantity;
            p.Price = dto.Price;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var p = await _context.Products.FindAsync(id);
            if (p == null) return false;

            _context.Products.Remove(p);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateStockAsync(UpdateStockDto dto)
        {
            var product = await _context.Products.FindAsync(dto.ProductId);
            if (product == null) return false;

            product.Stock += dto.StockToAdd;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
