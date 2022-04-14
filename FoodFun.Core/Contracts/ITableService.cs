namespace FoodFun.Core.Contracts
{
    using FoodFun.Core.Models.Table;

    public interface ITableService
    {
        Task<IEnumerable<TableServiceModel>> All();

        Task<bool> IsTableExist(string id);

        Task Add(int sizeId, int positionId);
    }
}
