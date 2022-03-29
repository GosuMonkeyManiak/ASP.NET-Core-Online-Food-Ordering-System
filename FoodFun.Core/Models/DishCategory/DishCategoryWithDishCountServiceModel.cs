namespace FoodFun.Core.Models.DishCategory
{
    public class DishCategoryWithDishCountServiceModel
    {
        public int Id { get; init; }

        public string Title { get; init; }

        public bool IsDisable { get; init; }

        public int Count { get; init; }
    }
}
