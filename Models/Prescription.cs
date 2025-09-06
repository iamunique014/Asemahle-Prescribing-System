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

        public DateTime DateIssued { get; set; }

        public int TotalRepeats { get; set; }

        public int RemainingRepeats { get; set; }

        public decimal TotalCost { get; set; }

        public string FilePath { get; set; }

        public string PrescriptionStatus { get; set; }

        // Navigation property
        public ICollection<MedicationItem> MedicationItems { get; set; } = new List<MedicationItem>();
    }
}
