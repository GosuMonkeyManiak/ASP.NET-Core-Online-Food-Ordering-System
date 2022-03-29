namespace FoodFun.Core.ValidationAttributes.ProductCategory
{
    using System.ComponentModel.DataAnnotations;
    using Category;
    using Contracts;
    using Microsoft.Extensions.DependencyInjection;

    using static Constants.ValidationConstants;

    public class MustBeUniqueProductCategoryTitleAttribute : CategoryBaseValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var categoryService = validationContext.GetService<IProductCategoryService>();

            if (!IsCategoryExist((string) value, categoryService))
            {
                return ValidationResult.Success;
            }

            this.ErrorMessage = CategoryAlreadyExist;

            return new ValidationResult(this.ErrorMessage);
        }
    }
}
