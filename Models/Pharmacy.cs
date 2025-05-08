using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrescribingSystem.Models
{
    public class Pharmacy
    {
        [Key]
        public int PharmacyId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Health Council Registration Number")]
        public string HealthCouncilRegistrationNumber { get; set; }

        [Required]
        [Display(Name = "Physical Address 1")]
        public string PhysicalAddress1 { get; set; }

        [Display(Name = "Physical Address 2 (Optional)")]
        public string? PhysicalAddress2 { get; set; } // Optional second address

        [Required]
        [Phone]
        [Display(Name = "Contact Number")]
        public string ContactNumber { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Url(ErrorMessage = "Please enter a valid URL.")]
        [Display(Name = "Website URL")]
        public string? WebsiteUrl { get; set; } // Make it nullable or optional


        [Required]
        [Display(Name = "Responsible Pharmacist")]
        [ForeignKey("PharmacistId")]
        public int PharmacistId { get; set; }

        // Navigation Property for the related Pharmacist
        public virtual Pharmacist Pharmacist { get; set; }
    }
}
