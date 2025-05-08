using System.ComponentModel.DataAnnotations;

namespace PrescribingSystem.Models
{
    public class DorsageForm
    {
        public int DorsageFormId { get; set; }

        [Required(ErrorMessage = "Dorsage form name is required.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Dorsage form name must be between 2 and 50 characters.")]
        [RegularExpression(@"^[A-Za-z\s\-]+$", ErrorMessage = "Dorsage form name can only contain letters, spaces, and hyphens.")]
        public string DorsageFormName { get; set; }
    }
}
