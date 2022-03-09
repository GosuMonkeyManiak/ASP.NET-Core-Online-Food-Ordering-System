namespace FoodFun.Core.Services
{
    using Contracts;
    using Infrastructure.Data;
    using Infrastructure.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Models.Products;

    public class ProductService : IProductService
    {
        private readonly FoodFunDbContext dbContext;

        public ProductService(FoodFunDbContext dbContext) 
            => this.dbContext = dbContext;

        public async Task<List<ProductCategoryServiceModel>> GetCategories()
            => await this.dbContext.ProductsCategories
                .Select(c => new ProductCategoryServiceModel()
                {
                    Id = c.Id,
                    Title = c.Title
                })
                .ToListAsync();

        public async Task<Tuple<bool, IEnumerable<string>>> AddProduct(
            string name, 
            string imageUrl, 
            int categoryId, 
            decimal price, 
            string description)
        {
            if (!await IsCategoryExist(categoryId))
            {
                return new(false, new List<string>() { "Category doesn't exist!" });
            }

            var product = new Product()
            {
                Name = name,
                ImageUrl = imageUrl,
                CategoryId = categoryId,
                Price = price,
                Description = description
            };

            await this.dbContext
                .Products
                .AddAsync(product);

            await this.dbContext.SaveChangesAsync();

            return new(true, new List<string>());
        }

        public async Task<IEnumerable<ProductServiceModel>> All()
            => await this.dbContext
                .Products
                .Include(p => p.Category)
                .Select(p => new ProductServiceModel()
                {
                    Id = p.Id,
                    Name = p.Name,
                    ImageUrl = p.ImageUrl,
                    Category = new ProductCategoryServiceModel()
                    {
                        Id = p.CategoryId,
                        Title = p.Category.Title
                    },
                    Price = p.Price,
                    Description = p.Description
                })
                .ToListAsync();

        private async Task<bool> IsCategoryExist(int categoryId)
            => await this.dbContext
                .ProductsCategories
                .AnyAsync(x => x.Id == categoryId);
    }
}
