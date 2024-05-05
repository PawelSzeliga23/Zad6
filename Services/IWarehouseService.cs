namespace WebApplication2.Warehouse;

public interface IWarehouseService
{
    Task<int?> RegisterProductInWarehouseAsync(WarehouseDto dto);
}