namespace FoodFun.Infrastructure.Common.Contracts
{
    using Models;

    public interface IProductRepository : IRepository<Product>
    {
        Task<Product> GetProductWithCategoryById(string id);

        Task<IEnumerable<Product>> GetAllProductsWithCategories(
            string searchTerm, 
            int categoryFilterId,
            byte orderNumber,
            int pageNumber);

        Task<int> GetNumberOfPagesByFilter(
            string searchTerm,
            int categoryFilterId);
    }
}
