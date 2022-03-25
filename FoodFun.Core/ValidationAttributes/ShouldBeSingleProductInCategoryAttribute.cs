namespace FoodFun.Core.ValidationAttributes
{
    using System.ComponentModel.DataAnnotations;
    using Contracts;
    using Microsoft.Extensions.DependencyInjection;
    using static Constants.ValidationConstants;

    [AttributeUsage(AttributeTargets.Property)]
    public class ShouldBeSingleProductInCategoryAttribute : ValidationAttribute
    {
        private readonly string productNameProperty;

        public ShouldBeSingleProductInCategoryAttribute(string productNameProperty) 
            => this.productNameProperty = productNameProperty;

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var categoryService = validationContext.GetService<IProductCategoryService>();

            var actualProductName = (string) validationContext
                .ObjectInstance
                .GetType()
                .GetProperty(productNameProperty)
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
    }
}
