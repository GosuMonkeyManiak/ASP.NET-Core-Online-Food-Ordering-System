namespace FoodFun.Core.ValidationAttributes.DishCategory
{
    using Category;
    using Contracts;
    using Microsoft.Extensions.DependencyInjection;
    using System.ComponentModel.DataAnnotations;
    using static Constants.ValidationConstants;

    public class MustBeUniqueDishCategoryTitle : CategoryBaseValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var categoryService = validationContext.GetService<IDishCategoryService>();

            if (!IsCategoryExist((string)value, categoryService))
            {
                return ValidationResult.Success;
            }

            this.ErrorMessage = CategoryAlreadyExist;

            return new ValidationResult(this.ErrorMessage);
        }
    }
}
