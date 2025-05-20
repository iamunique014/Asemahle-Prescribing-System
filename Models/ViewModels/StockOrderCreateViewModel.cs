using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrescribingSystem.Models.ViewModels
{
    public class StockOrderCreateViewModel
    {
        
        public int StockOrderId { get; set; }
        [Display(Name = "Order Number")]
        public string OrderNumber { get; set; }
        public List<Medication> Medications { get; set; } = new();

        // List of selected medication IDs
        public List<int> SelectedMedicationIds { get; set; } = new();

        // Quantities keyed by medication ID
        public Dictionary<int, int> Quantity { get; set; } = new();
        //public List<int> ExistingStockIds { get; set; } // IDs that should be excluded
        //public List<SelectListItem> AvailableStocks { get; set; }
        //public int? SelectedStockId { get; set; } // selected item



    }
}
