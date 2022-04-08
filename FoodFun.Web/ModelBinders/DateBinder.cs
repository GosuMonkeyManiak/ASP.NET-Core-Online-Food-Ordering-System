namespace FoodFun.Web.ModelBinders
{
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using System.Globalization;
    using System.Threading.Tasks;

    public class DateBinder : IModelBinder
    {
        private readonly string dateFormat;

        public DateBinder(string dateFormat)
        {
            this.dateFormat = dateFormat;
            //(dd/MM/yyyy)
        }

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var modelName = bindingContext.ModelName;

            ValueProviderResult valueResult = bindingContext
                .ValueProvider
                .GetValue(modelName);

            var isSucceed = false;

            if (valueResult != ValueProviderResult.None && !string.IsNullOrWhiteSpace(valueResult.FirstValue))
            {
                isSucceed = DateOnly.TryParseExact(
                    valueResult.FirstValue,
                    this.dateFormat,
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out DateOnly result);

                if (isSucceed)
                {
                    bindingContext.Result = ModelBindingResult.Success(result);
                }
            }

            if (!isSucceed)
            {
                bindingContext.ModelState.AddModelError(modelName, $"Date should be in format {dateFormat}!");
            }

            return Task.CompletedTask;
        }
    }
}
