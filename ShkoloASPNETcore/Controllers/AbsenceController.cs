using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShkoloASPNETcore.Core.Services.Contracts;
using ShkoloASPNETcore.Infrastructure.Data.Models;
using ShkoloASPNETcore.Services.Contracts;

namespace ShkoloASPNETcore.Web.Controllers
{
    [Authorize]
    public class AbsenceController : Controller
    {
        private readonly IAbsenceService _absenceService;
        private readonly IStudentService _studentService;
        private readonly ISubjectService _subjectService;
        private readonly UserManager<ApplicationUser> _userManager;

        public AbsenceController(
            IAbsenceService absenceService,
            IStudentService studentService,
            ISubjectService subjectService,
            UserManager<ApplicationUser> userManager)
        {
            _absenceService = absenceService;
            _studentService = studentService;
            _subjectService = subjectService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var allAbsences = await _absenceService.GetAllAbsencesAsync();

            if (currentUser == null) return Challenge();

            if (User.IsInRole("Student") || (!User.IsInRole("Teacher") && !User.IsInRole("Administrator")))
            {
                var studentAbsences = allAbsences.Where(a =>
                    a.Student != null &&
                    a.Student.FirstName == currentUser.FirstName &&
                    a.Student.LastName == currentUser.LastName).ToList();
                return View(studentAbsences);
            }

            return View(allAbsences);
        }

        [HttpGet]
        [Authorize(Roles = "Teacher,Administrator")]
        public async Task<IActionResult> Create()
        {
            ViewBag.Students = new SelectList(await _studentService.GetAllStudentsAsync(), "Id", "LastName");
            ViewBag.Subjects = new SelectList(await _subjectService.GetAllSubjectsAsync(), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Teacher,Administrator")]
        public async Task<IActionResult> Create(Absence absence)
        {
            ModelState.Remove("Student");
            ModelState.Remove("Subject");

            if (ModelState.IsValid)
            {
                await _absenceService.AddAbsenceAsync(absence);
                TempData["SuccessMessage"] = "Отсъствието е добавено успешно!";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Students = new SelectList(await _studentService.GetAllStudentsAsync(), "Id", "LastName", absence.StudentId);
            ViewBag.Subjects = new SelectList(await _subjectService.GetAllSubjectsAsync(), "Id", "Name", absence.SubjectId);
            return View(absence);
        }
    }
}