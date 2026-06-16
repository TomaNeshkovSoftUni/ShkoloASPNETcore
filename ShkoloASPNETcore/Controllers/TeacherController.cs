using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShkoloASPNETcore.Infrastructure.Data.Models;
using ShkoloASPNETcore.Services.Contracts;

namespace ShkoloASPNETcore.Web.Controllers
{
    [Authorize]
    public class TeacherController : Controller
    {
        private readonly ITeacherService _teacherService;
        private readonly UserManager<ApplicationUser> _userManager;

        public TeacherController(ITeacherService teacherService, UserManager<ApplicationUser> userManager)
        {
            _teacherService = teacherService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var teachers = await _teacherService.GetAllTeachersAsync();
            return View(teachers);
        }

        public async Task<IActionResult> Details(int id)
        {
            var teacher = await _teacherService.GetTeacherByIdAsync(id);
            if (teacher == null) return NotFound();

            return View(teacher);
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var teacher = await _teacherService.GetTeacherByIdAsync(id);
            if (teacher == null) return NotFound();

            var currentUserId = _userManager.GetUserId(User);
            if (teacher.ApplicationUserId == currentUserId)
            {
                TempData["ErrorMessage"] = "Не можете да изтриете собствения си профил!";
                return RedirectToAction(nameof(Index));
            }

            return View(teacher);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var teacher = await _teacherService.GetTeacherByIdAsync(id);
            if (teacher == null) return NotFound();

            var currentUserId = _userManager.GetUserId(User);
            if (teacher.ApplicationUserId == currentUserId)
            {
                TempData["ErrorMessage"] = "Опит за изтриване на собствен профил е блокиран!";
                return RedirectToAction(nameof(Index));
            }

            await _teacherService.DeleteTeacherAsync(id);
            TempData["SuccessMessage"] = "Учителят беше изтрит от системата!";
            return RedirectToAction(nameof(Index));
        }
    }
}