using System.Data.SqlClient;
using WebApplication2.Models;
using WebApplication2.Warehouse;

namespace WebApplication2.Repositories;

public class WarehouseRepository : IWarehouseRepository
{
    private readonly IConfiguration _configuration;

    public WarehouseRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<Product?> GetProductByIdAsync(int id)
    {
        Product? product = null;

        using (SqlConnection connection = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]))
        {
            await connection.OpenAsync();

            string query = "SELECT IdProduct, Name, Description, Price FROM Product WHERE Id = @Id";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Id", id);

                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        product = new Product
                        {
                            Id = (int)reader["IdProduct"],
                            Name = reader["Name"].ToString()!,
                            Description = reader["Description"].ToString()!,
                            Price = (double)reader["Price"]
                        };
                    }
                    else
                    {
                        throw new NotFoundException($"Product with id: {id} not found");
                    }
                }
            }
        }

        return product;
    }

    public async Task<Models.Warehouse?> GetWarehouseByIdAsync(int id)
    {
        Models.Warehouse? warehouse = null;

        using (SqlConnection connection = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]))
        {
            await connection.OpenAsync();

            string query = "SELECT IdWarehouse, Name, Address FROM Warehouse WHERE Id = @Id";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Id", id);

                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        warehouse = new Models.Warehouse()
                        {
                            Id = (int)reader["IdProduct"],
                            Name = reader["Name"].ToString()!,
                            Adress = reader["Address"].ToString()!
                        };
                    }
                    else
                    {
                        throw new NotFoundException($"Warehouse with id: {id} not found");
                    }
                }
            }
        }

        return warehouse;
    }

    public async Task<Order?> GetOrderByIdAsync(int id, int amount)
    {
        Order order = null;

        using (SqlConnection connection = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]))
        {
            await connection.OpenAsync();

            string query = "SELECT IdOrder, IdProduct, Amount, CreatedAt FROM Order WHERE IdProduct = @Id AND Amount = @Amount";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Id", id);
                command.Parameters.AddWithValue("@Amount", amount);

                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        order = new Order()
                        {
                            IdOrder = (int)reader["IdOrder"],
                            IdProduct = (int)reader["IdProduct"],
                            Amount = (int)reader["Amount"],
                            DateTime = (DateTime)reader["CreatedAt"]
                        };
                    }
                    else
                    {
                        throw new NotFoundException($"Order with idProduct: {id} and amount {amount} not found");
                    }
                }
            }
        }

        return order;
    }
}