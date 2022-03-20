namespace FoodFun.Web.Areas.Administration.Models.Role
{
    using System.ComponentModel.DataAnnotations;

    using static Infrastructure.Common.DataConstants.Role;
    using static Constants.GlobalConstants.Messages;

    public class RoleFormModel
    {
        [Required]
        [StringLength(
            TitleMaxLength,
            MinimumLength = TitleMinLength,
            ErrorMessage = RoleTitleError)]
        public string Title { get; init; }
    }
}
