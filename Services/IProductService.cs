using WebApplication2.Models;

namespace WebApplication2.Warehouse;

public interface IProductService
{
    public Task<Product?> GetProductByIdAsync(int id);
}