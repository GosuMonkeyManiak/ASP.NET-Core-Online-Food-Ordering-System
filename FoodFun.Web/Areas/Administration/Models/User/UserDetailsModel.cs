namespace FoodFun.Web.Areas.Administration.Models.User
{
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Mvc.Rendering;

    using static Infrastructure.Common.DataConstants.User;
    using static Constants.GlobalConstants.Messages;

    public class UserDetailsModel
    {
        public string Id { get; init; }
        
        [Required]
        [StringLength(
            UserNameMaxLength,
            MinimumLength = UserNameMinLength,
            ErrorMessage = UsernameError)]
        public string Username { get; init; }

        public List<SelectListItem> Roles { get; init; } = new List<SelectListItem>();
    }
}
