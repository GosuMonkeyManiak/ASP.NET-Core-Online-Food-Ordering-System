namespace FoodFun.Core.ValidationAttributes.Dish
{
    using System.ComponentModel.DataAnnotations;
    using Contracts;
    using Microsoft.Extensions.DependencyInjection;

    using static Constants.ValidationConstants;

    public class MustBeExistingDishAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var dishService = validationContext.GetService<IDishService>();

            if (dishService.IsDishExist((string) value).GetAwaiter().GetResult())
            {
                return ValidationResult.Success;
            }

            this.ErrorMessage = DishNotExist;

            return new ValidationResult(this.ErrorMessage);
        }
    }
}
