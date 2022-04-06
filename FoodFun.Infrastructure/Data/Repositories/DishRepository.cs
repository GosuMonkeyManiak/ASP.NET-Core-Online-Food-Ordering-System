namespace FoodFun.Infrastructure.Data.Repositories
{
    using Common.Contracts;
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class DishRepository : EfRepository<Dish>, IDishRepository
    {
        public DishRepository(FoodFunDbContext dbContext) 
            : base(dbContext)
        {
        }

        private IQueryable<Dish> DishesWithCategories
            => this.DbSet
                .Include(p => p.Category)
                .AsNoTracking();

        public async Task<Dish> GetDishWithCategoryById(string id)
            => await this.DbSet
                .Include(x => x.Category)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

        public async Task<IEnumerable<Dish>> GetDishesWithCategories()
            => await this.DishesWithCategories
                .ToListAsync();

        public async Task<IEnumerable<Dish>> GetAllDishesWithCategories(
            string searchTerm,
            int categoryFilterId,
            byte orderNumber,
            int pageNumber,
            int pageSize,
            bool onlyAvailable)
        {
            var query = this.DishesWithCategories;

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
            var query = this.DishesWithCategories;

            query = AddFilters(
                query,
                searchTerm,
                categoryFilterId,
                onlyAvailable);

            var result = await query
                .ToListAsync();

            return result.Count;
        }

        private IQueryable<Dish> AddFilters(
            IQueryable<Dish> query,
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

        public async Task<IEnumerable<Dish>> All(string[] ids)
            => await this.DbSet
                .AsNoTracking()
                .Include(c => c.Category)
                .Where(x => ids.Contains(x.Id))
                .ToListAsync();
    }
}
