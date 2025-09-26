using Microsoft.AspNetCore.Identity;
using PrescribingSystem.Models;

namespace PrescribingSystem.Data
{
    public class ApplicationUser:IdentityUser
    { 
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string? HealthCouncilRegistrationNumber { get; set; }

        public string IdentityNumber { get; set; }
        public ICollection<UserAllergy> Allergies { get; set; }

        // Link to prescriptions
        public ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();
    }
}
