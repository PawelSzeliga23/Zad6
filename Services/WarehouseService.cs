using WebApplication2.Repositories;
using WebApplication2.Warehouse;

namespace WebApplication2.Services;

public class WarehouseService : IWarehouseService
{
    private IWarehouseRepository _warehouseRepository;
    private IProductRepository _productRepository;
    private IOrderRepository _orderRepository;

    public WarehouseService(IWarehouseRepository warehouseRepository, IProductRepository productRepository,
        IOrderRepository orderRepository)
    {
        _warehouseRepository = warehouseRepository;
        _productRepository = productRepository;
        _orderRepository = orderRepository;
    }

    public async Task<int?> RegisterProductInWarehouseAsync(WarehouseDto dto)
    {
        var product = await _productRepository.GetProductByIdAsync(dto.IdProduct ?? 0);
        var warehouse = await _warehouseRepository.GetWarehouseByIdAsync(dto.IdWarehouse ?? 0);
        if (dto.Amount <= 0)
        {
            throw new ConflictException("Amount must be grater than 0.");
        }

        var order = await _orderRepository.GetOrderByIdAsync(dto.IdProduct ?? 0, dto.Amount ?? 0);
        if (dto.CreatedAt < order!.DateTime)
        {
            throw new ConflictException("Order date is past the request date.");
        }

        if (await _warehouseRepository.CheckIfProductWarehouseRecordExistAsync(order.IdOrder))
        {
            throw new ConflictException("Order is already fulfilled.");
        }

        decimal price = order.Amount * product!.Price;

        var id = await _warehouseRepository.RegisterProductInWarehouseAsync(
            idWarehouse: dto.IdWarehouse!.Value,
            idProduct: dto.IdProduct!.Value,
            idOrder: order.IdOrder,
            createdAt: DateTime.Now,
            amount: dto.Amount!.Value,
            price: price);
        return id;
    }

    public async Task RegisterProductInWarehouseProcedureAsync(WarehouseDto dto)
    {
        await _warehouseRepository.RegisterProductInWarehouseByProcedureAsync(dto.IdWarehouse ?? 0,
            dto.IdProduct ?? 0, dto.Amount ?? 0,
            DateTime.Now);
    }
}