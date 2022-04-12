namespace FoodFun.Core.Contracts
{
    public interface ITablePositionService
    {
        Task Add(string position);

        Task<bool> IsExist(string position);
    }
}