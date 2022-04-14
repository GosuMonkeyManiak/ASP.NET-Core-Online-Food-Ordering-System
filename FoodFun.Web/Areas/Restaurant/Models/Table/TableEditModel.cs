namespace FoodFun.Web.Areas.Restaurant.Models.Table
{
    using FoodFun.Core.ValidationAttributes.Table;
    using FoodFun.Core.ValidationAttributes.TablePosition;
    using FoodFun.Core.ValidationAttributes.TableSize;
    using FoodFun.Web.Areas.Restaurant.Models.TablePosition;
    using FoodFun.Web.Areas.Restaurant.Models.TableSize;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using System.ComponentModel.DataAnnotations;

    public class TableEditModel
    {
        [ShouldBeExistingTable]
        public string Id { get; init; }

        [Display(Name = "Positions")]
        [ShouldBeExistingTablePosition]
        public int PositionId { get; init; }

        [Display(Name = "Seats")]
        [ShouldBeExistingTableSize]
        public int SizeId { get; init; }

        [BindNever]
        public string Position { get; init; }

        [BindNever]
        public int Seats { get; init; }

        [BindNever]
        public IEnumerable<TablePositionListingModel> AllPositions { get; set; }

        [BindNever]
        public IEnumerable<TableSizeListingModel> AllSeats { get; set; }
    }
}
