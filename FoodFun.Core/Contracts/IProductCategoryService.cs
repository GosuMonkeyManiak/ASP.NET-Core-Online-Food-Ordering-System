namespace FoodFun.Core.Contracts
{
    using Models.ProductCategory;

    public interface IProductCategoryService
    {
        Task<bool> Add(string title);

        Task<IEnumerable<ProductCategoryWithProductCountServiceModel>> AllWithProductsCount();

        Task<IEnumerable<ProductCategoryServiceModel>> All();

        Task<IEnumerable<ProductCategoryServiceModel>> AllNotDisabled();

        Task<bool> IsCategoryActive(int id);

        Task<bool> IsCategoryExist(int id);

        Task<ProductCategoryServiceModel> GetByIdOrDefault(int id);

        Task<Tuple<bool, bool>> Update(
            int categoryId,
            string title);

        Task<bool> IsProductInCategory(
            int categoryId, 
            string productName);

        Task<bool> Disable(int id);

        Task<bool> Enable(int id);
    }
}
