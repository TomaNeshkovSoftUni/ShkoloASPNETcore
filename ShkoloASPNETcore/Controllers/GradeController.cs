using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShkoloASPNETcore.Infrastructure.Data.Models;
using ShkoloASPNETcore.Services.Contracts;

namespace ShkoloASPNETcore.Web.Controllers
{
    public class GradeController : Controller
    {
        private readonly IGradeService _gradeService;
        private readonly IStudentService _studentService;
        private readonly ISubjectService _subjectService;

        public GradeController(
            IGradeService gradeService,
            IStudentService studentService,
            ISubjectService subjectService)
        {
            _gradeService = gradeService;
            _studentService = studentService;
            _subjectService = subjectService;
        }

        public async Task<IActionResult> Index()
        {
            var grades = await _gradeService.GetAllGradesAsync();
            return View(grades);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var grade = await _gradeService.GetGradeByIdAsync(id.Value);
            if (grade == null) return NotFound();

            return View(grade);
        }

        public async Task<IActionResult> Create()
        {
            ViewData["StudentId"] = new SelectList(await _studentService.GetAllStudentsAsync(), "Id", "Id");
            ViewData["SubjectId"] = new SelectList(await _subjectService.GetAllSubjectsAsync(), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Grade grade)
        {
            ModelState.Remove("Student");
            ModelState.Remove("Subject");

            if (ModelState.IsValid)
            {
                await _gradeService.AddGradeAsync(grade);
                return RedirectToAction(nameof(Index));
            }
            ViewData["StudentId"] = new SelectList(await _studentService.GetAllStudentsAsync(), "Id", "Id", grade.StudentId);
            ViewData["SubjectId"] = new SelectList(await _subjectService.GetAllSubjectsAsync(), "Id", "Name", grade.SubjectId);
            return View(grade);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var grade = await _gradeService.GetGradeByIdAsync(id.Value);
            if (grade == null) return NotFound();

            ViewData["StudentId"] = new SelectList(await _studentService.GetAllStudentsAsync(), "Id", "Id", grade.StudentId);
            ViewData["SubjectId"] = new SelectList(await _subjectService.GetAllSubjectsAsync(), "Id", "Name", grade.SubjectId);
            return View(grade);
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
                try
                {
                    await _gradeService.UpdateGradeAsync(grade);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _gradeService.GradeExistsAsync(grade.Id))
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
            ViewData["StudentId"] = new SelectList(await _studentService.GetAllStudentsAsync(), "Id", "Id", grade.StudentId);
            ViewData["SubjectId"] = new SelectList(await _subjectService.GetAllSubjectsAsync(), "Id", "Name", grade.SubjectId);
            return View(grade);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var grade = await _gradeService.GetGradeByIdAsync(id.Value);
            if (grade == null) return NotFound();

            return View(grade);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _gradeService.DeleteGradeAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}