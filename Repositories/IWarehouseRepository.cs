using WebApplication2.Models;

namespace WebApplication2.Repositories;

public interface IWarehouseRepository
{
    public Task<Models.Warehouse?> GetWarehouseByIdAsync(int id);
    public Task<bool> CheckIfProductWarehouseRecordExistAsync(int idOrder);

    public Task<int?> RegisterProductInWarehouseAsync(int idWarehouse, int idProduct, int idOrder,
        DateTime createdAt, int amount, decimal price);

    public Task RegisterProductInWarehouseByProcedureAsync(int idWarehouse, int idProduct, int amount, DateTime createdAt);
}