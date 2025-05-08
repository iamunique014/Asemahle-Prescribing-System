using System.ComponentModel.DataAnnotations;

namespace PrescribingSystem.Models
{
    public class Doctor
    {
        [Key] // Optional if using EF and this is the primary key
        public int DoctorId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(50, ErrorMessage = "Name can't be longer than 50 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Surname is required")]
        [StringLength(50, ErrorMessage = "Surname can't be longer than 50 characters")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Practice Number is required")]
        [Display(Name = "Practice Number")]
        [StringLength(20, ErrorMessage = "Practice Number can't be longer than 20 characters")]
        public string Practice_Number { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
    }
}
