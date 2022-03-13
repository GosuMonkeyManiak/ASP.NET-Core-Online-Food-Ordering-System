namespace FoodFun.Infrastructure.Data.Repositories
{
    using Common.Contracts;
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class ProductRepository : EfRepository<Product>, IProductRepository
    {
        public ProductRepository(FoodFunDbContext dbContext) 
            : base(dbContext)
        {
        }

        public async Task<Product> GetProductWithCategoryById(string id)
            => await this.DbSet
                .Include(p => p.Category)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);

        public async Task<IEnumerable<Product>> GetAllProductsWithCategories()
            => await this.DbSet
                .Include(p => p.Category)
                .AsNoTracking()
                .ToListAsync();
    }
}
