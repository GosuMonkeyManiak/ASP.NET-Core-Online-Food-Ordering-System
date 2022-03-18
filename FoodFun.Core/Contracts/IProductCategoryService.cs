namespace FoodFun.Core.Contracts
{
    using Models.ProductCategory;

    public interface IProductCategoryService
    {
        Task<bool> Add(string title);

        Task<IEnumerable<ProductCategoryWithProductCountServiceModel>> AllWithProductsCount();

        Task<IEnumerable<ProductCategoryServiceModel>> All();

        Task<bool> IsCategoryExist(int id);

        Task<ProductCategoryServiceModel> GetByIdOrDefault(int id);

        Task<bool> Update(
            int categoryId,
            string title);
    }
}
