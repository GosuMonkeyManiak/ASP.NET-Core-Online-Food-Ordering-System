namespace FoodFun.Infrastructure.Data.Repositories
{
    using Common;
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

        public async Task<Product> GetProductWithCategoryById(string id)
            => await this.DbSet
                .Include(p => p.Category)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);

        public async Task<IEnumerable<Product>> GetAllProductsWithCategories(
            string searchTerm, 
            int categoryFilterId,
            byte orderNumber,
            int pageNumber)
        {
            var query = this.ProductsWithCategories;

            query = AddFilters(
                query,
                searchTerm,
                categoryFilterId);

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
                .Skip((pageNumber - 1) * DataConstants.ItemPerPage)
                .Take(DataConstants.ItemPerPage)
                .ToListAsync();
        }

        public async Task<int> GetNumberOfPagesByFilter(
            string searchTerm, 
            int categoryFilterId)
        {
            var query = this.ProductsWithCategories;

            query = AddFilters(
                query,
                searchTerm,
                categoryFilterId);

            var result = await query
                .ToListAsync();

            return result.Count;
        }

        private IQueryable<Product> AddFilters(
            IQueryable<Product> query,
            string searchTerm,
            int categoryFilterId)
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

            return query
                .Where(x => !x.Category.IsDisable);
        }
    }
}
