using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShkoloASPNETcore.Infrastructure.Data.Models;
using ShkoloASPNETcore.Services.Contracts;

namespace ShkoloASPNETcore.Web.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class TeacherController : Controller
    {
        private readonly ITeacherService _teacherService;

        public TeacherController(ITeacherService teacherService)
        {
            _teacherService = teacherService;
        }

        public async Task<IActionResult> Index()
        {
            var teachers = await _teacherService.GetAllTeachersAsync();
            return View(teachers);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Teacher teacher)
        {
            ModelState.Remove("Subjects");
            ModelState.Remove("ApplicationUser");

            if (!ModelState.IsValid)
            {
                return View(teacher);
            }

            await _teacherService.AddTeacherAsync(teacher, teacher.ApplicationUserId);
            TempData["SuccessMessage"] = "Учителят е добавен успешно!";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var teacher = await _teacherService.GetTeacherByIdAsync(id.Value);
            if (teacher == null) return NotFound();

            return View(teacher);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Teacher teacher)
        {
            if (id != teacher.Id) return NotFound();

            teacher.ApplicationUser = null;
            ModelState.Remove("ApplicationUser");
            ModelState.Remove("Subjects");

            if (ModelState.IsValid)
            {
                await _teacherService.UpdateTeacherAsync(teacher);
                TempData["SuccessMessage"] = "Данните на учителя бяха обновени успешно!";
                return RedirectToAction(nameof(Index));
            }
            return View(teacher);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var teacher = await _teacherService.GetTeacherByIdAsync(id.Value);
            if (teacher == null) return NotFound();

            return View(teacher);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _teacherService.DeleteTeacherAsync(id);
            TempData["SuccessMessage"] = "Учителят беше изтрит успешно!";
            return RedirectToAction(nameof(Index));
        }
    }
}