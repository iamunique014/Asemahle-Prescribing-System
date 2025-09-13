using System.ComponentModel.DataAnnotations;

namespace PrescribingSystem.Models
{
    public class Prescription
    {
        [Key]
        public int PrescriptionId { get; set; }

        [Required]
        public string CustomerId { get; set; }  // Assuming Identity User Id

        [Required]
        public string DoctorName { get; set; }

        public DateTime PrescriptionDate { get; set; }

        public decimal TotalCost { get; set; }

        public string FilePath { get; set; }

        public string RawText { get; set; }

        public PrescriptionStatus PrescriptionStatus { get; set; } = PrescriptionStatus.Pending;

        // Navigation property
        public ICollection<MedicationItem> MedicationItems { get; set; } = new List<MedicationItem>();
    }
    public enum PrescriptionStatus
    {
        Pending = 0,
        Approved = 1,
        ReadyForCollection = 2,
        Collected = 3,
        Rejected = 4
    }
}
