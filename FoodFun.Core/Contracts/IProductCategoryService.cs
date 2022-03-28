namespace FoodFun.Core.Contracts
{
    using Models.ProductCategory;

    public interface IProductCategoryService
    {
        Task Add(string title);

        Task<IEnumerable<ProductCategoryWithProductCountServiceModel>> AllWithProductsCount();

        Task<IEnumerable<ProductCategoryServiceModel>> All();

        Task<IEnumerable<ProductCategoryServiceModel>> AllNotDisabled();

        Task<bool> IsCategoryActive(int id);

        Task<bool> IsCategoryExist(int id);

        Task<bool> IsCategoryExist(string title);

        Task<ProductCategoryServiceModel> GetByIdOrDefault(int id);

        Task Update(
            int categoryId,
            string title);

        Task<bool> IsProductExistInCategory(
            int categoryId, 
            string productName);

        Task Disable(int id);

        Task Enable(int id);
    }
}
