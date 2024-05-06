using System.Data.SqlClient;
using WebApplication2.Models;
using WebApplication2.Warehouse;

namespace WebApplication2.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly IConfiguration _configuration;

    public OrderRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<Order?> GetOrderByIdAsync(int id, int amount)
    {
        using SqlConnection connection = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);

        await connection.OpenAsync();

        string query =
            "SELECT IdOrder, IdProduct, Amount, CreatedAt FROM [Order] WHERE IdProduct = @Id AND Amount = @Amount";
        using SqlCommand command = new SqlCommand(query, connection);

        command.Parameters.AddWithValue("@Id", id);
        command.Parameters.AddWithValue("@Amount", amount);

        using SqlDataReader reader = await command.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            var order = new Order()
            {
                IdOrder = (int)reader["IdOrder"],
                IdProduct = (int)reader["IdProduct"],
                Amount = (int)reader["Amount"],
                DateTime = (DateTime)reader["CreatedAt"]
            };
            return order;
        }
        else
        {
            throw new NotFoundException($"Order with idProduct: {id} and amount {amount} not found");
        }
    }
}