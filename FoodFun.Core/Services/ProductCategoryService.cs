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

        public async Task<IEnumerable<ProductCategoryWithProductCountServiceModel>> All()
            => await this.dbContext
                .ProductsCategories
                .Include(pc => pc.Products)
                .Select(pc => new ProductCategoryWithProductCountServiceModel()
                {
                    Id = pc.Id,
                    Title = pc.Title,
                    ProductsCount = pc.Products.Count
                })
                .ToListAsync();

        public async Task<bool> IsCategoryExist(int categoryId)
            => await this.dbContext
                .ProductsCategories
                .AnyAsync(x => x.Id == categoryId);

        public async Task<Tuple<bool, ProductCategoryServiceModel>> GetById(int id)
        {
            if (!await IsCategoryExist(id))
            {
                return new(false, null);
            }

            var productCategory = await this.dbContext
                .ProductsCategories
                .FirstOrDefaultAsync(pc => pc.Id == id);

            var productCategoryServiceModel = new ProductCategoryServiceModel()
            {
                Id = productCategory.Id,
                Title = productCategory.Title
            };

            return new(true, productCategoryServiceModel);
        }

        public async Task<bool> Update(int categoryId, string title)
        {
            if (!await IsCategoryExist(categoryId))
            {
                return false;
            }

            var productCategory = new ProductCategory()
            {
                Id = categoryId,
                Title = title
            };

            this.dbContext.Attach(productCategory).State = EntityState.Modified;

            await this.dbContext.SaveChangesAsync();

            return true;
        }
    }
}
