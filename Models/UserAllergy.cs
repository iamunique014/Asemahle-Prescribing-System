using PrescribingSystem.Data;

namespace PrescribingSystem.Models
{
    public class UserAllergy
    {
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public int ActiveIngredientId { get; set; }
        public ActiveIngredients ActiveIngredient { get; set; }
    }

}
