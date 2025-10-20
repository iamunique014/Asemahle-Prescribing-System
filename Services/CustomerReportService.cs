using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.EntityFrameworkCore;
using PrescribingSystem.Data;
using PrescribingSystem.Models;
using PrescribingSystem.Models.ViewModels;

namespace PrescribingSystem.Services
{
    public class CustomerReportService
    {
        private readonly ApplicationDbContext _context;

        public CustomerReportService(ApplicationDbContext context)
        {
            _context = context;
        }

        // ------------------------------------------------------------
        // 1. GET CUSTOMER REPORT DATA
        // ------------------------------------------------------------
        public async Task<CustomerReportViewModel> GetCustomerReportAsync(string userId, DateTime fromDate, DateTime toDate, string groupBy)
        {
            var user = await _context.Users.FindAsync(userId);

            // --- Load prescriptions and include items + medication ---
            var prescriptions = await _context.Prescriptions
                .Include(p => p.MedicationItems)
                    .ThenInclude(mi => mi.Medication)
                .Where(p => p.CustomerId == userId
                            && p.PrescriptionStatus == PrescriptionStatus.Processed
                            && p.PrescriptionDate >= fromDate
                            && p.PrescriptionDate <= toDate)
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

            // --- Calculate cost if 0 in DB ---
            foreach (var p in prescriptions)
            {
                if (p.TotalCost == 0 && p.Medications.Any())
                {
                    decimal total = 0;
                    foreach (var m in p.Medications)
                    {
                        var medPrice = await _context.Medication
                            .Where(x => x.Name == m.Name)
                            .Select(x => x.CurrentSalesPrice)
                            .FirstOrDefaultAsync();

                        total += medPrice * m.Quantity;
                    }

                    p.TotalCost = total;
                }
            }

            // --- Load orders for this customer (within range) ---
            // We'll find orders that contain OrderItems which reference MedicationItems that belong to the prescriptions we loaded.
            var prescriptionIds = prescriptions.Select(p => p.PrescriptionId).ToList();

            if (prescriptionIds.Any())
            {
                // Query order items joined to medication items and orders
                var orderItemsWithOrderAndMed = await (
                    from oi in _context.OrderItems
                    join mi in _context.MedicationItems on oi.MedicationItemId equals mi.MedicationItemId
                    join o in _context.PrescriptionOrders on oi.OrderId equals o.PrescriptionOrdersId
                    where o.CustomerId == userId
                          && o.OrderDate >= fromDate
                          && o.OrderDate <= toDate
                          && prescriptionIds.Contains(mi.PrescriptionId)
                    select new
                    {
                        PrescriptionId = mi.PrescriptionId,
                        OrderId = o.PrescriptionOrdersId,
                        o.OrderDate,
                        o.ReadyDate,
                        o.CollectedDate,
                        o.OrderStatus,
                        o.TotalCost
                    }
                ).ToListAsync();

                // Group by prescriptionId then by orderId to get distinct orders per prescription
                var ordersByPrescription = orderItemsWithOrderAndMed
                    .GroupBy(x => x.PrescriptionId)
                    .ToDictionary(
                        g => g.Key,
                        g => g.GroupBy(x => x.OrderId)
                              .Select(og => new CustomerOrderVM
                              {
                                  OrderDate = og.First().OrderDate,
                                  ReadyDate = og.First().ReadyDate,
                                  CollectedDate = og.First().CollectedDate,
                                  OrderStatus = og.First().OrderStatus.ToString(),
                                  TotalCost = og.First().TotalCost
                              })
                              .ToList()
                    );

                // Attach orders to matching prescriptions
                foreach (var p in prescriptions)
                {
                    if (ordersByPrescription.TryGetValue(p.PrescriptionId, out var orderList))
                    {
                        p.Orders = orderList;
                    }
                    else
                    {
                        p.Orders = new List<CustomerOrderVM>();
                    }
                }
            }
            else
            {
                // No prescriptions — ensure Orders lists exist
                foreach (var p in prescriptions)
                    p.Orders = new List<CustomerOrderVM>();
            }

            return new CustomerReportViewModel
            {
                CustomerName = $"{user?.UserName ?? "Customer"}",
                FromDate = fromDate,
                ToDate = toDate,
                GroupBy = groupBy,
                Prescriptions = prescriptions
            };
        }

        // ------------------------------------------------------------
        // 2. GENERATE PDF
        // ------------------------------------------------------------
        public byte[] GenerateCustomerReportPdf(CustomerReportViewModel report)
        {
            using (var ms = new MemoryStream())
            {
                Document doc = new Document(PageSize.A4, 40, 40, 40, 40);
                PdfWriter.GetInstance(doc, ms);
                doc.Open();

                // Fonts
                var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 14);
                var headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);
                var normalFont = FontFactory.GetFont(FontFactory.HELVETICA, 10);

