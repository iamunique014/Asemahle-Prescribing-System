using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace PrescribingSystem.Models.ViewModels
{
    public class AddMedicationStockViewModel
    {
        public List<Medication> Medications { get; set; } = new List<Medication>();

        [Display(Name = "Select Medications")]
        public List<int> SelectedMedicationIds { get; set; } = new List<int>();

        [Display(Name = "Supplier")]
        public SelectList Suppliers { get; set; }

        public int? SelectedSupplierId { get; set; }

        // ✅ This dictionary binds to Quantity[MedicationId] in the form
        public Dictionary<int, int> Quantity { get; set; } = new Dictionary<int, int>();

        // ✅ Optional: match with hidden input field from the view
        //public string OrderNumber { get; set; }
    }
}
