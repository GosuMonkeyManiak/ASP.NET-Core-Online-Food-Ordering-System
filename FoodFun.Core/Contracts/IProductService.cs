namespace FoodFun.Core.Contracts
{
    using Models.Product;

    public interface IProductService
    {
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
