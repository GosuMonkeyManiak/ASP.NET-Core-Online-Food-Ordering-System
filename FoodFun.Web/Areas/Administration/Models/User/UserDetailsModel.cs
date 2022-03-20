namespace FoodFun.Web.Areas.Administration.Models.User
{
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class UserDetailsModel
    {
        public string Id { get; init; }

        public string Username { get; init; }

        public List<SelectListItem> Roles { get; init; } = new List<SelectListItem>();
    }
}
