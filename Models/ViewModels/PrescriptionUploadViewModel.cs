using System.ComponentModel.DataAnnotations;

namespace PrescribingSystem.Models.ViewModels
{
    public class PrescriptionUploadViewModel
    {
        [Required]
        [Display(Name = "Doctor Name")]
        public string DoctorName { get; set; }

        [Required]
        [Range(1, 10)]
        public int TotalRepeats { get; set; } = 1;

        [Required]
        public IFormFile PrescriptionFile { get; set; }

        public List<MedicationLineViewModel> Medications { get; set; } = new List<MedicationLineViewModel>();
    }
}
