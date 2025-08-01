using System.ComponentModel.DataAnnotations;

namespace PrescribingSystem.Models
{
    public class ApprovedMedicationItem
    {

        [Key]
        public int Id { get; set; }

        public int ApprovedOrderId { get; set; }
        public ApprovedOrder ApprovedOrder { get; set; }

        public string MedicationName { get; set; }
        public int Quantity { get; set; }
    }
}
