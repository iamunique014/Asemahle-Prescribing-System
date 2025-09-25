namespace PrescribingSystem.Models.ViewModels
{
    public class AddCustomerAllergiesViewModel
    {
        public string UserId { get; set; }
        public int[] SelectedAllergies { get; set; } = Array.Empty<int>();
        public List<ActiveIngredientViewModel> ActiveIngredients { get; set; } = new();
    }
}
