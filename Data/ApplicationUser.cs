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

        public ICollection<CustomerAllergies> CustomerAllergies { get; set; } = new List<CustomerAllergies>();
    }
}
