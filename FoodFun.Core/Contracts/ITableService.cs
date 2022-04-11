namespace FoodFun.Core.Contracts
{
    using FoodFun.Core.Models.Table;

    public interface ITableService
    {
        Task<IEnumerable<TableServiceModel>> All();
    }
}
