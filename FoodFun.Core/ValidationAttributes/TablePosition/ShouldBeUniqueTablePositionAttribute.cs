namespace FoodFun.Core.ValidationAttributes.TablePosition
{
    using FoodFun.Core.Contracts;
    using Microsoft.Extensions.DependencyInjection;
    using System.ComponentModel.DataAnnotations;

    using static Constants.ValidationConstants;

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter)]
    public class ShouldBeUniqueTablePositionAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var tablePostionService = validationContext.GetService<ITablePositionService>();

            if (!tablePostionService.IsExist((string) value).GetAwaiter().GetResult())
            {
                return ValidationResult.Success;
            }

            this.ErrorMessage = TablePositionAlreadyExist;

            return new ValidationResult(this.ErrorMessage);
        }
    }
}
