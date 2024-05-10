using System.Data;
using System.Data.SqlClient;
using WebApplication2.Warehouse;

namespace WebApplication2.Repositories;

public class WarehouseRepository : IWarehouseRepository
{
    private readonly IConfiguration _configuration;

    public WarehouseRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<Models.Warehouse?> GetWarehouseByIdAsync(int id)
    {
        using SqlConnection connection = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        await connection.OpenAsync();

        string query = "SELECT IdWarehouse, Name, Address FROM [Warehouse] WHERE IdWarehouse = @Id";
        using SqlCommand command = new SqlCommand(query, connection);

        command.Parameters.AddWithValue("@Id", id);

        using SqlDataReader reader = await command.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            var warehouse = new Models.Warehouse()
            {
                Id = (int)reader["IdWarehouse"],
                Name = reader["Name"].ToString()!,
                Adress = reader["Address"].ToString()!
            };
            return warehouse;
        }
        else
        {
            throw new NotFoundException($"Warehouse with id: {id} not found");
        }
    }

    public async Task<bool> CheckIfProductWarehouseRecordExistAsync(int idOrder)
    {
        using SqlConnection connection = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        await connection.OpenAsync();

        string query = "SELECT COUNT(*) FROM [Product_Warehouse] WHERE IdOrder = @IdOrder";
        using SqlCommand command = new SqlCommand(query, connection);

        command.Parameters.AddWithValue("@IdOrder", idOrder);
        bool exists = (int)(await command.ExecuteScalarAsync())! > 0;

        return exists;
    }

    public async Task<int?> RegisterProductInWarehouseAsync(int idWarehouse, int idProduct, int idOrder,
        DateTime createdAt, int amount, decimal price)
    {
        await using var connection = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        await connection.OpenAsync();

        await using var transaction = await connection.BeginTransactionAsync();

        try
        {
            var query = "UPDATE [Order] SET FulfilledAt = @FulfilledAt WHERE IdOrder = @IdOrder";
            await using var command = new SqlCommand(query, connection);
            command.Transaction = (SqlTransaction)transaction;
            command.Parameters.AddWithValue("@IdOrder", idOrder);
            command.Parameters.AddWithValue("@FulfilledAt", DateTime.UtcNow);
            await command.ExecuteNonQueryAsync();

            command.CommandText = @"
                      INSERT INTO Product_Warehouse (IdWarehouse, IdProduct, IdOrder, CreatedAt, Amount, Price)
                      OUTPUT Inserted.IdProductWarehouse
                      VALUES (@IdWarehouse, @IdProduct, @IdOrder, @CreatedAt, @Amount, @Price);";
            command.Parameters.Clear();
            command.Parameters.AddWithValue("@IdWarehouse", idWarehouse);
            command.Parameters.AddWithValue("@IdProduct", idProduct);
            command.Parameters.AddWithValue("@IdOrder", idOrder);
            command.Parameters.AddWithValue("@CreatedAt", createdAt);
            command.Parameters.AddWithValue("@Amount", amount);
            command.Parameters.AddWithValue("@Price", price);
            var idProductWarehouse = (int)(await command.ExecuteScalarAsync())!;

            await transaction.CommitAsync();
            return idProductWarehouse;
        }
        catch
        {
            await transaction.RollbackAsync();
            return null;
        }
    }

    public async Task RegisterProductInWarehouseByProcedureAsync(int idWarehouse, int idProduct, int amount, DateTime createdAt)
    {
        await using var connection = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        await connection.OpenAsync();
        await using var command = new SqlCommand("AddProductToWarehouse", connection);
        command.CommandType = CommandType.StoredProcedure;
        command.Parameters.AddWithValue("IdProduct", idProduct);
        command.Parameters.AddWithValue("IdWarehouse", idWarehouse);
        command.Parameters.AddWithValue("Amount", amount);
        command.Parameters.AddWithValue("CreatedAt", createdAt);
        await command.ExecuteNonQueryAsync();
    }
}