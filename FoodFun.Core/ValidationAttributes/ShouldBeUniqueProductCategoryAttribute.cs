namespace FoodFun.Core.ValidationAttributes
{
    using Contracts;
    using Microsoft.Extensions.DependencyInjection;
    using System.ComponentModel.DataAnnotations;

    using static Constants.ValidationConstants;

    public class ShouldBeUniqueProductCategoryAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var productCategoryService = validationContext.GetService<IProductCategoryService>();

            if (!productCategoryService.IsCategoryExist((string) value).GetAwaiter().GetResult())
            {
                return ValidationResult.Success;
            }

            this.ErrorMessage = CategoryAlreadyExist;

            return new ValidationResult(this.ErrorMessage);
        }
    }
}
