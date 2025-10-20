namespace PrescribingSystem.Models.ViewModels
{
    public class CustomerReportViewModel
    {
        public string GroupBy { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string CustomerName { get; set; }
        public List<CustomerPrescriptionVM> Prescriptions { get; set; } = new();
    }
    public class CustomerPrescriptionVM
    {
        public int PrescriptionId { get; set; }
        public DateTime PrescriptionDate { get; set; }
        public string DoctorName { get; set; }
        public decimal TotalCost { get; set; }
        public List<CustomerMedicationVM> Medications { get; set; } = new();
    }

    public class CustomerMedicationVM
    {
        public string Name { get; set; }
        public string Dosage { get; set; }
        public int Quantity { get; set; }
        public int RemainingRepeats { get; set; }
        public int TotalRepeats { get; set; }
    }
}
