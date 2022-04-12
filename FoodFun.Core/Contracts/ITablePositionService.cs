namespace FoodFun.Core.Contracts
{
    using FoodFun.Core.Models.TablePosition;

    public interface ITablePositionService
    {
        Task Add(string position);

        Task<IEnumerable<TablePositionServiceModel>> All();

        Task<bool> IsExist(string position);

        Task<bool> IsExist(int id);

        Task<TablePositionServiceModel> GetByIdOrDefault(int id);

        Task Update(int id, string position);
    }
}