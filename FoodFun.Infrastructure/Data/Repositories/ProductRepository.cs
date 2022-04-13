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

        private IQueryable<Product> ProductsWithCategories 
            => this.DbSet
                .Include(p => p.Category)
                .AsNoTracking();

        public async Task<IEnumerable<Product>> All(string[] ids)
            => await this.ProductsWithCategories
                .Where(x => ids.Contains(x.Id))
                .ToListAsync();

        public async Task<Product> GetProductWithCategoryById(string id)
            => await this.ProductsWithCategories
                .FirstAsync(x => x.Id == id);

        public async Task<IEnumerable<Product>> GetAllProductsWithCategories(
            string searchTerm, 
            int categoryFilterId,
            byte orderNumber,
            int pageNumber,
            int pageSize,
            bool onlyAvailable)
        {
            var query = this.ProductsWithCategories;

            query = AddFilters(
                query,
                searchTerm,
                categoryFilterId,
                onlyAvailable);

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

            return await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> GetCountOfItemsByFilters(
            string searchTerm, 
            int categoryFilterId,
            bool onlyAvailable)
        {
            var query = this.ProductsWithCategories;

            query = AddFilters(
                query,
                searchTerm,
                categoryFilterId,
                onlyAvailable);

            var result = await query
                .ToListAsync();

            return result.Count;
        }

        private IQueryable<Product> AddFilters(
            IQueryable<Product> query,
            string searchTerm,
            int categoryFilterId,
            bool onlyAvailable)
        {
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

            if (onlyAvailable)
            {
                query = query
                    .Where(x => !x.Category.IsDisable)
                    .Where(x => x.Quantity > 0);
            }

            return query;
        }

        public async Task<IEnumerable<Product>> LatestFive()
            => (await this.DbSet
                .AsNoTracking()
                .Include(x => x.Category)
                .Where(x => x.Quantity > 0 && !x.Category.IsDisable)
                .ToListAsync())
                .TakeLast(5);
    }
}
