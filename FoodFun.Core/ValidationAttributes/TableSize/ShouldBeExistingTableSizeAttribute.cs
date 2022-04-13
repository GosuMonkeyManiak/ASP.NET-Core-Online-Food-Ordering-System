namespace FoodFun.Core.ValidationAttributes.TableSize
{
    using FoodFun.Core.Contracts;
    using Microsoft.Extensions.DependencyInjection;
    using System.ComponentModel.DataAnnotations;

    using static Constants.ValidationConstants;

    [AttributeUsage(AttributeTargets.Property)]
    public class ShouldBeExistingTableSizeAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var tableSizeService = validationContext.GetService<ITableSizeService>();

            if (tableSizeService.IsExistById((int) value).GetAwaiter().GetResult())
            {
                return ValidationResult.Success;
            }

            this.ErrorMessage = TableSizeNotExist;

            return new ValidationResult(this.ErrorMessage);
        }
    }
}
