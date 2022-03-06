namespace FoodFun.Web.Models.Account
{
    using System.ComponentModel.DataAnnotations;

    using static Infrastructure.Data.Common.DataConstants.User;

    public class RegisterFormModel
    {
        [Required]
        [StringLength(
            UserNameMaxLength,
            MinimumLength = UserNameMinLength,
            ErrorMessage = "{0} must be between {2} and {1} characters.")]
        public string Username { get; init; }

        [Required]
        [EmailAddress]
        public string Email { get; init; }

        [Required]
        [MinLength(
            PasswordMinLength,
            ErrorMessage = "{0} must be with minimum length of {1} characters.")]
        [DataType(DataType.Password)]
        public string Password { get; init; }

        [Required]
        [Compare(nameof(Password))]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; init; }
    }
}
