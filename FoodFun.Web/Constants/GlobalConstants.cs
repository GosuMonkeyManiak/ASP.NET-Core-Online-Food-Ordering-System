namespace FoodFun.Web.Constants
{
    public static class GlobalConstants
    {
        private const string ProductNotExistBase = "Product doesn't exist";
        private const string ProductCategoryNotExistBase = "Product category doesn't exist";

        private const string DishNotExistBase = "Dish doesn't exist";
        private const string DishCategoryNotExistBase = "Dish category doesn't exist";

        public const string Title = nameof(Title);
        public const int DefaultIdLength = 36;
        public const string CategoryId = nameof(CategoryId);

        public static class Redirect
        {
            public const string HomeIndexUrl = $"/{Home}/{Index}";
            public const string Home = nameof(Home);
            public const string Index = nameof(Index);
        }

        public static class Messages
        {
            public const string Error = nameof(Error);

            public const string UsernameError = "{0} must be between {2} and {1} characters.";
            public const string PasswordError = "{0} must be with minimum length of {1} characters.";

            public const string AccountExist = "Account already exist!";
            public const string AccountLockOut = "Account locked out for one hour.Please try again later.";
            public const string InvalidCredentials = "Invalid credentials.";

            public const string ProductNameError = "{0} of product must be between {2} and {1} characters.";
            public const string UrlError = "{0} is not valid.";
            public const string UrlErrorWithMaxLength = "{0} can't be more then {1} characters.";
            public const string ProductPriceError = "{0} must be between {1} and {2}.";
            public const string DescriptionError = "{0} must be with a minimum {1} characters.";

            public const string ProductNotExist = $"{ProductNotExistBase}!";
            public const string ProductCategoryNotExist = $"{ProductCategoryNotExistBase}!";
            public const string ProductAndCategoryNotExist = $"{ProductNotExistBase} or {ProductCategoryNotExistBase}!";
            public const string ProductCategoryAlreadyExist = "Product category already exist!";
            public const string ProductAlreadyExistInCategory = "Product already exists in category!";

            public const string ProductCategoryIdError = "{0} must be between {1} and {2}.";
            public const string CategoryTitleError = "{0} of category must be between {2} and {1} characters.";

            public const string DishNameError = "{0} of dish must be between {2} and {1} characters.";
            public const string DishPriceError = ProductPriceError;
            public const string DishNotExit = $"{DishNotExistBase}!";
            public const string DishAlreadyExistInCategory = "Dish already exist in category!";

            public const string DishCategoryNotExist = $"{DishCategoryNotExistBase}!";
            public const string DishAndCategoryNotExit = $"{DishNotExistBase} or {DishCategoryNotExistBase}!";
            public const string DishCategoryAlreadyExist = "Dish category already exist!";
        }

        public static class Roles
        {
            public const string Administrator = nameof(Administrator);
            public const string OrderManager = "Order Manager";
            public const string RestaurantManager = "Restaurant Manager";
            public const string Customer = nameof(Customer);
        }

        public static class Areas
        {
            public const string Products = nameof(Products);
            public const string Identity = nameof(Identity);
            public const string Administration = nameof(Administration);
        }
    }
}
