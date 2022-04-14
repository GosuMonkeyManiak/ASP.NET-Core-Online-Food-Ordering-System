namespace FoodFun.Web.Areas.Restaurant.Models.Table
{
    using FoodFun.Core.ValidationAttributes.TablePosition;
    using FoodFun.Web.Areas.Restaurant.Models.TablePosition;
    using FoodFun.Web.Areas.Restaurant.Models.TableSize;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using System.ComponentModel.DataAnnotations;
    using Core.ValidationAttributes.TableSize;

    public class TableFormModel
    {
        [Display(Name = "Table Sizes")]
        [ShouldBeExistingTableSize]
        public int SizeId { get; init; }

        [Display(Name = "Table Positions")]
        [ShouldBeExistingTablePosition]
        public int PositionId { get; init; }

        [BindNever]
        public IEnumerable<TableSizeListingModel> Sizes { get; set; }

        [BindNever]
        public IEnumerable<TablePositionListingModel> Positions { get; set; }
    }
}
