using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShkoloASPNETcore.Infrastructure.Data.Models;
using ShkoloASPNETcore.Services.Contracts;

namespace ShkoloASPNETcore.Web.Controllers
{
    [Authorize]
    public class StudentController : Controller
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        public async Task<IActionResult> Index()
        {
            var students = await _studentService.GetAllStudentsAsync();
            return View(students);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Student student)
        {
            student.EnrollmentNumber = "УЧ-" + Guid.NewGuid().ToString().Substring(0, 8).ToUpper();

            ModelState.Remove("Grades");
            ModelState.Remove("ApplicationUser");
            ModelState.Remove("ApplicationUserId");
            ModelState.Remove("EnrollmentNumber");

            if (!ModelState.IsValid)
            {
                return View(student);
            }

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _studentService.AddStudentAsync(student, currentUserId);
            TempData["SuccessMessage"] = "Ученикът е добавен успешно!";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var student = await _studentService.GetStudentByIdAsync(id.Value);
            if (student == null) return NotFound();

            return View(student);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Student student)
        {
            if (id != student.Id) return NotFound();

            student.ApplicationUser = null;
            ModelState.Remove("ApplicationUser");

            if (ModelState.IsValid)
            {
                await _studentService.UpdateStudentAsync(student);
                TempData["SuccessMessage"] = "Данните на ученика бяха обновени успешно!";
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var student = await _studentService.GetStudentByIdAsync(id.Value);
            if (student == null) return NotFound();

            return View(student);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _studentService.DeleteStudentAsync(id);
            TempData["SuccessMessage"] = "Ученикът беше изтрит успешно!";
            return RedirectToAction(nameof(Index));
        }
    }
}