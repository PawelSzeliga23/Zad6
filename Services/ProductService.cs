using WebApplication2.Models;
using WebApplication2.Repositories;

namespace WebApplication2.Warehouse;

public class ProductService : IProductService
{
    private readonly IProductRepository _repository;

    public ProductService(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<Product?> GetProductByIdAsync(int id)
    {
        return await _repository.GetProductByIdAsync(id);
    }
}