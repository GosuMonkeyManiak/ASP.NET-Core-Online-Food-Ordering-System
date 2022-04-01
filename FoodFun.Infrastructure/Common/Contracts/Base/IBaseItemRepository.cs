namespace FoodFun.Infrastructure.Common.Contracts.Base
{
    using System.Threading.Tasks;

    public interface IBaseItemRepository
    {
        Task<int> GetCountOfItemsByFilters(
            string searchTerm,
            int categoryFilterId,
            bool onlyAvailable);
    }
}
