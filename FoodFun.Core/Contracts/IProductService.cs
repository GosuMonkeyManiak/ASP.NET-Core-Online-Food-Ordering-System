namespace FoodFun.Core.Contracts
{
    using Models.Product;

    public interface IProductService
    {
        Task Add(
            string name,
            string imageUrl,
            int categoryId,
            decimal price,
            string description,
            ulong quantity);

        Task<Tuple<IEnumerable<ProductServiceModel>, int, int, int>> All(
            string searchTerm, 
            int categoryFilterId,
            byte order,
            int page,
            int pageSize,
            bool onlyAvailable = true);

        Task<IEnumerable<ProductServiceModel>> All(string[] ids);

        Task<ProductServiceModel> GetByIdOrDefault(string id);

        Task<bool> Update(
            string id,
            string name,
            string imageUrl,
            int categoryId,
            decimal price,
            string description,
            ulong quantity);

        Task<bool> IsProductExist(string productId);

        Task<IEnumerable<LatestProductServiceModel>> Latest();

        Task<decimal> PriceForProducts(params string[] ids);
    }
}
