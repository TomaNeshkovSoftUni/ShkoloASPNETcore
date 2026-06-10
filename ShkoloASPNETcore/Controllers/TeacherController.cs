using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShkoloASPNETcore.Infrastructure.Data.Models;
using ShkoloASPNETcore.Services.Contracts;

namespace ShkoloASPNETcore.Web.Controllers
{
    [Authorize(Roles = "Administrator")]
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

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var teacher = await _teacherService.GetTeacherByIdAsync(id.Value);
            if (teacher == null) return NotFound();

            return View(teacher);
        }

        public async Task<IActionResult> Create()
        {
            ViewData["ApplicationUserId"] = new SelectList(await _userManager.Users.ToListAsync(), "Id", "Id");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ApplicationUserId,Department")] Teacher teacher)
        {
            teacher.ApplicationUserId = "1";
            teacher.ApplicationUser = null;

            ModelState.Remove("ApplicationUser");
            ModelState.Remove("ApplicationUserId");

            if (ModelState.IsValid)
            {
                await _teacherService.AddTeacherAsync(teacher);
                return RedirectToAction(nameof(Index));
            }
            ViewData["ApplicationUserId"] = new SelectList(await _userManager.Users.ToListAsync(), "Id", "Id", teacher.ApplicationUserId);
            return View(teacher);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var teacher = await _teacherService.GetTeacherByIdAsync(id.Value);
            if (teacher == null) return NotFound();

            ViewData["ApplicationUserId"] = new SelectList(await _userManager.Users.ToListAsync(), "Id", "Id", teacher.ApplicationUserId);
            return View(teacher);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ApplicationUserId,Department")] Teacher teacher)
        {
            if (id != teacher.Id) return NotFound();

            teacher.ApplicationUser = null;
            ModelState.Remove("ApplicationUser");

            if (ModelState.IsValid)
            {
                try
                {
                    await _teacherService.UpdateTeacherAsync(teacher);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _teacherService.TeacherExistsAsync(teacher.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ApplicationUserId"] = new SelectList(await _userManager.Users.ToListAsync(), "Id", "Id", teacher.ApplicationUserId);
            return View(teacher);
        }

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
            return RedirectToAction(nameof(Index));
        }
    }
}