                // Header
                doc.Add(new Paragraph("IBHAYI PHARMACY", titleFont));
                doc.Add(new Paragraph("Customer Prescription & Orders Report", headerFont));
                doc.Add(new Paragraph($"Customer: {report.CustomerName}", normalFont));
                doc.Add(new Paragraph($"Date Range: {report.FromDate:dd MMM yyyy} - {report.ToDate:dd MMM yyyy}", normalFont));
                doc.Add(new Paragraph($"Grouped By: {report.GroupBy}", normalFont));
                doc.Add(new Paragraph(" "));

                if (!report.Prescriptions.Any())
                {
                    doc.Add(new Paragraph("No prescriptions found for the selected date range.", normalFont));
                    doc.Close();
                    return ms.ToArray();
                }

                // --- Group dynamically ---
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

                    default:
                        grouped = new[]
                        {
                            (Key: report.CustomerName ?? "Patient", Prescriptions: report.Prescriptions.AsEnumerable())
                        };
                        break;
                }

                // --- Build PDF content ---
                foreach (var group in grouped)
                {// Patient header row (with date on the right)
                    PdfPTable headerTable = new PdfPTable(2);
                    headerTable.WidthPercentage = 100;
                    headerTable.SetWidths(new float[] { 70f, 30f });

                    PdfPCell leftHeader = new PdfPCell(new Phrase($"{report.GroupBy}: {group.Key}", headerFont))
                    {
                        Border = Rectangle.NO_BORDER,
                        HorizontalAlignment = Element.ALIGN_LEFT
                    };
                    PdfPCell rightHeader = new PdfPCell(new Phrase($"Date: {DateTime.Now:dd MMM yyyy}", normalFont))
                    {
                        Border = Rectangle.NO_BORDER,
                        HorizontalAlignment = Element.ALIGN_RIGHT
                    };

                    headerTable.AddCell(leftHeader);
                    headerTable.AddCell(rightHeader);
                    doc.Add(headerTable);
                    doc.Add(new Paragraph(" "));

                    // Build table (exclude ID, keep doctor, meds, cost)
                    PdfPTable table = new PdfPTable(3) { WidthPercentage = 100 };
                    table.SetWidths(new float[] { 25f, 55f, 20f }); // wider Medications column
                    table.SpacingBefore = 5f;
                    table.SpacingAfter = 10f;

                    // Table headers
                    var headerBg = new BaseColor(230, 230, 230);
                    Font headerTableFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10);

                    PdfPCell docHeader = new PdfPCell(new Phrase("Doctor", headerTableFont)) { BackgroundColor = headerBg };
                    PdfPCell medsHeader = new PdfPCell(new Phrase("Medication(s)", headerTableFont)) { BackgroundColor = headerBg };
                    PdfPCell totalHeader = new PdfPCell(new Phrase("Total Cost (R)", headerTableFont)) { BackgroundColor = headerBg };

                    table.AddCell(docHeader);
                    table.AddCell(medsHeader);
                    table.AddCell(totalHeader);

                    // Table body
                    foreach (var p in group.Prescriptions)
                    {
                        string meds = string.Join("\n", p.Medications.Select(m =>
                            $"{m.Name} ({m.Quantity}x) - {m.Dosage}\n[Remaining: {m.RemainingRepeats}/{m.TotalRepeats}]"));

                        table.AddCell(new Phrase(p.DoctorName ?? "-", normalFont));

                        PdfPCell medCell = new PdfPCell(new Phrase(meds, normalFont))
                        {
                            MinimumHeight = 40f
                        };
                        table.AddCell(medCell);

                        table.AddCell(new Phrase(p.TotalCost.ToString("F2"), normalFont));

                        // --- Optional: include order details ---
                        if (p.Orders != null && p.Orders.Any())
                        {
                            var order = p.Orders.First();
                            PdfPCell orderCell = new PdfPCell(new Phrase(
                                $"Order Date: {order.OrderDate:dd MMM yyyy}\n" +
                                $"Status: {order.OrderStatus}\n" +
                                $"Ready: {(order.ReadyDate?.ToString("dd MMM yyyy") ?? "-")}\n" +
                                $"Collected: {(order.CollectedDate?.ToString("dd MMM yyyy") ?? "-")}\n" +
                                $"Order Total: {order.TotalCost:F2}", normalFont))
                            {
                                Colspan = 3
                            };
                            table.AddCell(orderCell);
                        }
                    }

                    doc.Add(table);
                    doc.Add(new Paragraph(" "));
                }

                doc.Close();
                return ms.ToArray();
            }
        }
    }
}

