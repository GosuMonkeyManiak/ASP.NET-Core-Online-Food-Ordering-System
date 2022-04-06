namespace FoodFun.Core.ValidationAttributes.Dish
{
    using FoodFun.Core.Contracts;
    using FoodFun.Core.ValidationAttributes.Category;
    using Microsoft.Extensions.DependencyInjection;
    using System.ComponentModel.DataAnnotations;

    using static Core.Constants.ValidationConstants;

    public class MustBeInActiveDishCategoryAttribute : CategoryBaseValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var dishCategoryService = validationContext.GetService<IDishCategoryService>();

            if (IsItemInAnyActiveCategory((string)value, dishCategoryService))
            {
                return ValidationResult.Success;
            }

            this.ErrorMessage = ItemMustBeInActiveCategory;

            return new ValidationResult(this.ErrorMessage);
        }
    }
}
