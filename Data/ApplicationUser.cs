using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PrescribingSystem.Models;
using System.ComponentModel.DataAnnotations;

namespace PrescribingSystem.Data
{
    [Index(nameof(IdentityNumber), IsUnique = true)]
    public class ApplicationUser:IdentityUser
    { 
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string? HealthCouncilRegistrationNumber { get; set; }

        [Required]
        [StringLength(13, MinimumLength = 13, ErrorMessage = "SA ID Number must be exactly 13 digits.")]
        [RegularExpression(@"^\d{13}$", ErrorMessage = "SA ID Number must contain only digits.")]
        [Display(Name = "Identity Number")]
        public string IdentityNumber { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Cellphone Number")]
        [RegularExpression(@"^(?:\+27|0)[6-8][0-9]{8}$", ErrorMessage = "Please enter a valid South African phone number.")]
        public string CellphoneNumber { get; set; }
        public string? Address { get; set; }
        public ICollection<UserAllergy> Allergies { get; set; }

        // Link to prescriptions
        public ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();
    }
}
