namespace PrescribingSystem.Models.ViewModels
{
    public class SubmitGroupedMedicationViewModel
    {
        public int SupplierId { get; set; }
        public List<int> MedicationsIds { get; set; }
    }
}
