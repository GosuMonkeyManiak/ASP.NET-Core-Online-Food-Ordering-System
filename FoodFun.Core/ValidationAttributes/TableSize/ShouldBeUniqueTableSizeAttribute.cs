namespace FoodFun.Core.ValidationAttributes.TableSize
{
    using FoodFun.Core.Contracts;
    using Microsoft.Extensions.DependencyInjection;
    using System.ComponentModel.DataAnnotations;

    using static Constants.ValidationConstants;

    public class ShouldBeUniqueTableSizeAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var tableSizeService = validationContext.GetService<ITableSizeService>();

            if (!tableSizeService.IsExist((int) value).GetAwaiter().GetResult())
            {
                return ValidationResult.Success;
            }

            this.ErrorMessage = TableSizeAlreadyExist;

            return new ValidationResult(this.ErrorMessage);
        }
    }
}
