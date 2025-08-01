using System.ComponentModel.DataAnnotations;

namespace PrescribingSystem.Models
{
    public class DeletedDoctor
    {

        [Key]
        public int DeletedDoctorId { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string Practice_Number { get; set; }

        public string Email { get; set; }

        public DateTime DeletedAt { get; set; } = DateTime.Now;
    }
}
