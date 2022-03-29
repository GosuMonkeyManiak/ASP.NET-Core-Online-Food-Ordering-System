namespace FoodFun.Core.Contracts
{
    using Models.ProductCategory;

    public interface IProductCategoryService : ICategory
    {
        Task Add(string title);

        Task<IEnumerable<ProductCategoryWithProductCountServiceModel>> AllWithProductsCount();

        Task<IEnumerable<ProductCategoryServiceModel>> All();

        Task<IEnumerable<ProductCategoryServiceModel>> AllNotDisabled();

        Task<ProductCategoryServiceModel> GetByIdOrDefault(int id);

        Task Update(
            int categoryId,
            string title);
    }
}
