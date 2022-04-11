namespace FoodFun.Infrastructure.Data.Repositories
{
    using FoodFun.Infrastructure.Common.Contracts;
    using FoodFun.Infrastructure.Models;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class ReservationRepository : EfRepository<Reservation>, IReservationRepository
    {
        public ReservationRepository(FoodFunDbContext dbContext) 
            : base(dbContext)
        {
        }

        public async Task<IEnumerable<Reservation>> GetAllByDate(DateOnly date)
            => await this.DbSet
                .Include(t => t.Table)
                .AsNoTracking()
                .Where(t => t.Date == date.ToDateTime(new TimeOnly()))
                .ToListAsync();
    }
}
