namespace FoodFun.Web.ModelBinders
{
    using Microsoft.AspNetCore.Mvc.ModelBinding;

    public class DateBinderProvider : IModelBinderProvider
    {
        private readonly string dateFormat;

        public DateBinderProvider(string dateFormat)
        {
            this.dateFormat = dateFormat;
        }

        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (context.Metadata.ModelType == typeof(DateOnly))
            {
                return new DateBinder(dateFormat);
            }

            return null;
        }
    }
}
