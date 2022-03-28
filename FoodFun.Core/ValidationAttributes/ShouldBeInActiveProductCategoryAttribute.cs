namespace FoodFun.Core.ValidationAttributes
{
    using System.ComponentModel.DataAnnotations;
    using Contracts;
    using Microsoft.Extensions.DependencyInjection;

    using static Constants.ValidationConstants;

    public class ShouldBeInActiveProductCategoryAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var productService = validationContext.GetService<IProductService>();
            var productCategoryService = validationContext.GetService<IProductCategoryService>();

            if (!productService.IsProductExist((string) value).GetAwaiter().GetResult())
            {
                this.ErrorMessage = ProductNotExist;

                return new ValidationResult(this.ErrorMessage);
            }

            var product = productService.GetByIdOrDefault((string) value).GetAwaiter().GetResult();

            if (productCategoryService.IsCategoryActive(product.Category.Id).GetAwaiter().GetResult())
            {
                return ValidationResult.Success;
            }

            this.ErrorMessage = ProductNotExist;

            return new ValidationResult(this.ErrorMessage);
;        }
    }
}
