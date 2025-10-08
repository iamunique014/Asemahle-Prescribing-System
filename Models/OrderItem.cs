namespace PrescribingSystem.Models
{
    public class OrderItem
    {
        public int OrderItemId { get; set; }

        public int OrderId { get; set; }
        public PrescriptionOrders PrescriptionOrders { get; set; }

        public int MedicationItemId { get; set; }
        public MedicationItem MedicationItem { get; set; }

        public int QuantityOrdered { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal LineTotal { get; set; }
    }
}
