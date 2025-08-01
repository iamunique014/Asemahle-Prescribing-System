namespace PrescribingSystem.Models
{
    public class DispensedNotification
    {
        public int Id { get; set; }
        public int PrescriptionId { get; set; }
        public string MedicationName { get; set; }
        public int Quantity { get; set; }
        public DateTime DispensedAt { get; set; }
        public bool IsRead { get; set; }
    }
}
