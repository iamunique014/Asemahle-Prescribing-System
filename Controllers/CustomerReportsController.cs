using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrescribingSystem.Data;
using PrescribingSystem.Models;
using PrescribingSystem.Models.ViewModels;
using System.Security.Claims;

namespace PrescribingSystem.Controllers
{
    public class CustomerReportsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CustomerReportsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult MyReports()
        {
            return View();
        }

        public async Task<IActionResult> DispensedPrescriptionPdf(DateTime fromDate, DateTime to, string groupBy)
        {
            var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Query prescriptions joined with PrescriptionOrders and MedicationItems
            var data = await (from p in _context.Prescriptions
                              join o in _context.PrescriptionOrders
                                  on p.CustomerId equals o.CustomerId
                              join oi in _context.OrderItems
                                on o.PrescriptionOrdersId equals oi.OrderId
                              from m in _context.MedicationItems
                                  .Where(mi => mi.PrescriptionId == p.PrescriptionId)
                              where p.CustomerId == customerId
                                    && p.PrescriptionDate >= fromDate
                                    && p.PrescriptionDate <= to
                                    && p.PrescriptionStatus == PrescriptionStatus.Processed
                                    && o.OrderStatus == OrderStatus.Collected
                              select new DispensedPrescriptionReportVM
                              {
                                  PatientName = p.CustomerId, // or full name
                                  DoctorName = p.DoctorName,
                                  MedicationName = m.Medication.Name,
                                  Quantity = m.Quantity,
                                  TotalCost = oi.LineTotal,
                                  PrescriptionDate = p.PrescriptionDate,
                                  OrderDate = o.OrderDate
                              }).ToListAsync();

            var groupedData = groupBy switch
            {
                "Doctor" => data.GroupBy(d => d.DoctorName),
                "Medication" => data.GroupBy(d => d.MedicationName),
                _ => data.GroupBy(d => d.PatientName)
            };

            using var ms = new MemoryStream();
            Document doc = new Document(PageSize.A4, 20, 20, 20, 20);
            PdfWriter.GetInstance(doc, ms);
            doc.Open();

            var titleFont = FontFactory.GetFont("Arial", 16, Font.BOLD);
            var headerFont = FontFactory.GetFont("Arial", 12, Font.BOLD);
            var cellFont = FontFactory.GetFont("Arial", 10);

            doc.Add(new Paragraph($"Dispensed Prescriptions Report ({fromDate:yyyy-MM-dd} to {to:yyyy-MM-dd})", titleFont));
            doc.Add(new Paragraph("\n"));

            foreach (var group in groupedData)
            {
                doc.Add(new Paragraph($"--- {group.Key} ---", headerFont));

                PdfPTable table = new PdfPTable(6);
                table.WidthPercentage = 100;
                table.SetWidths(new float[] { 20f, 20f, 20f, 10f, 15f, 15f });

                table.AddCell(new PdfPCell(new Phrase("Patient", headerFont)));
                table.AddCell(new PdfPCell(new Phrase("Doctor", headerFont)));
                table.AddCell(new PdfPCell(new Phrase("Medication", headerFont)));
                table.AddCell(new PdfPCell(new Phrase("Qty", headerFont)));
                table.AddCell(new PdfPCell(new Phrase("Prescription Date", headerFont)));
                table.AddCell(new PdfPCell(new Phrase("Total Cost", headerFont)));

                int totalQty = 0;
                decimal totalCost = 0;

                foreach (var item in group)
                {
                    table.AddCell(new PdfPCell(new Phrase(item.PatientName, cellFont)));
                    table.AddCell(new PdfPCell(new Phrase(item.DoctorName, cellFont)));
                    table.AddCell(new PdfPCell(new Phrase(item.MedicationName, cellFont)));
                    table.AddCell(new PdfPCell(new Phrase(item.Quantity.ToString(), cellFont)));
                    table.AddCell(new PdfPCell(new Phrase(item.PrescriptionDate.ToString("yyyy-MM-dd"), cellFont)));
                    table.AddCell(new PdfPCell(new Phrase(item.TotalCost.ToString("C"), cellFont)));

                    totalQty += item.Quantity;
                    totalCost += item.TotalCost;
                }

                // Add totals row
                PdfPCell totalCell = new PdfPCell(new Phrase("Total", headerFont));
                totalCell.Colspan = 3;
                totalCell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(totalCell);

                table.AddCell(new PdfPCell(new Phrase(totalQty.ToString(), headerFont)));
                table.AddCell(new PdfPCell(new Phrase("", headerFont)));
                table.AddCell(new PdfPCell(new Phrase(totalCost.ToString("C"), headerFont)));

                doc.Add(table);
                doc.Add(new Paragraph("\n"));
            }

            doc.Close();
            return File(ms.ToArray(), "application/pdf", $"DispensedReport_{DateTime.Now:yyyyMMddHHmmss}.pdf");
        }
    }
}
