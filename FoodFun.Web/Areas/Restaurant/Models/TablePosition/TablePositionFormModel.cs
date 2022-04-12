namespace FoodFun.Web.Areas.Restaurant.Models.TablePosition
{
    using FoodFun.Core.ValidationAttributes.TablePosition;
    using System.ComponentModel.DataAnnotations;

    using static Constants.GlobalConstants.Messages;
    using static FoodFun.Infrastructure.Common.DataConstants.TablePosition;

    public class TablePositionFormModel
    {
        [Required]
        [StringLength(
            PositionMaxLength,
            MinimumLength = PositionMinLength,
            ErrorMessage = PositionError)]
        [ShouldBeUniqueTablePosition]
        public string Position { get; init; }
    }
}
