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

        public async Task<ProductCategory> GetCategoryWithProductsById(int id)
            => await this.DbSet
                .Include(x => x.Products)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

        public async Task<IEnumerable<ProductCategory>> GetAllNotDisabled()
            => await this.DbSet
                .Where(x => !x.IsDisable)
                .AsNoTracking()
                .ToListAsync();
    }
}
