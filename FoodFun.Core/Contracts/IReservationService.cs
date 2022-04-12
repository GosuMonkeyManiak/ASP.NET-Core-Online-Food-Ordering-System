namespace FoodFun.Core.Contracts
{
    using FoodFun.Core.Models.Reservation;
    using FoodFun.Core.Models.Table;

    public interface IReservationService
    {
        Task<IEnumerable<TableServiceModel>> FreeTables(DateOnly date);

        Task<bool> Reserv(DateOnly date, string tableId, string userId);

        Task<IEnumerable<ReservationServiceModel>> AllByDate(DateOnly date);
    }
}
