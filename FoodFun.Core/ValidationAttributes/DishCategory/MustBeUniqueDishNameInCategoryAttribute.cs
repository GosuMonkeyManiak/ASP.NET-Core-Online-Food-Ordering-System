namespace FoodFun.Core.ValidationAttributes.DishCategory
{
    using Category;
    using Contracts;
    using Microsoft.Extensions.DependencyInjection;
    using System.ComponentModel.DataAnnotations;

    using static Constants.ValidationConstants;

    [AttributeUsage(AttributeTargets.Property)]
    public class MustBeUniqueDishNameInCategoryAttribute : CategoryBaseValidationAttribute
    {
        private readonly string propertyTitleDishName;

        public MustBeUniqueDishNameInCategoryAttribute(string propertyTitleDishName) 
            => this.propertyTitleDishName = propertyTitleDishName;

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var categoryService = validationContext.GetService<IDishCategoryService>();

            if (IsCategoryExist((int) value, categoryService))
            {
                var actualDishName = (string)validationContext
                    .ObjectInstance
                    .GetType()
                    .GetProperty(this.propertyTitleDishName)
                    .GetValue(validationContext.ObjectInstance);

                var isDishExistInCategory = IsItemExistInCategory((int)value, actualDishName, categoryService);

                if (!isDishExistInCategory)
                {
                    return ValidationResult.Success;
                }

                this.ErrorMessage = ItemAlreadyExistInCategory;

                return new ValidationResult(this.ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }
}
