using WebApplication2.Models;

namespace WebApplication2.Repositories;

public interface IOrderRepository
{
    public Task<Order?> GetOrderByIdAsync(int id, int amount);
}