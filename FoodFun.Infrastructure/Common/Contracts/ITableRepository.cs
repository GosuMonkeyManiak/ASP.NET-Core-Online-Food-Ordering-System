namespace FoodFun.Infrastructure.Common.Contracts
{
    using FoodFun.Infrastructure.Models;

    public interface ITableRepository : IRepository<Table>
    {
        Task<IEnumerable<Table>> AllWithPositionsAndSizes(string searchTerm = null);

        Task<Table> GetWithPositionAndSizeById(string id);
    }
}
