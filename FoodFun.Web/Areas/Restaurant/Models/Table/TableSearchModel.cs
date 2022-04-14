namespace FoodFun.Web.Areas.Restaurant.Models.Table
{
    using Microsoft.AspNetCore.Mvc.ModelBinding;

    public class TableSearchModel
    {
        public string SearchTerm { get; init; }

        [BindNever]
        public IEnumerable<TableListingModel> Tables { get; set; }
    }
}
