using Microsoft.AspNetCore.Mvc;
using Models;
using Web.Extensions;
using Web.Models;
using Web.Models.DTO;
using Web.Services.Implementation;
using Web.Services.Interface;
using User = Web.Models.User;

namespace Web.Controllers
{
    public class ServiceController : Controller
    {
        private readonly IService _serviceService;
        private readonly ISpecialty _speciality;

        public ServiceController(IService serviceService, ISpecialty speciality)
        {
            _serviceService = serviceService;
            _speciality = speciality;
        }
        public async Task<IActionResult> Index()
        {
            var user = HttpContext.Session.GetObjectFromJson<User>("User");
            if (user == null || user.Role.IdRole != 2)
            {
                return RedirectToAction("Login", "UserAuth");
            }

            var services = await _serviceService.ListService();
            return View(services);
        }

        // GET: Service/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var service = await _serviceService.GetServiceById(id);
            if (service == null)
            {
                TempData["Error"] = "Servicio no encontrado";
                return RedirectToAction(nameof(Index));
            }
            return View(service);
        }

        // GET: Service/Create
        public async Task<IActionResult> CreateAsync()

        {
            var specialties = await _speciality.ListSpecialties();
            ViewBag.Specialties = specialties;
            return View(new CreateServiceDTO());
        }

        // POST: Service/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateServiceDTO service)
        {
            if (!ModelState.IsValid)
            {
                return View(service);
            }

            var result = await _serviceService.CreateService(service);
            if (result > 0)
            {
                TempData["Success"] = "Servicio creado exitosamente";
                return RedirectToAction(nameof(Index));
            }

            TempData["Error"] = "Error al crear servicio";
            return View(service);
        }

        // GET: Service/Edit/5
        public async Task<IActionResult> Edit(int id)

        {
            var specialties = await _speciality.ListSpecialties();
            ViewBag.Specialties = specialties;
            var service = await _serviceService.GetServiceById(id);
            if (service == null)
            {
                TempData["Error"] = "Servicio no encontrado";
                return RedirectToAction(nameof(Index));
            }

            var updateDto = new UpdateServiceDTO
            {
                IdService = service.IdService,
                NameService = service.NameService,
                Description = service.Description,
                Price = service.Price,
                DurationMinutes = service.DurationMinutes,
                IdSpecialty = service.IdSpecialty
            };

            return View(updateDto);
        }

        // POST: Service/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateServiceDTO service)
        {
            if (id != service.IdService)
            {
                TempData["Error"] = "El ID no coincide";
                return RedirectToAction(nameof(Index));
            }

            if (!ModelState.IsValid)
            {
                return View(service);
            }

            var success = await _serviceService.UpdateService(id, service);
            if (success)
            {
                TempData["Success"] = "Servicio actualizado exitosamente";
                return RedirectToAction(nameof(Index));
            }

            TempData["Error"] = "Error al actualizar servicio";
            return View(service);
        }

        // POST: Service/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _serviceService.DeleteService(id);
            if (success)
            {
                TempData["Success"] = "Servicio eliminado exitosamente";
            }
            else
            {
                TempData["Error"] = "Error al eliminar servicio";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
