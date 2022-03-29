namespace FoodFun.Core.ValidationAttributes.ProductCategory
{
    using System.ComponentModel.DataAnnotations;
    using Category;
    using Contracts;
    using Microsoft.Extensions.DependencyInjection;

    using static Constants.ValidationConstants;

    public class MustBeUniqueProductNameInCategoryAttribute : CategoryBaseValidationAttribute
    {
        private readonly string propertyTitleOfProductName;

        public MustBeUniqueProductNameInCategoryAttribute(string propertyTitleOfProductName) 
            => this.propertyTitleOfProductName = propertyTitleOfProductName;

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var categoryService = validationContext.GetService<IProductCategoryService>();

            if (IsCategoryExist((int) value, categoryService))
            {
                var actualProductName = (string)validationContext
                    .ObjectInstance
                    .GetType()
                    .GetProperty(this.propertyTitleOfProductName)
                    .GetValue(validationContext.ObjectInstance);

                var isProductExistInCategory = IsItemExistInCategory((int)value, actualProductName, categoryService);

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
