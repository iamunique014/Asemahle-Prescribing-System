using System.ComponentModel.DataAnnotations;

namespace PrescribingSystem.Models.ViewModels
{
    public class PrescriptionUploadViewModel
    {

        [Required]
        public IFormFile PrescriptionFile { get; set; }

    }
}
