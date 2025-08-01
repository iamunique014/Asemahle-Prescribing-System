namespace PrescribingSystem.Models.ViewModels
{
    public class GroupedMedicationViewModel
    {
        public int SupplierId { get; set; }
        public string SupplierName { get; set; }
        public List<Medication> Medications { get; set; }
    }
}
