using System.ComponentModel.DataAnnotations;

namespace CustomerManagementHub.DataAccess.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Username is required.")]
        public string? UserName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is required.")]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        [DataType(DataType.Password)]
        public string? ConfirmPassword { get; set; }

        [Required(ErrorMessage = "User role is required.")]
        public string? UserRole { get; set; }

        public string? AdminUsername { get; set; }
        public string? AdminPassword { get; set; }
    }
}
