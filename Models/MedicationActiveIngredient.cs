using System.ComponentModel.DataAnnotations;

namespace PrescribingSystem.Models
{
    public class MedicationActiveIngredient
    {
        [Key]
        public int MedicationActiveIngredientId { get; set; }

        public int MedicationId { get; set; }
        public Medication Medication { get; set; }

        public int ActiveIngredientId { get; set; }
        public ActiveIngredients ActiveIngredient { get; set; }
        public decimal Strength { get; set; }
    }
}
