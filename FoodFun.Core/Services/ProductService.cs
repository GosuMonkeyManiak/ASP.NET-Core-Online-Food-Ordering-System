namespace FoodFun.Core.Services
{
    using Contracts;
    using Infrastructure.Data;
    using Infrastructure.Models;
    using Microsoft.EntityFrameworkCore;
    using Models.Product;
    using Models.ProductCategory;

    public class ProductService : IProductService
    {
        private readonly FoodFunDbContext dbContext;
        private readonly IProductCategoryService productCategoryService;

        public ProductService(
            FoodFunDbContext dbContext,
            IProductCategoryService productCategoryService)
        {
            this.dbContext = dbContext;
            this.productCategoryService = productCategoryService;
        }

        public async Task<List<ProductCategoryWithProductCountServiceModel>> GetCategories()
            => await this.dbContext.ProductsCategories
                .Select(c => new ProductCategoryWithProductCountServiceModel()
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
            if (!await this.productCategoryService.IsCategoryExist(categoryId))
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
                    Category = new ProductCategoryWithProductCountServiceModel()
                    {
                        Id = p.CategoryId,
                        Title = p.Category.Title
                    },
                    Price = p.Price,
                    Description = p.Description
                })
                .ToListAsync();

        public async Task<Tuple<bool, ProductServiceModel>> GetById(string productId)
        {
            if (!await IsProductExist(productId))
            {
                return new(false, null);
            }

            var product = await this.dbContext
                .Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == productId);

            var productServiceModel = new ProductServiceModel()
            {
                Id = product.Id,
                Name = product.Name,
                ImageUrl = product.ImageUrl,
                Price = product.Price,
                Category = new ProductCategoryWithProductCountServiceModel()
                {
                    Id = product.Category.Id,
                    Title = product.Category.Title
                },
                Description = product.Description
            };

            return new(true, productServiceModel);
        }

        public async Task<bool> Update(
            string id, 
            string name, 
            string imageUrl, 
            int categoryId,
            decimal price, 
            string description)
        {
            if (!await IsProductExist(id) || 
                !await this.productCategoryService.IsCategoryExist(categoryId))
            {
                return false;
            }

            var product = new Product()
            {
                Id = id,
                Name = name,
                ImageUrl = imageUrl,
                CategoryId = categoryId,
                Price = price,
                Description = description
            };

            this.dbContext.Entry(product).State = EntityState.Modified;

            await this.dbContext.SaveChangesAsync();

            return true;
        }

        private async Task<bool> IsProductExist(string productId)
            => await this.dbContext
                .Products
                .AnyAsync(x => x.Id == productId);
    }
}
