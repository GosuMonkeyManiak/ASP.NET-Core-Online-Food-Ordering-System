namespace FoodFun.Web.Areas.Identity.Models
{
    using System.ComponentModel.DataAnnotations;

    public class LoginFormModel
    {
        public string Username { get; init; }

        [DataType(DataType.Password)]
        public string Password { get; init; }
        
        [Display(Name = "Remember me")]
        public bool RememberMe { get; init; }
    }
}
