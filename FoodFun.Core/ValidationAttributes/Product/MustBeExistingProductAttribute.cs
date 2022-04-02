namespace FoodFun.Core.ValidationAttributes.Product
{
    using System.ComponentModel.DataAnnotations;
    using Contracts;
    using Microsoft.Extensions.DependencyInjection;

    using static Constants.ValidationConstants;

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter)]
    public class MustBeExistingProductAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var productService = validationContext.GetService<IProductService>();

            if (productService.IsProductExist((string) value).GetAwaiter().GetResult())
            {
                return ValidationResult.Success;
            }

            this.ErrorMessage = ProductNotExist;

            return new ValidationResult(this.ErrorMessage);
        }
    }
}
