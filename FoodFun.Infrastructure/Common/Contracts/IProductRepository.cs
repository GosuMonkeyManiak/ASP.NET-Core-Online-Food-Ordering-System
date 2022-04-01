namespace FoodFun.Infrastructure.Common.Contracts
{
    using Base;
    using Models;

    public interface IProductRepository : IRepository<Product>, IBaseItemRepository
    {
        Task<IEnumerable<Product>> All(string[] ids);

        Task<Product> GetProductWithCategoryById(string id);

        Task<IEnumerable<Product>> GetAllProductsWithCategories(
            string searchTerm, 
            int categoryFilterId,
            byte orderNumber,
            int pageNumber,
            int pageSize,
            bool onlyAvailable);
    }
}
