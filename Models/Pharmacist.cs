using System.ComponentModel.DataAnnotations;

namespace PrescribingSystem.Models
{
    public class Pharmacist
    {
        [Key]
        public int PharmacistId { get; set; }

        [Required]
        [RegularExpression("^[A-Z][a-zA-Z]*$", ErrorMessage = "Name must start with a capital letter and contain only letters.")]
        public string Name { get; set; }

        [Required]
        [RegularExpression("^[A-Z][a-zA-Z]*$", ErrorMessage = "Surname must start with a capital letter and contain only letters.")]
        public string Surname { get; set; }

        [Required]
        [Phone]
        [RegularExpression(@"^\+?[0-9]{10,15}$", ErrorMessage = "Enter a valid phone number (10–15 digits, optional +).")]
        public string Cellphone { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

  
        [Required]
        public string HealthCouncil { get; set; }

        [Required]
        [RegularExpression("^[A-Z0-9-]+$", ErrorMessage = "Registration number must be uppercase alphanumeric with optional dashes.")]
        public string RegistrationNumber { get; set; }

    }
}
