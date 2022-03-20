namespace FoodFun.Web.Areas.Admin.Models
{
    using System.ComponentModel.DataAnnotations;

    public class LoginFormModel
    {
        [Required]
        public string Username { get; init; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; init; }
        
        [Display(Name = "Remember me")]
        public bool RememberMe { get; init; }
    }
}
