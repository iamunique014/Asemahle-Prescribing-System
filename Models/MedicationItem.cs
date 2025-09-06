using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrescribingSystem.Models
{
    public class MedicationItem
    {
        [Key]
        public int MedicationItemId { get; set; }

        [Required]
        [ForeignKey(nameof(Prescription))]
        public int PrescriptionId { get; set; }
        public Prescription Prescription { get; set; }

        [Required]
        [ForeignKey(nameof(Medication))]
        public int MedicationId { get; set; }
        public Medication Medication { get; set; }

        // Prescription-specific fields
        public string Dosage { get; set; }  // Can override catalog dosage
        public int Quantity { get; set; }
        public decimal Price { get; set; } // Price at time of prescription
    }
}
