using System.Reflection.Metadata;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Logic;
using Microsoft.AspNetCore.Mvc;
using Models.DTO;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly AppointmentBL serviceBL;

        public AppointmentController(AppointmentBL serviceBL)
        {
            this.serviceBL = serviceBL;
        }

        [HttpGet("count-today/{doctorId}")]
        public async Task<IActionResult> CountAppointmentsTodayByDoctor(int doctorId)
        {
            var count = await serviceBL.CountAppointmentsTodayByDoctor(doctorId);
            return Ok(new { AppointmentsToday = count });
        }

        [HttpGet("count-completed/{doctorId}")]
        public async Task<IActionResult> CountCompletedAppointmentByDoctor(int doctorId)
        {
            var count = await serviceBL.CountCompletedAppointmentByDoctor(doctorId);
            return Ok(new { CompletedAppointments = count });
        }

        [HttpGet("count-patients/{doctorId}")]
        public async Task<IActionResult> CountPatientsByDoctor(int doctorId)
        {
            var count = await serviceBL.CountPatientsByDoctor(doctorId);
            return Ok(new { TotalPatients = count });
        }

        [HttpGet("count-upcoming/{doctorId}")]
        public async Task<IActionResult> CountUpcomingAppointmentsByDoctor(int doctorId)
        {
            var count = await serviceBL.CountUpcomingAppointmentsByDoctor(doctorId);
            return Ok(new { UpcomingAppointments = count });
        }

        [HttpGet("appointments-by-doctor/{doctorId}")]
        public async Task<IActionResult> GetAppointmentsByDoctorAndDate(int doctorId, [FromQuery] DateTime? date)
        {
            if (doctorId <= 0)
                return BadRequest("Doctor ID no válido.");

            var appointments = await serviceBL.ListAppointmentDateByDoctor(doctorId, date);

            if (appointments == null || !appointments.Any())
                return NotFound("No se encontraron citas para esta fecha.");

            return Ok(appointments);
        }

        [HttpGet("appointments-for-7-days/{doctorId}")]
        public async Task<IActionResult> GetAppointmentsSummaryLast7Days(int doctorId)
        {
            if (doctorId <= 0)
                return BadRequest("Doctor ID no válido.");

            var appointments = await serviceBL.GetAppointmentsSummaryLast7Days(doctorId);

            if (appointments == null || !appointments.Any())
                return NotFound("No se encontraron citas para esta fecha.");

            return Ok(appointments);
        }
        [HttpPost("change-state")]
        public async Task<IActionResult> ChangeStateAppointment([FromBody]ChangeAppointmentStateRequest response)
        {
            if (response.IdAppointment <= 0)
                return BadRequest("ID de cita no válido.");

            var validStates = new[] { "A", "X", "P", "N" };
            if (!validStates.Contains(response.State))
                return BadRequest("Estado no válido.");

            var result = await serviceBL.ChangeStateAppointment(response.IdAppointment, response.State);

            if (!result.Value)
                return NotFound(result.Message);

            return Ok(result);
        }

        [HttpGet("appointment-for-id/{appointmentId}/pdf")]
        public async Task<IActionResult> DownloadSingleAppointmentPdf(int appointmentId)
        {
            if (appointmentId <= 0)
                return BadRequest("ID de cita no válido.");

            var appointment = await serviceBL.GetAppointmentForId(appointmentId);

            if (appointment == null)
                return NotFound("No se encontró la cita.");

            using (var memoryStream = new MemoryStream())
            {
                var document = new iTextSharp.text.Document(PageSize.A4, 40, 40, 40, 40);
                var writer = PdfWriter.GetInstance(document, memoryStream);
                document.Open();

                var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 22, new BaseColor(37, 99, 235));
                var sectionTitleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 13, new BaseColor(51, 51, 51));
                var bodyFont = FontFactory.GetFont(FontFactory.HELVETICA, 11, new BaseColor(75, 75, 75));
                var labelFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 11, new BaseColor(100, 100, 100));
                var footerFont = FontFactory.GetFont(FontFactory.HELVETICA_OBLIQUE, 9, new BaseColor(120, 120, 120));

                var birthDate = appointment.Patient.BirthDate;
                var today = DateTime.Today;
                int age = today.Year - birthDate.Year;
                if (birthDate.Date > today.AddYears(-age)) age--;

                var cb = writer.DirectContent;

                float pageWidth = document.PageSize.Width;
                float logoX = document.LeftMargin;
                float logoY = document.Top - 15;

                cb.SaveState();
                float circleDiameter = 35f;
                float centerX = logoX + circleDiameter / 2;
                float centerY = logoY - circleDiameter / 2;

                BaseColor startColor = new BaseColor(0x25, 0x63, 0xeb);
                BaseColor endColor = new BaseColor(0x06, 0xb6, 0xd4);

                var shading = PdfShading.SimpleAxial(writer,
                    centerX - circleDiameter / 2, centerY,
                    centerX + circleDiameter / 2, centerY,
                    startColor, endColor);
                var shadingPattern = new PdfShadingPattern(shading);

                cb.SetShadingFill(shadingPattern);
                cb.Circle(centerX, centerY, circleDiameter / 2);
                cb.Fill();

                float heartSize = 20f;
                float heartX = centerX - heartSize / 2;
                float heartY = centerY - heartSize / 2 + 2;

                cb.SetLineWidth(2f);
                cb.SetColorStroke(BaseColor.WHITE);

                cb.MoveTo(heartX + heartSize * 0.5f, heartY - heartSize * 0.1f);

                cb.CurveTo(heartX + heartSize * 0.5f, heartY - heartSize * 0.1f,
                           heartX, heartY + heartSize * 0.25f,
                           heartX, heartY + heartSize * 0.5f);

                cb.CurveTo(heartX, heartY + heartSize * 0.75f,
                           heartX + heartSize * 0.25f, heartY + heartSize * 0.95f,
                           heartX + heartSize * 0.5f, heartY + heartSize * 0.75f);

                cb.CurveTo(heartX + heartSize * 0.75f, heartY + heartSize * 0.95f,
                           heartX + heartSize, heartY + heartSize * 0.75f,
                           heartX + heartSize, heartY + heartSize * 0.5f);

                cb.CurveTo(heartX + heartSize, heartY + heartSize * 0.25f,
                           heartX + heartSize * 0.5f, heartY - heartSize * 0.1f,
                           heartX + heartSize * 0.5f, heartY - heartSize * 0.1f);

                cb.ClosePathStroke();
                cb.RestoreState();

                var logoTextFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16, BaseColor.BLACK);
                var columnText = new ColumnText(cb);
                columnText.SetSimpleColumn(
                    logoX + circleDiameter + 8,
                    logoY - circleDiameter - 5,
                    logoX + circleDiameter + 150,
                    logoY + 5);
                columnText.AddElement(new Paragraph("SaludConnect", logoTextFont));
                columnText.Go();

                cb.SaveState();
                cb.SetLineWidth(2f);
                cb.SetColorStroke(new BaseColor(37, 99, 235));
                cb.MoveTo(document.LeftMargin, logoY - circleDiameter - 15);
                cb.LineTo(pageWidth - document.RightMargin, logoY - circleDiameter - 15);
                cb.Stroke();
                cb.RestoreState();

                document.Add(new Paragraph(" ") { SpacingAfter = 55 });

                var title = new Paragraph("Comprobante de Cita Médica", titleFont)
                {
                    Alignment = Element.ALIGN_CENTER,
                    SpacingAfter = 25
                };
                document.Add(title);

                void AddCard(string cardTitle, List<(string label, string value)> data, BaseColor accentColor)
                {
                    var table = new PdfPTable(1)
                    {
                        WidthPercentage = 100,
                        SpacingBefore = 10,
                        SpacingAfter = 15
                    };

                    var headerCell = new PdfPCell(new Phrase(cardTitle, sectionTitleFont))
                    {
                        BackgroundColor = accentColor,
                        Padding = 10,
                        Border = Rectangle.NO_BORDER,
                        HorizontalAlignment = Element.ALIGN_LEFT
                    };
                    table.AddCell(headerCell);

                    var bodyTable = new PdfPTable(2)
                    {
                        WidthPercentage = 100
                    };
                    bodyTable.SetWidths(new float[] { 35f, 65f });

                    foreach (var (label, value) in data)
                    {
                        var labelCell = new PdfPCell(new Phrase(label + ":", labelFont))
                        {
                            Border = Rectangle.NO_BORDER,
                            Padding = 8,
                            BackgroundColor = new BaseColor(249, 250, 251),
                            HorizontalAlignment = Element.ALIGN_RIGHT
                        };
                        bodyTable.AddCell(labelCell);

                        var valueCell = new PdfPCell(new Phrase(value, bodyFont))
                        {
                            Border = Rectangle.NO_BORDER,
                            Padding = 8,
                            BackgroundColor = BaseColor.WHITE
                        };
                        bodyTable.AddCell(valueCell);
                    }

                    var bodyCell = new PdfPCell(bodyTable)
                    {
                        Border = Rectangle.BOX,
                        BorderColor = new BaseColor(229, 231, 235),
                        BorderWidth = 1f,
                        Padding = 0
                    };
                    table.AddCell(bodyCell);

                    document.Add(table);
                }

                AddCard("📅 Detalles de la Cita", new List<(string, string)>
        {
            ("Fecha", appointment.DateAppointment.ToString("dd/MM/yyyy")),
            ("Hora", appointment.DateAppointment.ToString("HH:mm")),
            ("Servicio", appointment.Service.NameService),
            ("Precio", $"S/ {appointment.Service.Price:0.00}")
        }, new BaseColor(219, 234, 254));

                AddCard("👤 Datos del Paciente", new List<(string, string)>
        {
            ("Nombre completo", $"{appointment.Patient.FirstName} {appointment.Patient.LastNamePat} {appointment.Patient.LastNameMat}"),
            ("DNI", appointment.Patient.Document),
            ("Edad", $"{age} años"),
            ("Teléfono", appointment.Patient.Phone)
        }, new BaseColor(220, 252, 231));

                var infoBox = new PdfPTable(1)
                {
                    WidthPercentage = 100,
                    SpacingBefore = 20
                };

                var infoCell = new PdfPCell(new Phrase("ℹ️ Recuerde llegar 10 minutos antes de su cita y traer su documento de identidad.", bodyFont))
                {
                    BackgroundColor = new BaseColor(254, 249, 195),
                    Border = Rectangle.BOX,
                    BorderColor = new BaseColor(253, 224, 71),
                    Padding = 12,
                    HorizontalAlignment = Element.ALIGN_LEFT
                };
                infoBox.AddCell(infoCell);
                document.Add(infoBox);

                var thankYou = new Paragraph("Gracias por confiar en nosotros.\nSaludConnect está comprometido con tu bienestar.", footerFont)
                {
                    SpacingBefore = 35,
                    Alignment = Element.ALIGN_CENTER
                };
                document.Add(thankYou);

                cb.SaveState();
                cb.SetLineWidth(1f);
                cb.SetColorStroke(new BaseColor(229, 231, 235));
                cb.MoveTo(document.LeftMargin + 100, document.Bottom + 30);
                cb.LineTo(pageWidth - document.RightMargin - 100, document.Bottom + 30);
                cb.Stroke();
                cb.RestoreState();

                document.Close();

                var fileName = $"Cita_{appointment.Patient.FirstName}_{appointment.DateAppointment:yyyyMMdd}.pdf";
                return File(memoryStream.ToArray(), "application/pdf", fileName);
            }
        }

    }
}
