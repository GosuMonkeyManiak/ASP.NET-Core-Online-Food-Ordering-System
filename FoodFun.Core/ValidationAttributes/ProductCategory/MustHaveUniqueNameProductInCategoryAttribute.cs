namespace FoodFun.Core.ValidationAttributes.ProductCategory
{
    using System.ComponentModel.DataAnnotations;
    using Base;

    using static Constants.ValidationConstants;

    public class MustHaveUniqueNameProductInCategoryAttribute : ProductCategoryBaseValidationAttribute
    {
        private readonly string propertyTitleOfProductName;

        public MustHaveUniqueNameProductInCategoryAttribute(string propertyTitleOfProductName) 
            => this.propertyTitleOfProductName = propertyTitleOfProductName;

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var categoryService = GetProductCategoryService(validationContext);

            if (IsCategoryExist((int) value, categoryService))
            {
                var actualProductName = (string)validationContext
                    .ObjectInstance
                    .GetType()
                    .GetProperty(this.propertyTitleOfProductName)
                    .GetValue(validationContext.ObjectInstance);

                var isProductExistInCategory = categoryService.IsProductExistInCategory((int)value, actualProductName)
                    .GetAwaiter()
                    .GetResult();

                if (!isProductExistInCategory)
                {
                    return ValidationResult.Success;
                }

                this.ErrorMessage = ItemAlreadyExistInCategory;

                return new ValidationResult(this.ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }
}
