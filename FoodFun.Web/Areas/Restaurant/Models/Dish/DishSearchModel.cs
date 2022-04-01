namespace FoodFun.Web.Areas.Restaurant.Models.Dish
{
    using System.ComponentModel.DataAnnotations;
    using DishCategory;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Web.Models.Product;

    public class DishSearchModel
    {
        public string SearchTerm { get; init; }

        [Display(Name = "Category")]
        public int CategoryId { get; init; }

        [Display(Name = "Order")]
        public byte OrderNumber { get; init; }

        public int CurrentPageNumber { get; init; } = 1;

        [BindNever]
        public int LastPageNumber { get; init; }

        [BindNever]
        public int SelectedCategoryId { get; init; }

        [BindNever]
        public IEnumerable<DishListingModel> Dishes { get; init; } = new List<DishListingModel>();

        [BindNever]
        public IEnumerable<DishCategoryModel> Categories { get; init; } = new List<DishCategoryModel>();
    }
}
