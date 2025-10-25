using System.Xml.Linq;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Office2010.PowerPoint;
using DocumentFormat.OpenXml.Wordprocessing;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Mvc;
using Web.Services.Interface;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Paragraph = iTextSharp.text.Paragraph;
using Document = iTextSharp.text.Document;
using PageSize = iTextSharp.text.PageSize;

namespace Web.Controllers
{
    public class ReportController : Controller
    {
        private readonly IAdmin _admin;

        public ReportController(IAdmin admin)
        {
            _admin = admin;
        }

        // Descargar en PDF
        public async Task<IActionResult> DownloadGeneralReportPDF()
        {
            try
            {
                // Obtener datos
                var totalAppointments = await _admin.GetTotalAppointments();
                var totalPatients = await _admin.GetTotalPatients();
                var totalDoctors = await _admin.GetTotalDoctors();
                var todayAppointments = await _admin.GetTodayAppointments();
                var revenue = await _admin.GetTotalRevenue();
                var appointmentsByState = await _admin.GetAppointmentsByState();

                // Crear documento PDF
                var memoryStream = new MemoryStream();
                var document = new Document(PageSize.A4, 25, 25, 30, 30);
                var writer = PdfWriter.GetInstance(document, memoryStream);
                document.Open();

                // Título
                var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18, BaseColor.DARK_GRAY);
                var title = new Paragraph("REPORTE GENERAL DE MÉTRICAS\n\n", titleFont);
                title.Alignment = Element.ALIGN_CENTER;
                document.Add(title);

                // Fecha de generación
                var dateFont = FontFactory.GetFont(FontFactory.HELVETICA, 10, BaseColor.GRAY);
                var dateText = new Paragraph($"Fecha de generación: {DateTime.Now:dd/MM/yyyy HH:mm}\n\n", dateFont);
                dateText.Alignment = Element.ALIGN_RIGHT;
                document.Add(dateText);

                // Métricas Principales
                var headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 14, BaseColor.BLACK);
                document.Add(new Paragraph("MÉTRICAS PRINCIPALES\n\n", headerFont));

                var contentFont = FontFactory.GetFont(FontFactory.HELVETICA, 11);
                document.Add(new Paragraph($"Total de Citas: {totalAppointments}", contentFont));
                document.Add(new Paragraph($"Total de Pacientes: {totalPatients}", contentFont));
                document.Add(new Paragraph($"Total de Doctores: {totalDoctors}", contentFont));
                document.Add(new Paragraph($"Citas de Hoy: {todayAppointments}\n\n", contentFont));

               

                // Tabla de Citas por Estado
                if (appointmentsByState != null && appointmentsByState.Count > 0)
                {
                    document.Add(new Paragraph("DISTRIBUCIÓN DE CITAS POR ESTADO\n\n", headerFont));

                    var table = new PdfPTable(2);
                    table.WidthPercentage = 100;
                    table.SetWidths(new float[] { 70f, 30f });

                    // Headers
                    var cellHeaderFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 11, BaseColor.WHITE);
                    var headerCell = new PdfPCell(new Phrase("Estado", cellHeaderFont));
                    headerCell.BackgroundColor = new BaseColor(102, 126, 234);
                    headerCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    headerCell.Padding = 8;
                    table.AddCell(headerCell);

                    headerCell = new PdfPCell(new Phrase("Total", cellHeaderFont));
                    headerCell.BackgroundColor = new BaseColor(102, 126, 234);
                    headerCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    headerCell.Padding = 8;
                    table.AddCell(headerCell);

                    // Datos
                    var cellFont = FontFactory.GetFont(FontFactory.HELVETICA, 10);
                    foreach (var item in appointmentsByState)
                    {
                        var cell = new PdfPCell(new Phrase(item.StateName, cellFont));
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase(item.Total.ToString(), cellFont));
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        cell.Padding = 5;
                        table.AddCell(cell);
                    }

                    document.Add(table);
                }

                document.Close();

                var bytes = memoryStream.ToArray();
                memoryStream.Close();

                return File(bytes, "application/pdf", $"Reporte_General_{DateTime.Now:yyyyMMdd_HHmmss}.pdf");
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al generar el reporte PDF: {ex.Message}";
                return RedirectToAction("Dashboard", "Admin");
            }
        }

        public async Task<IActionResult> DownloadSpecialtiesReportPDF()
        {
            try
            {
                // Obtener datos
                var topSpecialties = await _admin.GetTopSpecialties(10);
                var appointmentsByState = await _admin.GetAppointmentsByState();
                var revenue = await _admin.GetTotalRevenue();

                // Crear documento PDF
                var memoryStream = new MemoryStream();
                var document = new Document(PageSize.A4, 25, 25, 30, 30);
                var writer = PdfWriter.GetInstance(document, memoryStream);
                document.Open();

                // Título
                var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18, BaseColor.DARK_GRAY);
                var title = new Paragraph("REPORTE DE ESPECIALIDADES Y ESTADÍSTICAS\n\n", titleFont);
                title.Alignment = Element.ALIGN_CENTER;
                document.Add(title);

                // Fecha
                var dateFont = FontFactory.GetFont(FontFactory.HELVETICA, 10, BaseColor.GRAY);
                var dateText = new Paragraph($"Fecha de generación: {DateTime.Now:dd/MM/yyyy HH:mm}\n\n", dateFont);
                dateText.Alignment = Element.ALIGN_RIGHT;
                document.Add(dateText);

                // Resumen Financiero
                var headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 14, BaseColor.BLACK);
               
                

                // Top Especialidades
                if (topSpecialties != null && topSpecialties.Count > 0)
                {
                    document.Add(new Paragraph("TOP ESPECIALIDADES MÁS SOLICITADAS\n\n", headerFont));

                    var table = new PdfPTable(3);
                    table.WidthPercentage = 100;
                    table.SetWidths(new float[] { 10f, 60f, 30f });

                    // Headers
                    var cellHeaderFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 11, BaseColor.WHITE);

                    var headerCell = new PdfPCell(new Phrase("#", cellHeaderFont));
                    headerCell.BackgroundColor = new BaseColor(13, 202, 240);
                    headerCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    headerCell.Padding = 8;
                    table.AddCell(headerCell);

                    headerCell = new PdfPCell(new Phrase("Especialidad", cellHeaderFont));
                    headerCell.BackgroundColor = new BaseColor(13, 202, 240);
                    headerCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    headerCell.Padding = 8;
                    table.AddCell(headerCell);

                    headerCell = new PdfPCell(new Phrase("Total Citas", cellHeaderFont));
                    headerCell.BackgroundColor = new BaseColor(13, 202, 240);
                    headerCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    headerCell.Padding = 8;
                    table.AddCell(headerCell);

                    // Datos
                    var cellFont = FontFactory.GetFont(FontFactory.HELVETICA, 10);
                    int position = 1;
                    foreach (var item in topSpecialties)
                    {
                        var cell = new PdfPCell(new Phrase(position.ToString(), cellFont));
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase(item.NameSpecialty, cellFont));
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase(item.TotalAppointments.ToString(), cellFont));
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        position++;
                    }

                    document.Add(table);
                }

                

                document.Close();

                var bytes = memoryStream.ToArray();
                memoryStream.Close();

                return File(bytes, "application/pdf", $"Reporte_Especialidades_{DateTime.Now:yyyyMMdd_HHmmss}.pdf");
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al generar el reporte PDF: {ex.Message}";
                return RedirectToAction("Dashboard", "Admin");
            }
        }
    }
}
