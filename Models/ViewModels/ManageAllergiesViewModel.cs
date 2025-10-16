using Microsoft.AspNetCore.Mvc.Rendering;

namespace PrescribingSystem.Models.ViewModels
{
    public class ManageAllergiesViewModel
    {
        public List<int> SelectedAllergyIds { get; set; } = new();
        public List<SelectListItem> AvailableAllergies { get; set; } = new();
    }
}
