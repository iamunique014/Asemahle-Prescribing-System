namespace PrescribingSystem.Models.ViewModels
{
    public class DispensedPrescriptionReportVM
    {
        public string PatientName { get; set; }
        public string DoctorName { get; set; }
        public string MedicationName { get; set; }
        public int Quantity { get; set; }
        public decimal TotalCost { get; set; }
        public DateTime PrescriptionDate { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
