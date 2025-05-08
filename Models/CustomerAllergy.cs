using PrescribingSystem.Migrations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PrescribingSystem.Models
{
    public class CustomerAllergy
    {
        [Key]
        public int CustomerAllergyId { get; set; }

        [Required]
        [Display(Name = "Active Ingredient")]
        public int ActiveIngredientId { get; set; }

        [Required]
        [Display(Name = "Customer")]
        public int CustomerId { get; set; }

        // Navigation
        [ForeignKey("ActiveIngredientId")]
        public virtual ActiveIngredients ActiveIngredients { get; set; }

        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; }
    }
}
