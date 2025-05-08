namespace PrescribingSystem.Models.ViewModels
{
    public class ActiveIngredientViewModel
    {
        public ActiveIngredients NewActiveIngredient { get; set; } = new ActiveIngredients();
        public List<ActiveIngredients> ActiveIngredientsList { get; set; } = new List<ActiveIngredients>();
    }
}
