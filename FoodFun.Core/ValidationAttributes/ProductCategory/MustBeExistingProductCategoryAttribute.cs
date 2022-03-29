namespace FoodFun.Core.ValidationAttributes.ProductCategory
{
    using System.ComponentModel.DataAnnotations;
    using Category;
    using Contracts;
    using Microsoft.Extensions.DependencyInjection;

    using static Constants.ValidationConstants;

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter)]
    public class MustBeExistingProductCategoryAttribute : CategoryBaseValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var categoryService = validationContext.GetService<IProductCategoryService>();

            if (IsCategoryExist((int) value, categoryService))
            {
                return ValidationResult.Success;
            }

            this.ErrorMessage = CategoryNotExist;

            return new ValidationResult(this.ErrorMessage);
        }
    }
}
