namespace FoodFun.Core.Services
{
    using Contracts;
    using Infrastructure.Data;
    using Infrastructure.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Models.ProductCategory;

    public class ProductCategoryService : IProductCategoryService
    {
        private FoodFunDbContext dbContext;

        public ProductCategoryService(FoodFunDbContext dbContext)
            => this.dbContext = dbContext;

        public async Task Add(string title)
            => await this.dbContext
                .ProductsCategories
                .AddAsync(new ProductCategory() { Title = title });

        public async Task<IEnumerable<ProductCategoryServiceModel>> All()
            => await this.dbContext
                .ProductsCategories
                .Select(pc => new ProductCategoryServiceModel()
                {
                    Id = pc.Id,
                    Title = pc.Title
                })
                .ToListAsync();

        public async Task<bool> IsCategoryExist(int categoryId)
            => await this.dbContext
                .ProductsCategories
                .AnyAsync(x => x.Id == categoryId);
    }
}
