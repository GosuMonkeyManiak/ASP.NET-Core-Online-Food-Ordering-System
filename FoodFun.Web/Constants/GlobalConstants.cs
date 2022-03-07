namespace FoodFun.Web.Constants
{
    public static class GlobalConstants
    {
        public const string Title = nameof(Title);

        public static class Redirect
        {
            public const string HomeIndexUrl = "/Home/Index";
        }

        public static class Messages
        {
            public const string UsernameError = "{0} must be between {2} and {1} characters.";
            public const string PasswordError = "{0} must be with minimum length of {1} characters.";

            public const string AccountExist = "Account already exist!";
            public const string AccountLockOut = "Account locked out for one hour.Please try again later.";
            public const string InvalidCredentials = "Invalid credentials.";
        }
    }
}
