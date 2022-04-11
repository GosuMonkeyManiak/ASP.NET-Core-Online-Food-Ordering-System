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

        public async Task<IEnumerable<Table>> AllWithPositionsAndSizes()
            => await this.DbSet
                .Include(t => t.TablePosition)
                .Include(t => t.TableSize)
                .AsNoTracking()
                .ToListAsync();
    }
}
