﻿namespace FoodFun.Web.Models.Account
{
    using System.ComponentModel.DataAnnotations;
    using Constants;

    using static Infrastructure.Data.Common.DataConstants.User;

    public class RegisterFormModel
    {
        [Required]
        [StringLength(
            UserNameMaxLength,
            MinimumLength = UserNameMinLength,
            ErrorMessage = GlobalConstants.Messages.UsernameError)]
        public string Username { get; init; }

        [Required]
        [EmailAddress]
        public string Email { get; init; }

        [Required]
        [MinLength(
            PasswordMinLength,
            ErrorMessage = GlobalConstants.Messages.PasswordError)]
        [DataType(DataType.Password)]
        public string Password { get; init; }

        [Required]
        [Compare(nameof(Password))]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; init; }
    }
}
