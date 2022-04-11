namespace FoodFun.Core.Contracts
{
    using FoodFun.Core.Models.Table;

    public interface IReservationService
    {
        Task<IEnumerable<TableServiceModel>> FreeTables(DateOnly date);
    }
}
