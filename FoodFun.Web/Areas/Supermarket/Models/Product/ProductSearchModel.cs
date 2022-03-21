namespace FoodFun.Web.Areas.Supermarket.Models.Product
{
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using ProductCategory;

    public class ProductSearchModel
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
        public IEnumerable<ProductListingModel> Products { get; init; } = new List<ProductListingModel>();

        [BindNever]
        public IEnumerable<ProductCategoryModel> Categories { get; init; } = new List<ProductCategoryModel>();
    }
}
