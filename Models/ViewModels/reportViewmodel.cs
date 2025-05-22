namespace PrescribingSystem.Models.ViewModels
{
    public class reportViewmodel
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string GroupBy { get; set; }
        public string PatientName { get; set; }
        public string DoctorName { get; set; }
        public string MedicationName { get; set; }
    }
}
