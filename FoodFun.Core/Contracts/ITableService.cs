namespace FoodFun.Core.Contracts
{
    using FoodFun.Core.Models.Table;

    public interface ITableService
    {
        Task<IEnumerable<TableServiceModel>> All(string searchTerm = null);

        Task<bool> IsTableExist(string id);

        Task Add(int sizeId, int positionId);

        Task<TableServiceModel> GetByIdOrDefault(string id);

        Task Update(string id, int positionId, int sizeId);
    }
}
