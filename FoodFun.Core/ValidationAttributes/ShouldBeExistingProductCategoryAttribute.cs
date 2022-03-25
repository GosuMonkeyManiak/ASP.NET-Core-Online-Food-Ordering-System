namespace FoodFun.Core.ValidationAttributes
{
    using System.ComponentModel.DataAnnotations;
    using Contracts;

    using static Constants.ValidationConstants;

    [AttributeUsage(AttributeTargets.Property)]
    public class ShouldBeExistingProductCategoryAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var categoryService = (IProductCategoryService) validationContext.GetService(typeof(IProductCategoryService));

            var isCategoryExist = categoryService.IsCategoryExist((int) value).GetAwaiter().GetResult();

            if (isCategoryExist)
            {
                return ValidationResult.Success;
            }

            this.ErrorMessage = CategoryNotExist;

            return new ValidationResult(this.ErrorMessage);
        }
    }
}
