using System.ComponentModel.DataAnnotations;

namespace PrescribingSystem.Models.ViewModels
{
    public class PrescriptionUploadViewModel
    {
        public int PrescriptionId { get; set; }
        public IFormFile? PrescriptionFile { get; set; }
        public bool ShouldProcess { get; set; } = true;
        // Existing file info (used during edit)
        public string? ExistingFilePath { get; set; }

        [Display(Name = "Prescription Date")]
        public DateTime? PrescriptionDate { get; set; }

        [Display(Name = "Status")]
        public string? PrescriptionStatus { get; set; }

        // Optional helper for UI
        public bool HasFile => !string.IsNullOrEmpty(ExistingFilePath);
    }
}
