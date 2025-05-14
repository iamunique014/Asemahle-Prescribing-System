using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PrescribingSystem.Models
{
    public class MedicationStockOrder
    {
        [Key]
        public int MedicationStockOrderId { get; set; }

        [Required]
        [ForeignKey(nameof(StockOrder))]
        public int StockOrderId { get; set; }

        [Required]
        [Display(Name = "Medication")]
        [ForeignKey("Medication")]
        public int MedicationId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than zero.")]
        public int Quantity { get; set; }

        // Navigation properties
        public StockOrder StockOrder { get; set; }
        public Medication Medication { get; set; }
    }
}
