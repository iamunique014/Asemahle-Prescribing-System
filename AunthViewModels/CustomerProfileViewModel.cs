using Microsoft.AspNetCore.Mvc.Rendering;

public class CustomerProfileViewModel
{
    public string Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string IdentityNumber { get; set; }
    public string Email { get; set; }

    public List<int> SelectedAllergyIds { get; set; } = new List<int>();
    public List<SelectListItem> AvailableAllergies { get; set; } = new List<SelectListItem>();
}
