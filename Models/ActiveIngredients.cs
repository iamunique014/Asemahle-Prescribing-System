using System.ComponentModel.DataAnnotations;

namespace PrescribingSystem.Models
{
    public class ActiveIngredients
    {
        [Key]
        public int ActiveIngredientId { get; set; }

        [Display(Name = "Active Ingredient Name")]
        [Required(ErrorMessage = "Active Ingredient Name is required")]
        public string? ActiveIngredientName { get; set; }

        // Navigation
        //public ICollection<CustomerAllergy> CustomerAllergies { get; set; } = new List<CustomerAllergy>();\
        public ICollection<UserAllergy> UserAllergies { get; set; }
        public ICollection<Medication> Medications { get; set; } = new List<Medication>(); // Optional reverse navigation
    }


}
