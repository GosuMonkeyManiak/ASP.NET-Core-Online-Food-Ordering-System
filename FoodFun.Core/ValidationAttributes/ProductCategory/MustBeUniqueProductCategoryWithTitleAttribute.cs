namespace FoodFun.Core.ValidationAttributes.ProductCategory
{
    using System.ComponentModel.DataAnnotations;
    using Base;

    using static Constants.ValidationConstants;

    public class MustBeUniqueProductCategoryWithTitleAttribute : ProductCategoryBaseValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var categoryService = GetProductCategoryService(validationContext);

            if (!IsCategoryExist((string) value, categoryService))
            {
                return ValidationResult.Success;
            }

            this.ErrorMessage = CategoryAlreadyExist;

            return new ValidationResult(this.ErrorMessage);
        }
    }
}
