using ExportApp.Models;

namespace ExportApp.Repositories
{
    public interface IProductRepository
    {
        List<Product> GetProducts();
    }
}
