namespace FoodFun.Infrastructure.Data.Repositories
{
    using Common.Contracts;
    using Microsoft.EntityFrameworkCore;
    using Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class OrderRepository : EfRepository<Order>, IOrderRepository
    {
        public OrderRepository(FoodFunDbContext dbContext) 
            : base(dbContext)
        {
        }

        public async Task<IEnumerable<Order>> AllWithUsers(bool onlyActive)
        {
            var query = this.DbSet
                .Include(o => o.User)
                .AsNoTracking();

            if (onlyActive)
            {
                query = query
                    .Where(x => !x.IsDelivered);
            }

            return await query.ToListAsync();
        }

        public async Task<Order> ByItWithItems(int id)
            => await this.DbSet
                .Include(p => p.OrderProducts)
                .Include(u => u.User)
                .Include(d => d.OrderDishes)
                .AsNoTracking()
                .FirstAsync(x => x.Id == id);
    }
}
