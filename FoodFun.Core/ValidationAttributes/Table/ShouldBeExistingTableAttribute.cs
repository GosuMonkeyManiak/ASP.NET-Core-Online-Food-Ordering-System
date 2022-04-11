namespace FoodFun.Core.ValidationAttributes.Table
{
    using FoodFun.Core.Contracts;
    using Microsoft.Extensions.DependencyInjection;
    using System.ComponentModel.DataAnnotations;

    using static Constants.ValidationConstants;

    public class ShouldBeExistingTableAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var tableService = validationContext.GetService<ITableService>();

            if (tableService.IsTableExist((string) value).GetAwaiter().GetResult())
            {
                return ValidationResult.Success;
            }

            this.ErrorMessage = TableNotExist;

            return new ValidationResult(this.ErrorMessage);
        }
    }
}
