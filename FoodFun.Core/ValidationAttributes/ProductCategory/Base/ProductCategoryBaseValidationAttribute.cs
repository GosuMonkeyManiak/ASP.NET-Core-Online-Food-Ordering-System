namespace FoodFun.Core.ValidationAttributes.ProductCategory.Base
{
    using System.ComponentModel.DataAnnotations;
    using Contracts;
    using Microsoft.Extensions.DependencyInjection;

    public abstract class ProductCategoryBaseValidationAttribute : ValidationAttribute
    {
        protected IProductCategoryService GetProductCategoryService(ValidationContext context)
            => context.GetService<IProductCategoryService>();

        protected bool IsCategoryExist(int categoryId, IProductCategoryService productCategoryService)
            => productCategoryService.IsCategoryExist(categoryId).GetAwaiter().GetResult();

        protected bool IsCategoryExist(string title, IProductCategoryService productCategoryService)
            => productCategoryService.IsCategoryExist(title).GetAwaiter().GetResult();
    }
}
