using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrescribingSystem.Models.ViewModels
{
    public class StockOrderCreateViewModel
    {
        public string OrderNumber { get; set; }

        public List<Medication> Medications { get; set; }

        public List<int> SelectedMedicationIds { get; set; }
        public int SupplierId { get; set; }
        public Supplier Supplier { get; set; }

        public Dictionary<int, int> Quantity { get; set; } = new();


        //public List<int> ExistingStockIds { get; set; } // IDs that should be excluded
        //public List<SelectListItem> AvailableStocks { get; set; }
        //public int? SelectedStockId { get; set; } // selected item



    }
}
