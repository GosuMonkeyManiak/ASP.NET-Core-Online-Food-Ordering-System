namespace FoodFun.Core.Contracts
{
    using Models.ProductCategory;

    public interface IProductCategoryService
    {
        Task<bool> Add(string title);

        Task<IEnumerable<ProductCategoryWithProductCountServiceModel>> AllWithProductsCount();

        Task<IEnumerable<ProductCategoryServiceModel>> All();

        Task<bool> IsCategoryExist(int id);

        Task<Tuple<bool, ProductCategoryServiceModel>> GetById(int id);

        Task<bool> Update(
            int categoryId,
            string title);
    }
}
