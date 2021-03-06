namespace FoodFun.Infrastructure.Common.Contracts
{
    using Models;

    public interface IProductCategoryRepository : IRepository<ProductCategory>
    {
        Task<IEnumerable<ProductCategory>> GetAllCategoriesWithProducts();

        Task<ProductCategory> GetCategoryWithProductsById(int id);

        Task<IEnumerable<ProductCategory>> GetAllNotDisabled();
    }
}
