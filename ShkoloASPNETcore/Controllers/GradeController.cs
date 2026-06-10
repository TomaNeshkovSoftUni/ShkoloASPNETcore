using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShkoloASPNETcore.Infrastructure.Data.Models;
using ShkoloASPNETcore.Services.Contracts;

namespace ShkoloASPNETcore.Web.Controllers
{
    [Authorize]
    public class GradeController : Controller
    {
        private readonly IGradeService _gradeService;

        public GradeController(IGradeService gradeService)
        {
            _gradeService = gradeService;
        }

        public async Task<IActionResult> Index()
        {
            var grades = await _gradeService.GetAllGradesAsync();
            return View(grades);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new Grade { DateIssued = DateTime.Now });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Grade grade)
        {
            ModelState.Remove("Student");
            ModelState.Remove("Subject");

            if (grade.DateIssued == default)
            {
                grade.DateIssued = DateTime.Now;
            }

            if (!ModelState.IsValid)
            {
                return View(grade);
            }

            await _gradeService.AddGradeAsync(grade);
            TempData["SuccessMessage"] = "Оценката е въведена успешно!";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var grade = await _gradeService.GetAllGradesAsync();
            var currentGrade = System.Linq.Enumerable.FirstOrDefault(grade, g => g.Id == id.Value);

            if (currentGrade == null) return NotFound();

            return View(currentGrade);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Grade grade)
        {
            if (id != grade.Id) return NotFound();

            ModelState.Remove("Student");
            ModelState.Remove("Subject");

            if (ModelState.IsValid)
            {
                await _gradeService.UpdateGradeAsync(grade);
                TempData["SuccessMessage"] = "Оценката е коригирана успешно!";
                return RedirectToAction(nameof(Index));
            }
            return View(grade);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var grades = await _gradeService.GetAllGradesAsync();
            var currentGrade = System.Linq.Enumerable.FirstOrDefault(grades, g => g.Id == id.Value);

            if (currentGrade == null) return NotFound();

            return View(currentGrade);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _gradeService.DeleteGradeAsync(id);
            TempData["SuccessMessage"] = "Оценката е изтрита успешно!";
            return RedirectToAction(nameof(Index));
        }
    }
}