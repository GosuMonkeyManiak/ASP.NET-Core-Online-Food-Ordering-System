namespace FoodFun.Infrastructure.Data.Common
{
    public class DataConstants
    {
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
            public const int TitleMaxLength = 30;
        }

        public class Product
        {
            public const int NameMaxLength = 100;
        }

        public class DishCategory
        {
            public const int TitleMaxLength = 80;
        }

        public class Dish
        {
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
    }
}
