using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrescribingSystem.Models
{
    public class PrescriptionOrders
    {
        [Key]
        public int PrescriptionOrdersId { get; set; }
        public string CustomerId { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public OrderStatus OrderStatus { get; set; }
        public decimal TotalCost { get; set; }
        public bool IsRepeatOrder { get; set; }
       // public bool EmailSent { get; set; }
        public DateTime? ReadyDate { get; set; }
        public DateTime? CollectedDate { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
    }
    public enum OrderStatus
    {
        Pending,
        Aproved,
        ReadyForCollection,
        Rejected,
        Collected
    }
}
