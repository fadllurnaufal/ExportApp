using ExportApp.Models;
using System.Data;
using Microsoft.Data.SqlClient;


namespace ExportApp.Repositories
{
    public class ProductRepository : IProductRepository
{
    private readonly string _connString;

    public ProductRepository(IConfiguration config)
    {
        _connString = config.GetConnectionString("DefaultConnection");
    }

    public List<Product> GetProducts()
    {
        var result = new List<Product>();
        using var conn = new SqlConnection(_connString);
        using var cmd = new SqlCommand("GetProduct", conn)
        {
            CommandType = CommandType.StoredProcedure
        };

        conn.Open();
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            result.Add(new Product
            {
                ID = reader.GetInt32(0),
                Name = reader.GetString(1),
                Price = reader.GetDouble(2)
            });
        }
        return result;
    }
}

}
