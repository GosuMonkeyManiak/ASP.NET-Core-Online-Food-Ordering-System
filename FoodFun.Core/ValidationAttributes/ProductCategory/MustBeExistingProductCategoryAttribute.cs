namespace FoodFun.Core.ValidationAttributes.ProductCategory
{
    using System.ComponentModel.DataAnnotations;
    using Base;
    using static Constants.ValidationConstants;

    [AttributeUsage(AttributeTargets.Property)]
    public class MustBeExistingProductCategoryAttribute : ProductCategoryBaseValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var categoryService = GetProductCategoryService(validationContext);

            if (IsCategoryExist((int) value, categoryService))
            {
                return ValidationResult.Success;
            }

            this.ErrorMessage = CategoryNotExist;

            return new ValidationResult(this.ErrorMessage);
        }
    }
}
