namespace FoodFun.Infrastructure.Data.Repositories
{
    using FoodFun.Infrastructure.Common.Contracts;
    using FoodFun.Infrastructure.Models;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class TableRepository : EfRepository<Table>, ITableRepository
    {
        public TableRepository(FoodFunDbContext dbContext) 
            : base(dbContext)
        {
        }

        public async Task<IEnumerable<Table>> AllWithPositionsAndSizes(string searchTerm = null)
        {
            var query = this.DbSet
               .Include(t => t.TablePosition)
               .Include(t => t.TableSize)
               .AsNoTracking();

            if (searchTerm != null)
            {
                query = query
                    .Where(x =>
                        x.Id.Contains(searchTerm) ||
                        x.TablePosition.Position.Contains(searchTerm));
            }

            return await query.ToListAsync();
        }
    }
}
