using System.ComponentModel.DataAnnotations;

namespace PrescribingSystem.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email is required to login.")]
        [EmailAddress]
        public string Email {  get; set; }
        [Required(ErrorMessage = "Password is required to login.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name ="Remember me")]
        public bool RememberMe { get; set; } 
    }
}
