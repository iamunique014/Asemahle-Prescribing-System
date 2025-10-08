namespace PrescribingSystem.Models.ViewModels
{
    public class AvailableMedicationViewModel
    {
        public int MedicationItemId { get; set; }
        public string MedicationName { get; set; }
        public string DoctorName { get; set; }
        public int Quantity { get; set; }
        public int RemainingRepeats { get; set; }
        public int PrescriptionId { get; set; }
        public bool Selected { get; set; } // for checkboxes
    }
}
