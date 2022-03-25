namespace FoodFun.Core.ValidationAttributes
{
    using System.ComponentModel.DataAnnotations;
    using Contracts;
    using Microsoft.Extensions.DependencyInjection;

    using static Constants.ValidationConstants;

    public class ShouldBeExistingProductAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var productService = validationContext.GetService<IProductService>();

            var isProductExist = productService.IsProductExist((string) value)
                .GetAwaiter()
                .GetResult();

            if (isProductExist)
            {
                return ValidationResult.Success;
            }

            this.ErrorMessage = ProductNotExist;

            return new ValidationResult(this.ErrorMessage);
        }
    }
}
