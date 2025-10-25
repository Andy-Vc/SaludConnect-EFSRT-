using Microsoft.AspNetCore.Mvc;
using Web.Extensions;
using Web.Models;
using Web.Models.DTO;
using Web.Services.Implementation;
using Web.Services.Interface;

namespace Web.Controllers
{
    public class ConsultoryController : Controller
    {

        private readonly IConsultory consultory;
        private readonly IUser user;
        private readonly ISpecialty specialty;


        public ConsultoryController(IConsultory consultory, IUser user, ISpecialty specialty)
        {
            this.consultory = consultory;
            this.user = user;
            this.specialty = specialty;
        }

        //CRUD

        public async Task<IActionResult> Index()
        {
            var user = HttpContext.Session.GetObjectFromJson<User>("User");
            if (user == null || user.Role.IdRole != 2)
            {
                return RedirectToAction("Login", "UserAuth");
            }

            var consultories = await consultory.ListConsultories();
            return View(consultories);
        }


        public async Task<IActionResult> Details(int id)
        {
            var consultories = await consultory.GetConsultoryById(id);
            if (consultories == null)
            {
                TempData["Error"] = "Consultorio no encontrado";
                return RedirectToAction(nameof(Index));
            }
            return View(consultories);
        }

        public async Task<IActionResult> Create()
        {
            var specialties = await specialty.ListSpecialties();
            ViewBag.Specialties = specialties;
            return View(new CreateConsultoryDTO());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateConsultoryDTO consultories)
        {
            if (!ModelState.IsValid)
            {
                return View(consultory);
            }
            

            var result = await consultory.CreateConsultory(consultories);

            if (result != null)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(consultories);
        }



        public async Task<IActionResult> Edit(int id)
        {
            var specialties = await specialty.ListSpecialties();
            ViewBag.Specialties = specialties;
            var consultories = await consultory.GetConsultoryById(id);
            if (consultories == null)
            {
                TempData["Error"] = "Consultorio no encontrado";
                return RedirectToAction(nameof(Index));
            }

            var updateDto = new UpdateConsultoryDTO
            {
                IdConsultories = consultories.IdConsultories,
                NumberConsultories = consultories.NumberConsultories,
                FloorNumber = consultories.FloorNumber,
                IdSpecialty = consultories.IdSpecialty,
                
            };

            return View(updateDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateConsultoryDTO consultories)
        {
            if (id != consultories.IdConsultories)
            {
                TempData["Error"] = "El ID no coincide";
                return RedirectToAction(nameof(Index));
            }

            if (!ModelState.IsValid)
            {
                return View(consultories);
            }

            var success = await consultory.UpdateConsultory(id, consultories);
            if (success)
            {
                TempData["Success"] = "Doctor actualizado exitosamente";
                return RedirectToAction(nameof(Index));
            }

            TempData["Error"] = "Error al actualizar doctor";
            return View(consultories);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await consultory.DeleteConsultory(id);

            if (success)
            {
                TempData["Success"] = "Consultorio eliminado exitosamente";
            }
            else
            {
                TempData["Error"] = "Error al eliminar consultorio";
            }

            return RedirectToAction(nameof(Index));
        }

    }
}
