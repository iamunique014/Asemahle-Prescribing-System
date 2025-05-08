using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace PrescribingSystem.Models
{
    public class Medication
    {
        [Key]
        public int MedicationId { get; set; }

        [Required]
        [Display(Name = "Medication Name")]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [Range(0, 6)]
        public int Schedule { get; set; }

        [Required]
        [Display(Name = "Dosage Form")]
        public int DorsageFormId { get; set; }

        [ForeignKey(nameof(DorsageFormId))]
        public DorsageForm DorsageForm { get; set; }
        

        [Required]
        [DataType(DataType.Currency)]
        public decimal CurrentSalesPrice { get; set; }

        [Required]
        public int SupplierId { get; set; }

        [ForeignKey(nameof(SupplierId))]
       public Supplier Supplier { get; set; }

        [Required]
        [Range(0, 100000)]
        public int ReOrderLevel { get; set; }

        [Required]
        [Range(0, 1000000)]
        public int QuantityOnHand { get; set; }

        public bool Status { get; set; }
    }
}
