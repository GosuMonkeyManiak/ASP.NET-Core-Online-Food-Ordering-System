﻿namespace FoodFun.Core.Contracts
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
            long quantity);

        Task<Tuple<IEnumerable<ProductServiceModel>, int, int, int>> All(
            string searchTerm, 
            int categoryFilterId,
            byte order,
            int page,
            int pageSize,
            bool onlyAvailable = true);

        Task<ProductServiceModel> GetByIdOrDefault(string id);

        Task<bool> Update(
            string id,
            string name,
            string imageUrl,
            int categoryId,
            decimal price,
            string description,
            long quantity);

        Task<bool> IsProductExist(string productId);
    }
}
