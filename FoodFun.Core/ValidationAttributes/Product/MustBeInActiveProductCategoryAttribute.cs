namespace FoodFun.Core.ValidationAttributes.Product
{
    using System.ComponentModel.DataAnnotations;
    using Category;
    using Contracts;
    using Microsoft.Extensions.DependencyInjection;

    using static Constants.ValidationConstants;

    public class MustBeInActiveProductCategory : CategoryBaseValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var productCategoryService = validationContext.GetService<IProductCategoryService>();

            if (IsItemInAnyActiveCategory((string) value, productCategoryService))
            {
                return ValidationResult.Success;
            }

            this.ErrorMessage = ItemMustBeInActiveCategory;

            return new ValidationResult(this.ErrorMessage);
        }
    }
}
