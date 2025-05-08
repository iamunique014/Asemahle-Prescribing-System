using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrescribingSystem.Models.ViewModels
{
    public class MedicationViewModel
    {
        [Required]
        [Display(Name = "Medication Name")]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Schedule (0–6)")]
        [Range(0, 6)]
        public int Schedule { get; set; }

        [Required]
        [Display(Name = "Dosage Form")]
        public int DorsageFormId { get; set; }

        [Required]
        [Display(Name = "Current Sales Price (R)")]
        [DataType(DataType.Currency)]
        [Range(0, 100000)]
        public decimal CurrentSalesPrice { get; set; }

        [Required]
        [Display(Name = "Supplier")]
        public int SupplierId { get; set; }

        [Required]
        [Display(Name = "Re-order Level (Units)")]
        [Range(0, 100000)]
        public int ReOrderLevel { get; set; }

        [Required]
        [Display(Name = "Quantity on Hand (Units)")]
        [Range(0, 1000000)]
        public int QuantityOnHand { get; set; }

        [Display(Name = "Is Active?")]
        public bool Status { get; set; }
        
       
    }
}
