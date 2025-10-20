using PrescribingSystem.Data;
using PrescribingSystem.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using iTextSharp.text;
using iTextSharp.text.pdf;
using PrescribingSystem.Models;

namespace PrescribingSystem.Services
{
    public class CustomerReportService
    {
        private readonly ApplicationDbContext _context;

        public CustomerReportService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Get report data
        public async Task<CustomerReportViewModel> GetCustomerReportAsync(string userId, DateTime from, DateTime to, string groupBy)
        {
            var user = await _context.Users.FindAsync(userId);

            var prescriptions = await _context.Prescriptions
                .Include(p => p.MedicationItems)
                    .ThenInclude(mi => mi.Medication)
                .Where(p => p.CustomerId == userId
                            && p.PrescriptionStatus == PrescriptionStatus.Processed
                            && p.PrescriptionDate >= from
                            && p.PrescriptionDate <= to)
                .Select(p => new CustomerPrescriptionVM
                {
                    PrescriptionId = p.PrescriptionId,
                    PrescriptionDate = p.PrescriptionDate,
                    DoctorName = p.DoctorName,
                    TotalCost = p.TotalCost,
                    Medications = p.MedicationItems.Select(mi => new CustomerMedicationVM
                    {
                        Name = mi.Medication.Name,
                        Dosage = mi.Dosage,
                        Quantity = mi.Quantity,
                        RemainingRepeats = mi.RemainingRepeats,
                        TotalRepeats = mi.TotalRepeats
                    }).ToList()
                })
                .ToListAsync();

            return new CustomerReportViewModel
            {
                CustomerName = $"{user?.UserName ?? "Customer"}",
                FromDate = from,
                ToDate = to,
                GroupBy = groupBy,
                Prescriptions = prescriptions
            };
        }

        // Generate PDF using iTextSharp
        public byte[] GenerateCustomerReportPdf(CustomerReportViewModel report)
        {
            using (var ms = new MemoryStream())
            {
                Document doc = new Document(PageSize.A4, 40, 40, 40, 40);
                PdfWriter.GetInstance(doc, ms);
                doc.Open();

                var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 14);
                var headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);
                var normalFont = FontFactory.GetFont(FontFactory.HELVETICA, 10);

                doc.Add(new Paragraph("IBHAYI PHARMACY", titleFont));
                doc.Add(new Paragraph("Customer Prescription & Orders Report", headerFont));
                doc.Add(new Paragraph($"Customer: {report.CustomerName}", normalFont));
                doc.Add(new Paragraph($"Date Range: {report.FromDate:dd MMM yyyy} - {report.ToDate:dd MMM yyyy}", normalFont));
                doc.Add(new Paragraph($"Grouped By: {report.GroupBy}", normalFont));
                doc.Add(new Paragraph(" "));

                if (!report.Prescriptions.Any())
                {
                    doc.Add(new Paragraph("No prescriptions found for the selected date range.", normalFont));
                }
                else
                {
                    // Group dynamically
                    IEnumerable<(string Key, IEnumerable<CustomerPrescriptionVM> Prescriptions)> grouped;

                    switch (report.GroupBy.ToLower())
                    {
                        case "doctor":
                            grouped = report.Prescriptions
                                .GroupBy(p => p.DoctorName)
                                .Select(g => (Key: g.Key ?? "Unknown Doctor", Prescriptions: g.AsEnumerable()));
                            break;

                        case "medication":
                            grouped = report.Prescriptions
                                .SelectMany(p => p.Medications.Select(m => new { m.Name, Prescription = p }))
                                .GroupBy(x => x.Name)
                                .Select(g => (Key: g.Key ?? "Unknown Medication", Prescriptions: g.Select(x => x.Prescription)));
                            break;

                        default: // Patient grouping
                            grouped = new[]
                            {
                                (Key: report.CustomerName ?? "Patient", Prescriptions: report.Prescriptions.AsEnumerable())
                            };
                            break;
                    }

                    foreach (var group in grouped)
                    {
                        doc.Add(new Paragraph($"{report.GroupBy}: {group.Key}", headerFont));
                        doc.Add(new Paragraph(" "));

                        PdfPTable table = new PdfPTable(5) { WidthPercentage = 100 };
                        table.AddCell("Prescription ID");
                        table.AddCell("Date");
                        table.AddCell("Doctor");
                        table.AddCell("Medication(s)");
                        table.AddCell("Total Cost (R)");

                        foreach (var p in group.Prescriptions)
                        {
                            var meds = string.Join(", ", p.Medications.Select(m =>
                                $"{m.Name} ({m.Quantity}x, {m.Dosage}) [{m.RemainingRepeats}/{m.TotalRepeats}]"));

                            table.AddCell(p.PrescriptionId.ToString());
                            table.AddCell(p.PrescriptionDate.ToString("dd MMM yyyy"));
                            table.AddCell(p.DoctorName);
                            table.AddCell(meds);
                            table.AddCell(p.TotalCost.ToString("F2"));
                        }

                        doc.Add(table);
                        doc.Add(new Paragraph(" "));
                    }
                }

                doc.Close();
                return ms.ToArray();
            }
        }
    }
}
