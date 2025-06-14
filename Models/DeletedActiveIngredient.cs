using System.ComponentModel.DataAnnotations;

namespace PrescribingSystem.Models
{
    public class DeletedActiveIngredient
    {
        [Key]
        public int DeletedActiveIngredientId { get; set; }

        public int OriginalActiveIngredientId { get; set; }

        [Required]
        [Display(Name = "Active Ingredient Name")]
        public string ActiveIngredientName { get; set; }

        public DateTime DeletedAt { get; set; } = DateTime.UtcNow;
    }
}
