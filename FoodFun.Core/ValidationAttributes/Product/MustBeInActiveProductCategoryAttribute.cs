namespace FoodFun.Core.ValidationAttributes.Product
{
    using System.ComponentModel.DataAnnotations;
    using Category;
    using Contracts;
    using Microsoft.Extensions.DependencyInjection;

    using static Constants.ValidationConstants;

    public class MustBeInActiveDishCategory : CategoryBaseValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var dishCategoryService = validationContext.GetService<IDishCategoryService>();

            if (IsItemInAnyActiveCategory((string) value, dishCategoryService))
            {
                return ValidationResult.Success;
            }

            this.ErrorMessage = ItemMustBeInActiveCategory;

            return new ValidationResult(this.ErrorMessage);
        }
    }
}
