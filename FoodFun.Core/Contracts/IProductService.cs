namespace FoodFun.Core.Contracts
{
    using Models.Products;

    public interface IProductService
    {
        Task<List<ProductCategoryServiceModel>> GetCategories();

        Task<Tuple<bool, IEnumerable<string>>> AddProduct(
            string name,
            string imageUrl,
            int categoryId,
            decimal price,
            string description);

        Task<IEnumerable<ProductServiceModel>> All();

        Task<Tuple<bool, ProductServiceModel>> GetById(string productId);

        Task<bool> Update(
            string id,
            string name,
            string imageUrl,
            int categoryId,
            decimal price,
            string description);
    }
}
