namespace FoodFun.Core.Contracts
{
    using Models.ProductCategory;

    public interface IProductCategoryService
    {
        Task Add(string title);

        Task<IEnumerable<ProductCategoryServiceModel>> All();

        Task<bool> IsCategoryExist(int categoryId);
    }
}
