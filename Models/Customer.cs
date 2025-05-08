using System.ComponentModel.DataAnnotations;

namespace PrescribingSystem.Models
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [StringLength(50)]
        public string Surname { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Cellphone Number")]
        public string CellphoneNumber { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Visit Date")]
        public DateTime VisitDate { get; set; }
        // Navigation
        public ICollection<CustomerAllergy> CustomerAllergies { get; set; }

        internal static int FirstOrDefault(Func<object, bool> value)
        {
            throw new NotImplementedException();
        }
    }
}
