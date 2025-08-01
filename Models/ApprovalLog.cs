namespace PrescribingSystem.Models
{
    public class ApprovalLog
    {
        public int ApprovalLogId { get; set; }

        public int StockOrderId { get; set; }
        public StockOrder StockOrder { get; set; }

        public string ApprovedBy { get; set; }
        public DateTime ApprovedAt { get; set; }

        public string Action { get; set; } = "Stock Order Approved";
    }
}
