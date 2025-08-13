using PrescribingSystem.Models;

namespace PrescribingSystem.AunthViewModels
{
    public class CustomerProfileViewModel
    {
        // Basic profile
        public string FirstName { get; set; }
        public string LastName{ get; set; }
        public string IdentityNumber { get; set; }
        

        public string Email { get; set; }

        // Allergies
        public List<int> SelectedAllergyIds { get; set; } = new();
        public List<ActiveIngredients> AvailableIngredients { get; set; } = new();
    }
}
