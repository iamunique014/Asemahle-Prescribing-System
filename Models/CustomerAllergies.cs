using PrescribingSystem.Data;
using PrescribingSystem.Models;

namespace PrescribingSystem.Models
{
    public class CustomerAllergies
    {
        public int Id { get; set; }

        // Foreign keys
        public string CustomerId { get; set; }
        public ApplicationUser Customer { get; set; }

        public int ActiveIngredientId { get; set; }
        public ActiveIngredients ActiveIngredient { get; set; }
    }
}
