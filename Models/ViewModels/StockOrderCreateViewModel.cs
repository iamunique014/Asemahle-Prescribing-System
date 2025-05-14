using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrescribingSystem.Models.ViewModels
{
    public class StockOrderCreateViewModel
    {
        [Required]
        public int StockOrderId { get; set; }

        [Required(ErrorMessage = "Please select at least one medication.")]
        [Display(Name = "Medications")]
        public List<int> SelectedMedicationIds { get; set; } = new List<int>();

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than zero.")]
        public int Quantity { get; set; }
    }
}
