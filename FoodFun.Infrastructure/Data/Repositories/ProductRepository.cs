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

        public async Task<IEnumerable<Product>> GetAllProductsWithCategories(
            string searchTerm, 
            int categoryFilterId,
            byte orderNumber)
        {
            var query = this.DbSet
                .Include(p => p.Category)
                .AsNoTracking();

            if (searchTerm != null)
            {
                query = query
                    .Where(x =>
                        x.Name.Contains(searchTerm) ||
                        x.Description.Contains(searchTerm));
            }

            if (categoryFilterId != 0)
            {
                query = query
                    .Where(x => x.CategoryId == categoryFilterId);
            }

            if (orderNumber == 1)
            {
                query = query
                    .OrderByDescending(x => x.Price);
            }
            else
            {
                query = query
                    .OrderBy(x => x.Price);
            }

            return await query.ToListAsync();
        }
    }
}
