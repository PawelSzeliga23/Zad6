using System.Data.SqlClient;
using WebApplication2.Models;
using WebApplication2.Warehouse;

namespace WebApplication2.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly IConfiguration _configuration;

    public ProductRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<Product?> GetProductByIdAsync(int id)
    {

        using SqlConnection connection = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        await connection.OpenAsync();

        string query = "SELECT IdProduct, Name, Description, Price FROM dbo.Product WHERE IdProduct = @Id";
        using SqlCommand command = new SqlCommand(query, connection);

        command.Parameters.AddWithValue("@Id", id);

        using SqlDataReader reader = await command.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            var product = new Product
            {
                Id = (int)reader["IdProduct"],
                Name = reader["Name"].ToString()!,
                Description = reader["Description"].ToString()!,
                Price = (decimal)reader["Price"]
            };
            return product;
        }
        else
        {
            throw new NotFoundException($"Product with id: {id} not found");
        }
    }
}