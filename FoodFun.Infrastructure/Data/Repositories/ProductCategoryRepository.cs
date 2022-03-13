namespace FoodFun.Infrastructure.Data.Repositories
{
    using Common.Contracts;
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class ProductCategoryRepository : EfRepository<ProductCategory>, IProductCategoryRepository
    {
        public ProductCategoryRepository(FoodFunDbContext dbContext) 
            : base(dbContext)
        {
        }

        public async Task<IEnumerable<ProductCategory>> GetAllCategoriesWithProducts()
            => await this.DbSet
                .Include(c => c.Products)
                .ToListAsync();
    }
}
