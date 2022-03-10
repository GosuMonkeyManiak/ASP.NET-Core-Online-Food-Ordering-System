namespace FoodFun.Web.Constants
{
    public static class GlobalConstants
    {
        public const string Title = nameof(Title);

        public static class Redirect
        {
            public const string HomeIndexUrl = "/Home/Index";
            public const string Home = nameof(Home);
            public const string Index = nameof(Index);
        }

        public static class Messages
        {
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
