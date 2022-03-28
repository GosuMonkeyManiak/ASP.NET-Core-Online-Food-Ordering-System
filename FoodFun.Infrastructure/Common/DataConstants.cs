namespace FoodFun.Infrastructure.Common
{
    public class DataConstants
    {
        public const int PublicPageSize = 3;
        public const int SupermarketPageSize = 8;

        public const string PriceMinLength = "0.10";
        public const string PriceMaxLength = "500.0";

        public const int DescriptionMinLength = 10;

        public const int DefaultIdMaxLength = 450;
        public const int UrlMaxLength = 2048;

        public class User
        {
            public const int UserNameMinLength = 3;
            public const int UserNameMaxLength = 256;

            public const int PasswordMinLength = 6;
        }

        public class ProductCategory
        {
            public const int TitleMinLength = 4;
            public const int TitleMaxLength = 30;
        }

        public class Product
        {
            public const int NameMinLength = 4;
            public const int NameMaxLength = 100;

            public const int QuantityMinLength = 0;
        }

        public class DishCategory
        {
            public const int TitleMinLength = 3;
            public const int TitleMaxLength = 80;
        }

        public class Dish
        {
            public const int NameMinLength = 5;
            public const int NameMaxLength = 120;
        }

        public class TablePosition
        {
            public const int PositionMaxLength = 20;
        }

        public class Address
        {
            public const int NameMaxLength = 50;
        }

        public class Role
        {
            public const int TitleMinLength = 4;
            public const int TitleMaxLength = 256;
        }
    }
}
