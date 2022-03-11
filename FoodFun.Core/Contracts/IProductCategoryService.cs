﻿namespace FoodFun.Core.Contracts
{
    using Models.ProductCategory;

    public interface IProductCategoryService
    {
        Task Add(string title);

        Task<IEnumerable<ProductCategoryWithProductCountServiceModel>> All();

        Task<bool> IsCategoryExist(int categoryId);

        Task<Tuple<bool, ProductCategoryServiceModel>> GetById(int id);

        Task<bool> Update(
            int categoryId,
            string title);
    }
}