namespace FoodFun.Core.Contracts
{
    using FoodFun.Core.Models.TableSize;

    public interface ITableSizeService
    {
        Task<IEnumerable<TableSizeServiceModel>> All();

        Task Add(int seats);

        Task<bool> IsExist(int seats);

        Task<bool> IsExistById(int id);

        Task<TableSizeServiceModel> GetByIdOrDefault(int id);
        Task Update(int id, int seats);
    }
}
