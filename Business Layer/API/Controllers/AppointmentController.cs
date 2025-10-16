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
        public async Task<IActionResult> ChangeStateAppointment([FromBody] ChangeAppointmentStateRequest response)
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
        [HttpGet("appointment-for-id/{appointmentId}")]
        public async Task<IActionResult> GetAppointmentForID(int appointmentId)
        {
            if (appointmentId <= 0)
                return BadRequest("Cita id no válido.");

            var appointments = await serviceBL.GetAppointmentForId(appointmentId);

            return Ok(appointments);
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
                var sectionTitleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 13, BaseColor.WHITE);
                var bodyFont = FontFactory.GetFont(FontFactory.HELVETICA, 11, new BaseColor(75, 75, 75));
                var labelFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 11, new BaseColor(100, 100, 100));
                var footerFont = FontFactory.GetFont(FontFactory.HELVETICA_OBLIQUE, 9, new BaseColor(120, 120, 120));
                var statusFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10, BaseColor.WHITE);

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

                // Texto del logo
                var logoTextFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16, BaseColor.BLACK);
                var columnText = new ColumnText(cb);
                columnText.SetSimpleColumn(
                    logoX + circleDiameter + 8,
                    logoY - circleDiameter - 5,
                    logoX + circleDiameter + 150,
                    logoY + 5);
                columnText.AddElement(new Paragraph("SaludConnect", logoTextFont));
                columnText.Go();

                // Badge de estado (esquina superior derecha)
                string estadoTexto = appointment.State switch
                {
                    "P" => "PENDIENTE",
                    "A" => "ASISTIDA",
                    "X" => "CANCELADA",
                    "N" => "NO ASISTIÓ",
                    _ => "DESCONOCIDO"
                };

                BaseColor estadoColor = appointment.State switch
                {
                    "P" => new BaseColor(251, 191, 36),  // Amarillo
                    "A" => new BaseColor(34, 197, 94),   // Verde
                    "X" => new BaseColor(239, 68, 68),   // Rojo
                    "N" => new BaseColor(148, 163, 184), // Gris
                    _ => new BaseColor(100, 100, 100)
                };

                float badgeWidth = 90f;
                float badgeHeight = 22f;
                float badgeX = pageWidth - document.RightMargin - badgeWidth;
                float badgeY = logoY - 10;

                cb.SaveState();
                cb.SetColorFill(estadoColor);
                cb.RoundRectangle(badgeX, badgeY, badgeWidth, badgeHeight, 4f);
                cb.Fill();
                cb.RestoreState();

                var badgeText = new ColumnText(cb);
                badgeText.SetSimpleColumn(badgeX, badgeY, badgeX + badgeWidth, badgeY + badgeHeight);
                var statusPara = new Paragraph(estadoTexto, statusFont) { Alignment = Element.ALIGN_CENTER };
                badgeText.AddElement(statusPara);
                badgeText.Go();

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
                    SpacingAfter = 5
                };
                document.Add(title);

                var appointmentNro = new Paragraph($"N° Cita: {appointment.IdAppointment.ToString().PadLeft(6, '0')}", bodyFont)
                {
                    Alignment = Element.ALIGN_CENTER,
                    SpacingAfter = 25
                };
                document.Add(appointmentNro);

                void AddCard(string cardTitle, List<(string label, string value)> data, BaseColor headerColor)
                {
                    var table = new PdfPTable(1)
                    {
                        WidthPercentage = 100,
                        SpacingBefore = 10,
                        SpacingAfter = 15
                    };

                    var headerCell = new PdfPCell(new Phrase(cardTitle, sectionTitleFont))
                    {
                        BackgroundColor = headerColor,
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
            ("Fecha", appointment.DateAppointment.ToString("dddd, dd 'de' MMMM 'de' yyyy", new System.Globalization.CultureInfo("es-ES"))),
            ("Hora", appointment.DateAppointment.ToString("hh:mm tt")),
            ("Especialidad", appointment.Specialty.NameSpecialty),
            ("Precio de Consulta", $"S/ {appointment.AppointmentPrice:0.00}")
        }, new BaseColor(37, 99, 235));

                AddCard("👤 Información del Paciente", new List<(string, string)>
        {
            ("Nombre Completo", $"{appointment.Patient.FirstName} {appointment.Patient.LastNamePat} {appointment.Patient.LastNameMat}"),
            ("DNI", appointment.Patient.Document),
            ("Edad", $"{age} años"),
            ("Fecha de Nacimiento", appointment.Patient.BirthDate.ToString("dd/MM/yyyy")),
            ("Género", appointment.Patient.Gender == "M" ? "Masculino" : "Femenino"),
            ("Teléfono", appointment.Patient.Phone),
            ("Email", appointment.Patient.Email)
        }, new BaseColor(34, 197, 94));

                AddCard("👨‍⚕️ Médico Asignado", new List<(string, string)>
        {
            ("Doctor(a)", $"Dr(a). {appointment.Doctor.FirstName} {appointment.Doctor.LastNamePat} {appointment.Doctor.LastNameMat}"),
            ("Especialidad", appointment.Specialty.NameSpecialty)
        }, new BaseColor(139, 92, 246));

                AddCard("📍 Ubicación", new List<(string, string)>
        {
            ("Consultorio", appointment.Office.NroOffice),
            ("Piso", appointment.Office.FloorNumber.ToString())
        }, new BaseColor(236, 72, 153));

                var infoBox = new PdfPTable(1)
                {
                    WidthPercentage = 100,
                    SpacingBefore = 20
                };

                var infoText = "ℹ️ Recordatorios Importantes:\n\n" +
                               "• Llegar 10 minutos antes de su cita\n" +
                               "• Traer documento de identidad original\n" +
                               "• En caso de no poder asistir, cancelar con 24h de anticipación";

                var infoCell = new PdfPCell(new Phrase(infoText, bodyFont))
                {
                    BackgroundColor = new BaseColor(254, 249, 195),
                    Border = Rectangle.BOX,
                    BorderColor = new BaseColor(253, 224, 71),
                    BorderWidth = 1.5f,
                    Padding = 15,
                    HorizontalAlignment = Element.ALIGN_LEFT
                };
                infoBox.AddCell(infoCell);
                document.Add(infoBox);

                var thankYou = new Paragraph(
                    "Gracias por confiar en nosotros.\n" +
                    "SaludConnect está comprometido con tu bienestar.\n" +
                    $"Generado el: {DateTime.Now:dd/MM/yyyy HH:mm}",
                    footerFont)
                {
                    SpacingBefore = 35,
                    Alignment = Element.ALIGN_CENTER
                };
                document.Add(thankYou);

                // Línea final decorativa
                cb.SaveState();
                cb.SetLineWidth(1f);
                cb.SetColorStroke(new BaseColor(229, 231, 235));
                cb.MoveTo(document.LeftMargin + 100, document.Bottom + 30);
                cb.LineTo(pageWidth - document.RightMargin - 100, document.Bottom + 30);
                cb.Stroke();
                cb.RestoreState();

                document.Close();

                var fileName = $"Cita_{appointment.Patient.LastNamePat}_{appointment.DateAppointment:yyyyMMdd}_{appointment.IdAppointment}.pdf";

                Response.Headers.Add("Content-Disposition",
                    "attachment; filename*=UTF-8''" + Uri.EscapeDataString(fileName));

                return File(memoryStream.ToArray(), "application/pdf");

            }
        }

        [HttpGet("appointment-for-id/{appointmentId}/medical-record-pdf")]
        public async Task<IActionResult> DownloadMedicalRecordPdf(int appointmentId)
        {
            if (appointmentId <= 0)
                return BadRequest("ID de cita no válido.");

            var appointment = await serviceBL.GetAppointmentForId(appointmentId);

            if (appointment == null)
                return NotFound("No se encontró la cita.");

            if (appointment.MedicalRecord == null)
                return BadRequest("Esta cita no tiene un registro médico asociado.");

            using (var memoryStream = new MemoryStream())
            {
                var document = new iTextSharp.text.Document(PageSize.A4, 40, 40, 40, 40);
                var writer = PdfWriter.GetInstance(document, memoryStream);
                document.Open();

                var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 20, new BaseColor(220, 38, 38));
                var sectionTitleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 14, BaseColor.WHITE);
                var subsectionFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12, new BaseColor(51, 51, 51));
                var bodyFont = FontFactory.GetFont(FontFactory.HELVETICA, 11, new BaseColor(75, 75, 75));
                var labelFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 11, new BaseColor(100, 100, 100));
                var footerFont = FontFactory.GetFont(FontFactory.HELVETICA_OBLIQUE, 9, new BaseColor(120, 120, 120));
                var importantFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 11, new BaseColor(220, 38, 38));

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

                BaseColor startColor = new BaseColor(0xdc, 0x26, 0x26);
                BaseColor endColor = new BaseColor(0xef, 0x44, 0x44);

                var shading = PdfShading.SimpleAxial(writer,
                    centerX - circleDiameter / 2, centerY,
                    centerX + circleDiameter / 2, centerY,
                    startColor, endColor);
                var shadingPattern = new PdfShadingPattern(shading);

                cb.SetShadingFill(shadingPattern);
                cb.Circle(centerX, centerY, circleDiameter / 2);
                cb.Fill();

                float crossSize = 18f;
                float crossThickness = 5f;
                float crossX = centerX - crossSize / 2;
                float crossY = centerY - crossSize / 2;

                cb.SetColorFill(BaseColor.WHITE);
                cb.Rectangle(crossX + (crossSize - crossThickness) / 2, crossY, crossThickness, crossSize);
                cb.Fill();

                cb.Rectangle(crossX, crossY + (crossSize - crossThickness) / 2, crossSize, crossThickness);
                cb.Fill();
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

                float badgeWidth = 120f;
                float badgeHeight = 22f;
                float badgeX = pageWidth - document.RightMargin - badgeWidth;
                float badgeY = logoY - 10;

                cb.SaveState();
                cb.SetColorFill(new BaseColor(220, 38, 38));
                cb.RoundRectangle(badgeX, badgeY, badgeWidth, badgeHeight, 4f);
                cb.Fill();
                cb.RestoreState();

                var confidencialFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10, BaseColor.WHITE);
                var badgeText = new ColumnText(cb);
                badgeText.SetSimpleColumn(badgeX, badgeY, badgeX + badgeWidth, badgeY + badgeHeight);
                var statusPara = new Paragraph("🔒 CONFIDENCIAL", confidencialFont) { Alignment = Element.ALIGN_CENTER };
                badgeText.AddElement(statusPara);
                badgeText.Go();

                // Línea divisoria
                cb.SaveState();
                cb.SetLineWidth(2f);
                cb.SetColorStroke(new BaseColor(220, 38, 38));
                cb.MoveTo(document.LeftMargin, logoY - circleDiameter - 15);
                cb.LineTo(pageWidth - document.RightMargin, logoY - circleDiameter - 15);
                cb.Stroke();
                cb.RestoreState();

                document.Add(new Paragraph(" ") { SpacingAfter = 55 });

                var title = new Paragraph("Historia Clínica - Registro Médico", titleFont)
                {
                    Alignment = Element.ALIGN_CENTER,
                    SpacingAfter = 5
                };
                document.Add(title);

                var recordId = new Paragraph($"Registro N°: {appointment.MedicalRecord.IdRecord.ToString().PadLeft(6, '0')}", bodyFont)
                {
                    Alignment = Element.ALIGN_CENTER,
                    SpacingAfter = 25
                };
                document.Add(recordId);

                void AddCard(string cardTitle, List<(string label, string value)> data, BaseColor headerColor)
                {
                    var table = new PdfPTable(1)
                    {
                        WidthPercentage = 100,
                        SpacingBefore = 10,
                        SpacingAfter = 15
                    };

                    var headerCell = new PdfPCell(new Phrase(cardTitle, sectionTitleFont))
                    {
                        BackgroundColor = headerColor,
                        Padding = 10,
                        Border = Rectangle.NO_BORDER,
                        HorizontalAlignment = Element.ALIGN_LEFT
                    };
                    table.AddCell(headerCell);

                    var bodyTable = new PdfPTable(2)
                    {
                        WidthPercentage = 100
                    };
                    bodyTable.SetWidths(new float[] { 30f, 70f });

                    foreach (var (label, value) in data)
                    {
                        var labelCell = new PdfPCell(new Phrase(label + ":", labelFont))
                        {
                            Border = Rectangle.NO_BORDER,
                            Padding = 8,
                            BackgroundColor = new BaseColor(249, 250, 251),
                            HorizontalAlignment = Element.ALIGN_RIGHT,
                            VerticalAlignment = Element.ALIGN_TOP
                        };
                        bodyTable.AddCell(labelCell);

                        var valueCell = new PdfPCell(new Phrase(value, bodyFont))
                        {
                            Border = Rectangle.NO_BORDER,
                            Padding = 8,
                            BackgroundColor = BaseColor.WHITE,
                            VerticalAlignment = Element.ALIGN_TOP
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

                AddCard("👤 Información del Paciente", new List<(string, string)>
        {
            ("Nombre Completo", $"{appointment.Patient.FirstName} {appointment.Patient.LastNamePat} {appointment.Patient.LastNameMat}"),
            ("DNI", appointment.Patient.Document),
            ("Edad", $"{age} años"),
            ("Género", appointment.Patient.Gender == "M" ? "Masculino" : "Femenino"),
            ("Fecha de Nacimiento", appointment.Patient.BirthDate.ToString("dd/MM/yyyy"))
        }, new BaseColor(100, 116, 139));

                AddCard("📋 Datos de la Consulta", new List<(string, string)>
        {
            ("Fecha de Atención", appointment.DateAppointment.ToString("dddd, dd 'de' MMMM 'de' yyyy", new System.Globalization.CultureInfo("es-ES"))),
            ("Hora", appointment.DateAppointment.ToString("hh:mm tt")),
            ("Médico Tratante", $"Dr(a). {appointment.Doctor.FirstName} {appointment.Doctor.LastNamePat} {appointment.Doctor.LastNameMat}"),
            ("Especialidad", appointment.Specialty.NameSpecialty),
            ("Consultorio", $"{appointment.Office.NroOffice} - Piso {appointment.Office.FloorNumber}"),
            ("Fecha de Reporte", appointment.MedicalRecord.DateReport.ToString("dd/MM/yyyy HH:mm"))
        }, new BaseColor(59, 130, 246));

                var obsTable = new PdfPTable(1)
                {
                    WidthPercentage = 100,
                    SpacingBefore = 10,
                    SpacingAfter = 15
                };

                var obsHeaderCell = new PdfPCell(new Phrase("📝 Observaciones Clínicas", sectionTitleFont))
                {
                    BackgroundColor = new BaseColor(16, 185, 129),
                    Padding = 10,
                    Border = Rectangle.NO_BORDER,
                    HorizontalAlignment = Element.ALIGN_LEFT
                };
                obsTable.AddCell(obsHeaderCell);

                var obsContentCell = new PdfPCell(new Phrase(appointment.MedicalRecord.Observations ?? "Sin observaciones", bodyFont))
                {
                    Border = Rectangle.BOX,
                    BorderColor = new BaseColor(229, 231, 235),
                    BorderWidth = 1f,
                    Padding = 12,
                    MinimumHeight = 60f
                };
                obsTable.AddCell(obsContentCell);
                document.Add(obsTable);

                // ==================== DIAGNÓSTICO ====================
                var diagTable = new PdfPTable(1)
                {
                    WidthPercentage = 100,
                    SpacingBefore = 10,
                    SpacingAfter = 15
                };

                var diagHeaderCell = new PdfPCell(new Phrase("🔬 Diagnóstico", sectionTitleFont))
                {
                    BackgroundColor = new BaseColor(220, 38, 38),
                    Padding = 10,
                    Border = Rectangle.NO_BORDER,
                    HorizontalAlignment = Element.ALIGN_LEFT
                };
                diagTable.AddCell(diagHeaderCell);

                var diagContentCell = new PdfPCell(new Phrase(appointment.MedicalRecord.Diagnosis ?? "Sin diagnóstico", importantFont))
                {
                    Border = Rectangle.BOX,
                    BorderColor = new BaseColor(220, 38, 38),
                    BorderWidth = 2f,
                    Padding = 12,
                    BackgroundColor = new BaseColor(254, 242, 242),
                    MinimumHeight = 50f
                };
                diagTable.AddCell(diagContentCell);
                document.Add(diagTable);

                // ==================== TRATAMIENTO ====================
                var treatTable = new PdfPTable(1)
                {
                    WidthPercentage = 100,
                    SpacingBefore = 10,
                    SpacingAfter = 15
                };

                var treatHeaderCell = new PdfPCell(new Phrase("💊 Tratamiento Prescrito", sectionTitleFont))
                {
                    BackgroundColor = new BaseColor(139, 92, 246),
                    Padding = 10,
                    Border = Rectangle.NO_BORDER,
                    HorizontalAlignment = Element.ALIGN_LEFT
                };
                treatTable.AddCell(treatHeaderCell);

                var treatContentCell = new PdfPCell(new Phrase(appointment.MedicalRecord.Treatment ?? "Sin tratamiento prescrito", bodyFont))
                {
                    Border = Rectangle.BOX,
                    BorderColor = new BaseColor(229, 231, 235),
                    BorderWidth = 1f,
                    Padding = 12,
                    MinimumHeight = 60f
                };
                treatTable.AddCell(treatContentCell);
                document.Add(treatTable);

                // ==================== SERVICIOS ADICIONALES ====================
                if (appointment.MedicalRecord.AdditionalServices != null &&
                    appointment.MedicalRecord.AdditionalServices.Any())
                {
                    var servicesTitle = new Paragraph("🧪 Exámenes y Servicios Adicionales Solicitados", subsectionFont)
                    {
                        SpacingBefore = 15,
                        SpacingAfter = 10
                    };
                    document.Add(servicesTitle);

                    var servicesTable = new PdfPTable(5)
                    {
                        WidthPercentage = 100,
                        SpacingAfter = 15
                    };
                    servicesTable.SetWidths(new float[] { 10f, 30f, 35f, 12f, 13f });

                    // Headers
                    string[] headers = { "#", "Servicio", "Descripción", "Duración", "Estado" };
                    var headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10, BaseColor.WHITE);

                    foreach (var header in headers)
                    {
                        var cell = new PdfPCell(new Phrase(header, headerFont))
                        {
                            BackgroundColor = new BaseColor(71, 85, 105),
                            Padding = 8,
                            HorizontalAlignment = Element.ALIGN_CENTER,
                            VerticalAlignment = Element.ALIGN_MIDDLE
                        };
                        servicesTable.AddCell(cell);
                    }

                    // Datos
                    var rowFont = FontFactory.GetFont(FontFactory.HELVETICA, 9, new BaseColor(75, 75, 75));
                    int counter = 1;
                    decimal totalCost = 0;

                    foreach (var addService in appointment.MedicalRecord.AdditionalServices)
                    {
                        // Número
                        servicesTable.AddCell(new PdfPCell(new Phrase(counter.ToString(), rowFont))
                        {
                            Padding = 6,
                            HorizontalAlignment = Element.ALIGN_CENTER,
                            BackgroundColor = new BaseColor(248, 250, 252)
                        });

                        // Servicio
                        servicesTable.AddCell(new PdfPCell(new Phrase(addService.Service.NameService, rowFont))
                        {
                            Padding = 6,
                            BackgroundColor = BaseColor.WHITE
                        });

                        // Descripción
                        servicesTable.AddCell(new PdfPCell(new Phrase(addService.Service.Description ?? "-", rowFont))
                        {
                            Padding = 6,
                            BackgroundColor = BaseColor.WHITE
                        });

                        // Duración
                        servicesTable.AddCell(new PdfPCell(new Phrase($"{addService.Service.DurationMinutes} min", rowFont))
                        {
                            Padding = 6,
                            HorizontalAlignment = Element.ALIGN_CENTER,
                            BackgroundColor = BaseColor.WHITE
                        });

                        // Estado
                        string estadoTexto = addService.State switch
                        {
                            "P" => "Pendiente",
                            "A" => "Aplicado",
                            "X" => "Cancelado",
                            _ => "-"
                        };

                        BaseColor estadoColor = addService.State switch
                        {
                            "P" => new BaseColor(251, 191, 36),
                            "A" => new BaseColor(34, 197, 94),
                            "X" => new BaseColor(239, 68, 68),
                            _ => new BaseColor(156, 163, 175)
                        };

                        var estadoFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 8, BaseColor.WHITE);
                        servicesTable.AddCell(new PdfPCell(new Phrase(estadoTexto, estadoFont))
                        {
                            Padding = 6,
                            HorizontalAlignment = Element.ALIGN_CENTER,
                            BackgroundColor = estadoColor
                        });

                        totalCost += addService.Service.Price;
                        counter++;
                    }

                    document.Add(servicesTable);

                    // Total de servicios adicionales
                    var totalTable = new PdfPTable(2)
                    {
                        WidthPercentage = 100,
                        SpacingBefore = 5
                    };
                    totalTable.SetWidths(new float[] { 70f, 30f });

                    var totalLabelFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12, new BaseColor(51, 51, 51));
                    var totalValueFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 14, new BaseColor(220, 38, 38));

                    totalTable.AddCell(new PdfPCell(new Phrase("Costo Total de Servicios Adicionales:", totalLabelFont))
                    {
                        Border = Rectangle.NO_BORDER,
                        HorizontalAlignment = Element.ALIGN_RIGHT,
                        Padding = 8
                    });

                    totalTable.AddCell(new PdfPCell(new Phrase($"S/ {totalCost:0.00}", totalValueFont))
                    {
                        Border = Rectangle.NO_BORDER,
                        HorizontalAlignment = Element.ALIGN_CENTER,
                        Padding = 8,
                        BackgroundColor = new BaseColor(254, 242, 242)
                    });

                    document.Add(totalTable);
                }

                // ==================== CUADRO DE ADVERTENCIA ====================
                var warningBox = new PdfPTable(1)
                {
                    WidthPercentage = 100,
                    SpacingBefore = 25
                };

                var warningText = "⚠️ DOCUMENTO CONFIDENCIAL\n\n" +
                                 "Este documento contiene información médica confidencial protegida por ley. " +
                                 "Su divulgación no autorizada está prohibida. Solo el paciente y personal médico " +
                                 "autorizado pueden acceder a esta información.";

                var warningCell = new PdfPCell(new Phrase(warningText, footerFont))
                {
                    BackgroundColor = new BaseColor(254, 242, 242),
                    Border = Rectangle.BOX,
                    BorderColor = new BaseColor(220, 38, 38),
                    BorderWidth = 1.5f,
                    Padding = 12,
                    HorizontalAlignment = Element.ALIGN_JUSTIFIED
                };
                warningBox.AddCell(warningCell);
                document.Add(warningBox);

                // ==================== FIRMA DEL MÉDICO ====================
                document.Add(new Paragraph(" ") { SpacingBefore = 30 });

                var signatureTable = new PdfPTable(1)
                {
                    WidthPercentage = 50,
                    HorizontalAlignment = Element.ALIGN_CENTER
                };

                cb.SaveState();
                float signatureY = document.GetBottom(60);
                float signatureLineWidth = 200f;
                float signatureLineX = (pageWidth - signatureLineWidth) / 2;

                cb.SetLineWidth(1f);
                cb.SetColorStroke(BaseColor.BLACK);
                cb.MoveTo(signatureLineX, signatureY);
                cb.LineTo(signatureLineX + signatureLineWidth, signatureY);
                cb.Stroke();
                cb.RestoreState();

                var signatureText = new Paragraph(
                    $"Dr(a). {appointment.Doctor.FirstName} {appointment.Doctor.LastNamePat} {appointment.Doctor.LastNameMat}\n" +
                    $"{appointment.Specialty.NameSpecialty}",
                    bodyFont)
                {
                    Alignment = Element.ALIGN_CENTER,
                    SpacingBefore = 35
                };
                document.Add(signatureText);

                // ==================== PIE DE PÁGINA ====================
                var footer = new Paragraph(
                    $"Documento generado el: {DateTime.Now:dd/MM/yyyy HH:mm}\n" +
                    "SaludConnect - Sistema de Gestión Médica",
                    footerFont)
                {
                    SpacingBefore = 20,
                    Alignment = Element.ALIGN_CENTER
                };
                document.Add(footer);

                document.Close();

                var fileName = $"HistoriaClinica_{appointment.Patient.LastNamePat}_{appointment.DateAppointment:yyyyMMdd}_{appointment.MedicalRecord.IdRecord}.pdf";
                return File(memoryStream.ToArray(), "application/pdf", fileName);
            }
        }
    }
}
