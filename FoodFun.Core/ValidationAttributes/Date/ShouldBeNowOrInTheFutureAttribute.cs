namespace FoodFun.Core.ValidationAttributes.Date
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using static Constants.ValidationConstants;

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter)]
    public class ShouldBeNowOrInTheFutureAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var date = ((DateOnly)value).ToDateTime(TimeOnly.MaxValue);

            if (date < DateTime.Now)
            {
                this.ErrorMessage = DateMustBeNowOrInTheFuture;

                return new ValidationResult(this.ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }
}
