namespace FoodFun.Core.ValidationAttributes.Category
{
    using System.ComponentModel.DataAnnotations;
    using Contracts;

    public abstract class CategoryBaseValidationAttribute : ValidationAttribute
    {
        protected bool IsCategoryExist(int categoryId, ICategory categoryService)
            => categoryService.IsCategoryExist(categoryId).GetAwaiter().GetResult();

        protected bool IsCategoryExist(string title, ICategory categoryService)
            => categoryService.IsCategoryExist(title).GetAwaiter().GetResult();

        protected bool IsItemExistInCategory(int categoryId, string itemName, ICategory categoryService)
            => categoryService.IsItemExistInCategory(categoryId, itemName).GetAwaiter().GetResult();

        protected bool IsItemInAnyActiveCategory(string itemId, ICategory categoryService)
            => categoryService.IsItemInAnyActiveCategory(itemId).GetAwaiter().GetResult();
    }
}
