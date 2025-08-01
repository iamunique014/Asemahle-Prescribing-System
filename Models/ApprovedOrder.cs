using System.ComponentModel.DataAnnotations;

namespace PrescribingSystem.Models
{
    public class ApprovedOrder
    {

        [Key]
        public int ApprovedOrderId { get; set; }
        public string OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime ApprovedDate { get; set; }
        public DateTime ApprovedAt { get; set; }
        public DateTime ReceivedDate { get; set; }
        public bool IsReceived { get; set; } = false; // ✅ new property

        public DateTime? ReceivedAt { get; set; } // ✅ optional timestamp
        // You can add navigation if you want:
        public ICollection<ApprovedMedicationItem> MedicationItems { get; set; }
    }
}
