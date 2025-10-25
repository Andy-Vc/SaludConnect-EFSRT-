using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Web.Extensions;
using Web.Models;
using Web.Models.DTO;
using Web.Models.ViewModels.PatientVM;
using Web.Models.ViewModels.PatientWM;
using Web.Services.Implementation;
using Web.Services.Interface;

namespace Web.Controllers
{
    public class PatientController : Controller
    {

        private readonly IPatient _patient;
        private readonly IDoctor _doctor;
        private readonly IAppointment _appointment;
        private readonly ISpecialty _specialty;


        public PatientController(IPatient inyec, IDoctor doctor, IAppointment appointment, ISpecialty specialty)
        {
            this._patient = inyec;
            this._doctor = doctor;
            this._appointment = appointment;
            this._specialty = specialty;
        }

        public async Task<IActionResult> Index() 
        {
            var user = HttpContext.Session.GetObjectFromJson<User>("User"); //Obtener al usuario logeado
            if (user == null) 
            {
                return RedirectToAction("Login", "UserAuth");
            } //validacion

            var idUserPatient = user.IdUser;

            var totalApointments    = await _patient.CountAppointments(idUserPatient);
            var ApointmentsAssisted = await _patient.CountAppointmentsAssisted(idUserPatient);
            var ApointmentsCanceled = await _patient.CountAppointmentsCanceled(idUserPatient);
            var ApointmentsEarring  = await _patient.CountAppointmentsEarring(idUserPatient);

            var nextAppointments    = await _patient.PatientNextAppointement(idUserPatient);

            ViewBag.TotalAppointments   = totalApointments;
            ViewBag.ApointmentsAssisted = ApointmentsAssisted;
            ViewBag.ApointmentsCanceled = ApointmentsCanceled;
            ViewBag.ApointmentsEarring  = ApointmentsEarring;
            ViewBag.NextAppointments = nextAppointments;


            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Booking() 
        {
            var user = HttpContext.Session.GetObjectFromJson<User>("User");
            if (user == null)
            {
                return RedirectToAction("Login", "UserAuth");
            }

            var idUserPatient = user.IdUser;

            try
            {
                var specialties = await _specialty.ListSpecialtiesWithDescription();

                if (specialties == null || specialties.Count == 0)
                {
                    ViewBag.Message = "No hay especialidades disponibles en este momento.";
                    return View(new List<SpecialtyViewModel>());
                }

                ViewBag.Especialidades = specialties;
                return View(specialties);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error al cargar las especialidades: " + ex.Message;
                return View(new List<Specialty>());
            }
        }


        //LLamado a especialidades ajax
        [HttpGet]
        public async Task<IActionResult> GetSpecialtiesPartial()
        {
            try
            {
                var specialties = await _specialty.ListSpecialtiesWithDescription();

                if (specialties == null)
                {
                    return PartialView("_SpecialtiesListPartial", new List<SpecialtyViewModel>());
                }

                return PartialView("_SpecialtiesListPartial", specialties);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error al cargar la lista de especialidades: " + ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> BookingDoctor(int idSpecialty)
        {
            if (idSpecialty <= 0)
            {
                return RedirectToAction("Booking");
            }

            try
            {
                var doctors = await _doctor.ListDoctorsWithExperience(idSpecialty);

                ViewBag.IdSpecialty = idSpecialty;
                return View(doctors);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Error al cargar la lista de doctores.");
                return View(new List<Web.Models.ViewModels.PatientVM.DoctorCard>());
            }
        }

        //Lllamado a doctores x especialidad
        [HttpGet]
        public async Task<IActionResult> GetDoctorsPartial(int idSpecialty)
        {
            try
            {
                if (idSpecialty <= 0)
                {
                    return StatusCode(400, "Debe seleccionar una especialidad válida.");
                }

                var doctors = await _doctor.ListDoctorsWithExperience(idSpecialty);

                return PartialView("_BookingDoctorPartial", doctors);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error al cargar los doctores: " + ex.Message);
            }
        }


        [HttpGet]
        public async Task<IActionResult> BookingDate(int idDoctor, int idSpecialty)
        {
            try
            {
                var doctorInfo = await _doctor.GetDoctorInfo(idDoctor);

                if (doctorInfo == null)
                {
                    TempData["ErrorMessage"] = "El doctor seleccionado no fue encontrado.";
                    return RedirectToAction("BookingDoctor", new { idSpecialty = idSpecialty });
                }

                var availableDates = await _appointment.SearchAvailableDatesAppointments(idDoctor, idSpecialty);

                Console.WriteLine($"Fechas disponibles obtenidas: {availableDates?.Count ?? 0}");

                var viewModel = new BookingDateViewModel
                {
                    IdDoctor = idDoctor,
                    IdSpecialty = idSpecialty,
                    DoctorInfo = doctorInfo,
                    AvailableDates = availableDates ?? new List<AvailableDateAppointment>()
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error al cargar la página de agendamiento: " + ex.Message;
                return RedirectToAction("Index");
            }
        }


        [HttpGet]
        public async Task<IActionResult> GetTimeSlots(int idDoctor, int idSpecialty, string date)
        {
            try
            {
                if (!DateOnly.TryParse(date, out DateOnly selectedDate))
                {
                    return Json(new { success = false, message = "Fecha inválida" });
                }

                Console.WriteLine($"Solicitando slots para: Doctor={idDoctor}, Especialidad={idSpecialty}, Fecha={date}");

                var timeSlots = await _appointment.GetAvailableTimeSlots(idDoctor, idSpecialty, selectedDate);

                Console.WriteLine($"Slots obtenidos: {timeSlots?.Count ?? 0}");

                return Json(new { success = true, data = timeSlots });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en GetTimeSlots: {ex.Message}");
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetBookingSummary(int idDoctor, int idSpecialty, string date, string time)
        {
            try
            {
                if (idDoctor <= 0 || idSpecialty <= 0 || string.IsNullOrEmpty(date) || string.IsNullOrEmpty(time))
                {
                    return Json(new { success = false, message = "Faltan datos para generar el resumen." });
                }

                if (!DateTime.TryParse($"{date} {time}", out DateTime appointmentDateTime))
                {
                    return Json(new { success = false, message = "Fecha u hora inválida." });
                }

                var user = HttpContext.Session.GetObjectFromJson<User>("User");
                if (user == null)
                {
                    return Json(new { success = false, redirect = Url.Action("Login", "UserAuth") });
                }

                var idUserPatient = user.IdUser;
                int idPatient = idUserPatient;

                var doctorInfo = await _doctor.GetDoctorInfo(idDoctor);

                return Json(new
                {
                    success = true,
                    data = new
                    {
                        IdPatient = idPatient,
                        IdDoctor = idDoctor,
                        IdSpecialty = idSpecialty,
                        DateAppointment = appointmentDateTime,
                        DoctorName = doctorInfo?.fullNameDoc,
                        SpecialtyName = doctorInfo?.speciality?.NameSpecialty,
                        DoctorPhoto = doctorInfo?.imgProfile,
                        Date = date,
                        Time = time
                    }
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al generar el resumen: " + ex.Message });
            }
        }


        [HttpGet]
        public async Task<IActionResult> BookingSuccess(int idDoctor, int idSpecialty, string date, string time)
        {
            try
            {
                if (idDoctor <= 0 || idSpecialty <= 0 || string.IsNullOrEmpty(date) || string.IsNullOrEmpty(time))
                {
                    TempData["ErrorMessage"] = "Faltan datos para confirmar la cita.";
                    return RedirectToAction("BookingDate", new { idDoctor, idSpecialty });
                }

                if (!DateTime.TryParse($"{date} {time}", out DateTime appointmentDateTime))
                {
                    TempData["ErrorMessage"] = "Fecha u hora inválida.";
                    return RedirectToAction("BookingDate", new { idDoctor, idSpecialty });
                }

                var user = HttpContext.Session.GetObjectFromJson<User>("User");
                if (user == null)
                {
                    return RedirectToAction("Login", "UserAuth");
                }

                var idUserPatient = user.IdUser;
                int idPatient = idUserPatient;

                var doctorInfo = await _doctor.GetDoctorInfo(idDoctor);
                if (doctorInfo == null)
                {
                    TempData["ErrorMessage"] = "No se encontró información del doctor.";
                    return RedirectToAction("BookingDate", new { idDoctor, idSpecialty });
                }

                var viewModel = new BookingConfirmViewModel
                {
                    IdPatient = idPatient,
                    IdDoctor = idDoctor,
                    IdSpecialty = idSpecialty,
                    DateAppointment = appointmentDateTime,
                    DoctorInfo = doctorInfo,
                    Date = date,
                    Time = time
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error al cargar la confirmación: " + ex.Message;
                return RedirectToAction("BookingDate", new { idDoctor, idSpecialty });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmBookingAjax([FromBody] BookingConfirmViewModel model)
        {
            try
            {
                Console.WriteLine($"Received from JS: Patient={model.IdPatient}, Doctor={model.IdDoctor}, Specialty={model.IdSpecialty}, Date={model.DateAppointment}");

                var createRequest = new CreateAppointmentRequest
                {
                    IdPatient = model.IdPatient,
                    IdDoctor = model.IdDoctor,
                    IdSpecialty = model.IdSpecialty,
                    DateAppointment = model.DateAppointment
                };

                Console.WriteLine($"Calling CreateAppointment...");
                var createdAppointment = await _appointment.CreateAppointment(createRequest);

                Console.WriteLine($"Create appointment result: {(createdAppointment != null ? $"SUCCESS - ID: {createdAppointment.IdAppointment}" : "FAILED - NULL")}");

                if (createdAppointment == null)
                {
                    return Json(new
                    {
                        success = false,
                        message = "No se pudo crear la cita. Intente nuevamente."
                    });
                }

                Console.WriteLine($"=== APPOINTMENT CREATED SUCCESSFULLY - ID: {createdAppointment.IdAppointment} ===");
                return Json(new
                {
                    success = true,
                    message = "¡Cita creada exitosamente!",
                    appointmentId = createdAppointment.IdAppointment
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"=== ERROR IN CONFIRM BOOKING: {ex.Message} ===");
                return Json(new
                {
                    success = false,
                    message = "Error al crear la cita: " + ex.Message
                });
            }
        }


        public async Task<IActionResult> Record()
        {
            var user = HttpContext.Session.GetObjectFromJson<User>("User"); //Obtener al usuario logeado
            if (user == null)
            {
                return RedirectToAction("Login", "UserAuth");
            } //validacion

            var idUserPatient = user.IdUser;

            var totalApointments = await _patient.CountAppointments(idUserPatient);
            var ApointmentsAssisted = await _patient.CountAppointmentsAssisted(idUserPatient);
            var ApointmentsCanceled = await _patient.CountAppointmentsCanceled(idUserPatient);
            var ApointmentsEarring = await _patient.CountAppointmentsEarring(idUserPatient);

            var recordAppointments = await _patient.RecordAppointments(idUserPatient);

            ViewBag.TotalAppointments = totalApointments;
            ViewBag.ApointmentsAssisted = ApointmentsAssisted;
            ViewBag.ApointmentsCanceled = ApointmentsCanceled;
            ViewBag.ApointmentsEarring = ApointmentsEarring;
            ViewBag.ListAppointments = recordAppointments;

            return View();
        }


        [HttpGet]
        public async Task<IActionResult> AppointmentsDetails(int idAppointment)
        {
            var user = HttpContext.Session.GetObjectFromJson<User>("User");
            if (user == null)
            {
                return RedirectToAction("Login", "UserAuth");
            }

            var idUserPatient = user.IdUser;

            try
            {
                if (idAppointment <= 0)
                {
                    TempData["Error"] = "ID de cita inválido.";
                    return RedirectToAction("Index", "Patient");
                }

                var appointment = await _appointment.GetAppointmentByIdBooking(idAppointment);

                if (appointment == null)
                {
                    TempData["Error"] = "No se encontró la cita solicitada.";
                    return RedirectToAction("Index", "Patient");
                }

                int currentUserId = idUserPatient;
                if (appointment.Patient.IdUser != currentUserId)
                {
                    TempData["Error"] = "No tienes permiso para ver esta cita.";
                    return RedirectToAction("Index", "Patient");
                }

                if (TempData["Success"] != null)
                {
                    ViewBag.SuccessMessage = TempData["Success"];
                }

                return View(appointment);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error al cargar los detalles: " + ex.Message;
                return RedirectToAction("Index", "Patient");
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelAppointment(int id)
        {
            try
            {

                if (id <= 0)
                {
                    return Json(new { success = false, message = "ID de cita inválido" });
                }

                var appointment = await _appointment.GetAppointmentByIdBooking(id);
                Console.WriteLine($"Cita encontrada: {appointment != null}");

                if (appointment == null)
                {
                    return Json(new { success = false, message = "Cita no encontrada" });
                }

                var user = HttpContext.Session.GetObjectFromJson<User>("User");
                if (user == null)
                {
                    return RedirectToAction("Login", "UserAuth");
                }

                var idUserPatient = user.IdUser;
                Console.WriteLine($"Usuario actual: {idUserPatient}, Paciente de la cita: {appointment.Patient?.IdUser}");

                int currentUserId = idUserPatient;
                if (appointment.Patient.IdUser != currentUserId)
                {
                    return Json(new { success = false, message = "No tienes permiso para cancelar esta cita" });
                }

                var cancelledAppointment = await _appointment.ChangeStateAppointmentToCancel(id);

                if (cancelledAppointment != null)
                {
                    return Json(new
                    {
                        success = true,
                        message = "Cita cancelada exitosamente",
                        redirectUrl = Url.Action("Record", "Patient")
                    });
                }

                return Json(new { success = false, message = "No se pudo cancelar la cita" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }

        public IActionResult Help() 
        {
            return View();
        }
        
        [HttpGet]
        public async Task<List<RelationShip>> CompletListRelations()
        {
            var Lista = await _patient.CompletListOfRelationShips();
            return Lista;
        }
        public async Task<IActionResult> Profile() 
        {
            var user = HttpContext.Session.GetObjectFromJson<User>("User"); 
            if (user == null)
            {
                return RedirectToAction("Login", "UserAuth");
            }

            //lista
            var listRelations = await CompletListRelations();
            ViewBag.ListRelations = listRelations;


            var idUserPatient = user.IdUser;
            var patient = await _patient.PatientInformation(idUserPatient);
            
            return View(patient);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateInformationPatient(PatientUpdate updatePatient, IFormFile? photoFile, string? currentImage) 
        {
            try
            {
                Stream? photoStream = null;
                string? fileName = null;

                if (photoFile != null) { 
                    photoStream = photoFile.OpenReadStream();
                    fileName = photoFile.FileName;
                }

                var updateInformation = await _patient.UpdateInformationPatient(updatePatient, photoStream, fileName);
                if (photoFile == null) {
                    updateInformation.imageProfile = currentImage;
                }

               TempData["SuccesUpdate"] = ($"The patient with ID {updatePatient.idUser} was updated successfully");

                return RedirectToAction("Profile");
            }
            catch (Exception ex) 
            { 
                System.Diagnostics.Debug.WriteLine("Error: " + ex);
                return RedirectToAction("Profile");
            }
        }
        public IActionResult Notifications() 
        {
            return View();
        }
        
    }
}
