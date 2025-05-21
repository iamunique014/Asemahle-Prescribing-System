using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PrescribingSystem.Models
{
    public class StockOrder
    {
        [Key]
        public int StockOrderId { get; set; }

        [Required]
        [Display(Name = "Order Number")]
        public string OrderNumber { get; set; } = $"ORD-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";

        [Required]
        [Display(Name = "Supplier")]
        [ForeignKey("SupplierId")]
        public int SupplierId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Order Date")]
        public DateTime OrderDate { get; set; } = DateTime.Now;

        [Required]
        [Display(Name = "Order Status")]
        public bool Status { get; set; } = false; // false = pending, true = completed

        // Navigation properties
        public Supplier Supplier { get; set; }

        [Display(Name = "Ordered Items")]
        public ICollection<MedicationStockOrder> MedicationStockOrder { get; set; }

    }
}
