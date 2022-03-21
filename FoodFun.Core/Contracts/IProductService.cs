namespace FoodFun.Core.Contracts
{
    using Models.Product;

    public interface IProductService
    {
        Task<Tuple<bool, bool>> AddProduct(
            string name,
            string imageUrl,
            int categoryId,
            decimal price,
            string description,
            long quantity);

        Task<Tuple<IEnumerable<ProductServiceModel>, int, int, int>> All(
            string searchTerm, 
            int categoryFilterId,
            byte order,
            int page);

        Task<ProductServiceModel> GetByIdOrDefault(string id);

        Task<Tuple<bool, bool, bool>> Update(
            string id,
            string name,
            string imageUrl,
            int categoryId,
            decimal price,
            string description);

        Task<bool> Delete(string id);
    }
}
