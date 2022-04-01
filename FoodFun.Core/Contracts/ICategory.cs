namespace FoodFun.Core.Contracts
{
    public interface ICategory
    {
        Task Disable(int id);

        Task Enable(int id);

        Task<bool> IsCategoryActive(int id);

        Task<bool> IsCategoryExist(int id);

        Task<bool> IsCategoryExist(string title);

        Task<bool> IsItemExistInCategory(int categoryId, string itemName);

        Task<bool> IsItemInAnyActiveCategory(string itemId);
    }
}
