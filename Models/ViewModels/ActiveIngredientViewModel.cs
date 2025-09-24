namespace PrescribingSystem.Models.ViewModels
{
    public class ActiveIngredientViewModel
    {
        public int id { get; set; }
        public string Name { get; set; }

        public ActiveIngredients NewActiveIngredient { get; set; } = new ActiveIngredients();
        public List<ActiveIngredients> ActiveIngredientsList { get; set; } = new List<ActiveIngredients>();
    }
}
