namespace FoodFun.Core.Constants
{
    public class ValidationConstants
    {
        public const string CategoryNotExist = "Category doesn't exist!";
        public const string CategoryAlreadyExist = "Category already exist!";

        public const string ItemNotExist = "Item doesn't exist.";
        public const string ItemAlreadyExistInCategory = "Item already exist in category!";
        public const string ItemMustBeInActiveCategory = "Item must be in active category!";

        public const string ProductNotExist = "Product doesn't exist!";
        public const string DishNotExist = "Dish doesn't exist!";

        public const string DateMustBeNowOrInTheFuture = "Date must be today or in the future!";

        public const string TableNotExist = "Table doesn't exist";
        public const string TablePositionAlreadyExist = "Table position already exist!";
    }
}
