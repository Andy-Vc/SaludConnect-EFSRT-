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

        [HttpGet]
        public async Task<IActionResult> BookingDoctor(int idSpecialty)
        {
            try
            {
                if (idSpecialty <= 0)
                {
                    TempData["Error"] = "Debe seleccionar una especialidad primero.";
                    return RedirectToAction(nameof(Booking));
                }

                var doctors = await _doctor.ListDoctorsWithExperience(idSpecialty);

                if (doctors == null || doctors.Count == 0)
                {
                    ViewBag.Message = "No hay doctores disponibles para esta especialidad.";
                    return View(new List<DoctorCard>());
                }

                ViewBag.IdSpecialty = idSpecialty;

                TempData["SelectedSpecialtyId"] = idSpecialty;

                return View(doctors);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error al cargar los doctores: " + ex.Message;
                return View(new List<DoctorCard>());
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
                    TempData["Error"] = "No se pudo obtener la información del doctor.";
                    return RedirectToAction(nameof(BookingDoctor), new { idSpecialty });
                }

                var availableDates = await _appointment.SearchAvailableDatesAppointments(idDoctor, idSpecialty);

                var viewModel = new BookingDateViewModel
                {
                    IdDoctor = idDoctor,
                    IdSpecialty = idSpecialty,
                    DoctorInfo = doctorInfo,
                    AvailableDates = availableDates ?? new List<AvailableDateAppointment>()
                };

                TempData["SelectedDoctorId"] = idDoctor;
                TempData["SelectedSpecialtyId"] = idSpecialty;

                return View(viewModel);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error al cargar las fechas disponibles: " + ex.Message;
                return RedirectToAction(nameof(BookingDoctor), new { idSpecialty });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetTimeSlots(int idDoctor, int idSpecialty, string date)
        {
            try
            {
                // 1. Validar y parsear la fecha
                if (!DateOnly.TryParse(date, out DateOnly selectedDate))
                {
                    return Json(new { success = false, message = "Fecha inválida" });
                }

                // 2. Llamar al servicio para obtener slots de tiempo
                var timeSlots = await _appointment.GetAvailableTimeSlots(idDoctor, idSpecialty, selectedDate);

                // 3. Retornar JSON para AJAX
                return Json(new { success = true, data = timeSlots });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }


        [HttpGet]
        public async Task<IActionResult> BookingSuccess(int idDoctor, int idSpecialty, string date, string time)
        {
            try
            {
                if (idDoctor <= 0 || idSpecialty <= 0 || string.IsNullOrEmpty(date) || string.IsNullOrEmpty(time))
                {
                    TempData["Error"] = "Faltan datos para confirmar la cita.";
                    return RedirectToAction(nameof(Booking));
                }

                if (!DateTime.TryParse($"{date} {time}", out DateTime appointmentDateTime))
                {
                    TempData["Error"] = "Fecha u hora inválida.";
                    return RedirectToAction(nameof(BookingDate), new { idDoctor, idSpecialty });
                }

                var user = HttpContext.Session.GetObjectFromJson<User>("User");
                if (user == null)
                {
                    return RedirectToAction("Login", "UserAuth");
                }

                var idUserPatient = user.IdUser;

                int idPatient = idUserPatient;

                var doctorInfo = await _doctor.GetDoctorInfo(idDoctor);

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
                ViewBag.Error = "Error al procesar la confirmación: " + ex.Message;
                return RedirectToAction(nameof(Booking));
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

            var totalApointments    = await _patient.CountAppointments(idUserPatient);
            var ApointmentsAssisted = await _patient.CountAppointmentsAssisted(idUserPatient);
            var ApointmentsCanceled = await _patient.CountAppointmentsCanceled(idUserPatient);
            var ApointmentsEarring  = await _patient.CountAppointmentsEarring(idUserPatient);

            var recordAppointments = await _patient.RecordAppointments(idUserPatient);

            ViewBag.TotalAppointments   = totalApointments;
            ViewBag.ApointmentsAssisted = ApointmentsAssisted;
            ViewBag.ApointmentsCanceled = ApointmentsCanceled;
            ViewBag.ApointmentsEarring  = ApointmentsEarring;
            ViewBag.ListAppointments = recordAppointments;

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> AppointmentsDetails(int id) 
        {
            var user = HttpContext.Session.GetObjectFromJson<User>("User");
            if (user == null)
            {
                return RedirectToAction("Login", "UserAuth");
            }

            var idUserPatient = user.IdUser;

            try
            {
                if (id <= 0)
                {
                    TempData["Error"] = "ID de cita inválido.";
                    return RedirectToAction("Index", "Patient");
                }

                var appointment = await _appointment.GetAppointmentByIdBooking(id);

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
        public async Task<IActionResult> ConfirmBooking(BookingConfirmViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View("BookingSuccess", model);
                }

                var validationRequest = new ValidateAppointmentRequest
                {
                    IdPatient = model.IdPatient,
                    IdDoctor = model.IdDoctor,
                    IdSpecialty = model.IdSpecialty,
                    DateAppointment = model.DateAppointment
                };

                var validation = await _appointment.ValidateAppointmentAvailability(validationRequest);

                if (validation == null || !validation.IsAvailable)
                {
                    TempData["Error"] = validation?.ErrorMessage ?? "No se pudo validar la disponibilidad.";
                    return RedirectToAction(nameof(BookingDate), new
                    {
                        idDoctor = model.IdDoctor,
                        idSpecialty = model.IdSpecialty
                    });
                }

                var createRequest = new CreateAppointmentRequest
                {
                    IdPatient = model.IdPatient,
                    IdDoctor = model.IdDoctor,
                    IdSpecialty = model.IdSpecialty,
                    DateAppointment = model.DateAppointment
                };

                var createdAppointment = await _appointment.CreateAppointment(createRequest);

                if (createdAppointment == null)
                {
                    TempData["Error"] = "No se pudo crear la cita. Intente nuevamente.";
                    return View("BookingSuccess", model);
                }

                TempData["Success"] = "¡Cita creada exitosamente!";
                TempData["AppointmentId"] = createdAppointment.IdAppointment;

                return RedirectToAction(nameof(AppointmentsDetails), new { id = createdAppointment.IdAppointment });
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al crear la cita: " + ex.Message;
                return View("BookingSuccess", model);
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
