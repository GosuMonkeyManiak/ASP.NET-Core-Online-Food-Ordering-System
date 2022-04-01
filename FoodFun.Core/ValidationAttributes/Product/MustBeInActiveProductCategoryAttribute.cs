namespace FoodFun.Core.ValidationAttributes.Product
{
    using System.ComponentModel.DataAnnotations;
    using Category;
    using Contracts;
    using Microsoft.Extensions.DependencyInjection;

    using static Constants.ValidationConstants;

    public class MustBeInActiveProductCategoryAttribute : CategoryBaseValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var productService = validationContext.GetService<IProductService>();
            var productCategory = validationContext.GetService<IProductCategoryService>();

            if (!productService.IsProductExist((string) value).GetAwaiter().GetResult())
            {
                return ValidationResult.Success;
            }

            if (IsItemInAnyActiveCategory((string) value, productCategory))
            {
                return ValidationResult.Success;
            }

            this.ErrorMessage = ItemMustBeInActiveCategory;

            return new ValidationResult(this.ErrorMessage);
        }
    }
}
