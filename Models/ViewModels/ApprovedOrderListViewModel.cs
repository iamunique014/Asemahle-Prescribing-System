namespace PrescribingSystem.Models.ViewModels
{
    public class ApprovedOrderListViewModel
    {
        public List<ApprovedOrder> ReceivedOrders { get; set; }
        public List<ApprovedOrder> NotReceivedOrders { get; set; }
    }
}
