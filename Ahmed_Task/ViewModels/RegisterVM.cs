using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Ahmed_Task.ViewModels
{
    public class RegisterVM
    {
        [Required(ErrorMessage = "Username is required")]
        [MinLength(3, ErrorMessage = "Username must be at least 3 characters long")]
        [Remote("CheckUniqueUserName", controller: "Account", ErrorMessage = "username is taken")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [Remote("CheckUniqueEmail", controller: "Account", ErrorMessage = "email already registered")]
        public string Email { get; set; }


        [Required(ErrorMessage = "Password is required")]
        [RegularExpression("^(?=.{6,}$)(?=.*[A-Z])(?=.*[^A-Za-z0-9]).+$",
           ErrorMessage = "Password must be at least 6 chars, contain an uppercase letter and a special character.")]
        public string Password { get; set; }


        [Required(ErrorMessage = "Confirm password is required")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [RegularExpression("^01[0,1,2,5][0-9]{8}$", ErrorMessage = "Invalid Egypt phone number")]
        public string PhoneNumber { get; set; }
    }
}
