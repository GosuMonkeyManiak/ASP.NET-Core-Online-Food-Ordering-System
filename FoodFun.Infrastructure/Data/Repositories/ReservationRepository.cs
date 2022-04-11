namespace FoodFun.Infrastructure.Data.Repositories
{
    using FoodFun.Infrastructure.Common.Contracts;
    using FoodFun.Infrastructure.Models;

    public class ReservationRepository : EfRepository<Reservation>, IReservationRepository
    {
        public ReservationRepository(FoodFunDbContext dbContext) 
            : base(dbContext)
        {
        }
    }
}
