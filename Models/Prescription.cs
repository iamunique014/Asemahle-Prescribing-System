using PrescribingSystem.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrescribingSystem.Models
{
    public class Prescription
    {
        [Key]
        public int PrescriptionId { get; set; }

        [Required]
        public string CustomerId { get; set; }  // Assuming Identity User Id

        [ForeignKey(nameof(CustomerId))]
        public ApplicationUser Customer { get; set; }   // Navigation property

        [Required]
        public string DoctorName { get; set; }

        public DateTime PrescriptionDate { get; set; }

        public decimal TotalCost { get; set; }

        public string FilePath { get; set; }

        public string RawText { get; set; }

        // NEW FIELD: customer decides if pharmacist should process
        public bool ShouldProcess { get; set; } = false;

        public PrescriptionStatus PrescriptionStatus { get; set; } = PrescriptionStatus.Pending;

        // Navigation property
        public ICollection<MedicationItem> MedicationItems { get; set; } = new List<MedicationItem>();
    }
    public enum PrescriptionStatus
    {
        Pending = 0,
        Processed = 1,
        ReadyForCollection = 2,
        Collected = 3,
        Rejected = 4
    }
}
