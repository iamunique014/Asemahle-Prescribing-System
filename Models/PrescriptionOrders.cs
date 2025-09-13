using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrescribingSystem.Models
{
    public class PrescriptionOrders
    {
        [Key]
        public int PrescriptionOrdersId { get; set; }
        public string CustomerId { get; set; }
        [Required]
        [ForeignKey(nameof(Prescription))]
        public int PrescriptionId { get; set; }
        public Prescription Prescription { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public OrderStatus OrderStatus { get; set; }
        public IsDeleted IsDeleted { get; set; }
    }
    public enum OrderStatus
    {
        Pending,
        Aproved,
        ReadyForCollection,
        Rejected,
        Collected
    }
    public enum IsDeleted
    {
        Active,
        Deleted
    }
}
