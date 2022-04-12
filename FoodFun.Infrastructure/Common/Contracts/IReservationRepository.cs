namespace FoodFun.Infrastructure.Common.Contracts
{
    using FoodFun.Infrastructure.Models;

    public interface IReservationRepository : IRepository<Reservation>
    {
        Task<IEnumerable<Reservation>> GetAllWithTablesByDate(DateOnly date);

        Task<IEnumerable<Reservation>> All(DateOnly date);
    }
}
