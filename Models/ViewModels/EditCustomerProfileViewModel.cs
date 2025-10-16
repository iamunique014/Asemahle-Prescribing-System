using System.ComponentModel.DataAnnotations;

namespace PrescribingSystem.Models.ViewModels
{
    public class EditCustomerProfileViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required]
        [Phone]
        [Display(Name = "Cellphone Number")]
        [RegularExpression(@"^(?:\+27|0)[6-8][0-9]{8}$", ErrorMessage = "Please enter a valid South African phone number.")]
        public string CellphoneNumber { get; set; }
        public string Email { get; set; } 
    }
}
