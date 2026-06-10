using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShkoloASPNETcore.Core.Services.Contracts;
using ShkoloASPNETcore.Infrastructure.Data.Models;
using ShkoloASPNETcore.Services.Contracts;

namespace ShkoloASPNETcore.Web.Controllers
{
    public class AbsenceController : Controller
    {
        private readonly IAbsenceService _absenceService;
        private readonly IStudentService _studentService;
        private readonly ISubjectService _subjectService;

        public AbsenceController(
            IAbsenceService absenceService,
            IStudentService studentService,
            ISubjectService subjectService)
        {
            _absenceService = absenceService;
            _studentService = studentService;
            _subjectService = subjectService;
        }

        public async Task<IActionResult> Index()
        {
            var absences = await _absenceService.GetAllAbsencesAsync();
            return View(absences);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Students = new SelectList(await _studentService.GetAllStudentsAsync(), "Id", "Id");
            ViewBag.Subjects = new SelectList(await _subjectService.GetAllSubjectsAsync(), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Absence absence)
        {
            ModelState.Remove("Student");
            ModelState.Remove("Subject");

            if (ModelState.IsValid)
            {
                await _absenceService.AddAbsenceAsync(absence);
                TempData["SuccessMessage"] = "Отсъствието/Закъснението е добавено успешно!";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Students = new SelectList(await _studentService.GetAllStudentsAsync(), "Id", "Id", absence.StudentId);
            ViewBag.Subjects = new SelectList(await _subjectService.GetAllSubjectsAsync(), "Id", "Name", absence.SubjectId);
            return View(absence);
        }
    }
}