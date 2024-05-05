using WebApplication2.Models;

namespace WebApplication2.Repositories;

public interface IWarehouseRepository
{
    public Task<Product?> GetProductByIdAsync(int id);
    public Task<Models.Warehouse?> GetWarehouseByIdAsync(int id);
    public Task<Order?> GetOrderByIdAsync(int id, int amount);
}