namespace FoodFun.Core.ValidationAttributes.TablePosition
{
    using FoodFun.Core.Contracts;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.ComponentModel.DataAnnotations;

    using static Constants.ValidationConstants;

    [AttributeUsage(AttributeTargets.Property)]
    public class ShouldBeExistingTablePositionAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var tablePositionService = validationContext.GetService<ITablePositionService>();

            if (tablePositionService.IsExist((int) value).GetAwaiter().GetResult())
            {
                return ValidationResult.Success;
            }

            this.ErrorMessage = TablePostionNotExist;

            return new ValidationResult(this.ErrorMessage);
        }
    }
}
