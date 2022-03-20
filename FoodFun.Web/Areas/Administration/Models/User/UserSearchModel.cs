namespace FoodFun.Web.Areas.Administration.Models.User
{
    using Microsoft.AspNetCore.Mvc.ModelBinding;

    public class UserSearchModel
    {
        public string SearchTerm { get; init; }

        [BindNever]
        public IEnumerable<UserListingModel> Users { get; init; }
    }
}
