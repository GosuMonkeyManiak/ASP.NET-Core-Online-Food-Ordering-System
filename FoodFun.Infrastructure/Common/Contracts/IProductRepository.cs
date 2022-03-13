namespace FoodFun.Infrastructure.Common.Contracts
{
    using Models;

    public interface IProductRepository : IRepository<Product>
    {
        Task<Product> GetProductWithCategoryById(string id);

        Task<IEnumerable<Product>> GetAllProductsWithCategories();
    }
}
