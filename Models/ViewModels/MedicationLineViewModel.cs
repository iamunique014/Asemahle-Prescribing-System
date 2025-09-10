using System.ComponentModel.DataAnnotations;

namespace PrescribingSystem.Models.ViewModels
{
    public class MedicationLineViewModel
    {
        [Required]
        [Display(Name = "Medication")]
        public int MedicationId{ get; set; }

        [Required]
        public string Dosage { get; set; }

        [Required]
        [Range(1, 1000)]
        public int Quantity { get; set; }

        [Required]
        [Range(0.01, 10000)]
        public decimal Price { get; set; }
    }
}
