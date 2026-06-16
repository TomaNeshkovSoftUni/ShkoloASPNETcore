using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShkoloASPNETcore.Infrastructure.Data.Models;
using ShkoloASPNETcore.Services.Contracts;

namespace ShkoloASPNETcore.Web.Controllers
{
    [Authorize]
    public class GradeController : Controller
    {
        private readonly IGradeService _gradeService;
        private readonly IStudentService _studentService;
        private readonly ISubjectService _subjectService;
        private readonly UserManager<ApplicationUser> _userManager;

        public GradeController(
            IGradeService gradeService,
            IStudentService studentService,
            ISubjectService subjectService,
            UserManager<ApplicationUser> userManager)
        {
            _gradeService = gradeService;
            _studentService = studentService;
            _subjectService = subjectService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("Teacher") || User.IsInRole("Administrator"))
            {
                var allGrades = await _gradeService.GetAllGradesAsync();
                return View(allGrades);
            }

            var userId = _userManager.GetUserId(User);
            if (userId == null) return Challenge();

            var student = await _studentService.GetStudentByUserIdAsync(userId);

            if (student == null)
            {
                return View(new List<Grade>());
            }

            var studentGrades = await _gradeService.GetGradesByStudentIdAsync(student.Id);
            return View(studentGrades);
        }

        public async Task<IActionResult> Details(int id)
        {
            var grade = await _gradeService.GetGradeByIdAsync(id);
            if (grade == null)
            {
                return NotFound();
            }

            return View(grade);
        }

        [Authorize(Roles = "Teacher,Administrator")]
        public async Task<IActionResult> Create()
        {
            ViewBag.Students = new SelectList(await _studentService.GetAllStudentsAsync(), "Id", "LastName");
            ViewBag.Subjects = new SelectList(await _subjectService.GetAllSubjectsAsync(), "Id", "Name");
            return View(new Grade { DateIssued = DateTime.Now });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Teacher,Administrator")]
        public async Task<IActionResult> Create(Grade grade)
        {
            ModelState.Remove("Student");
            ModelState.Remove("Subject");
            ModelState.Remove("Issuer");

            if (ModelState.IsValid)
            {
                var currentUser = await _userManager.GetUserAsync(User);
                grade.Issuer = User.IsInRole("Administrator") ? "Admin" : $"{currentUser?.FirstName} {currentUser?.LastName}";
                grade.DateIssued = DateTime.Now;

                await _gradeService.AddGradeAsync(grade);
                TempData["SuccessMessage"] = "Оценката е добавена успешно!";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Students = new SelectList(await _studentService.GetAllStudentsAsync(), "Id", "LastName", grade.StudentId);
            ViewBag.Subjects = new SelectList(await _subjectService.GetAllSubjectsAsync(), "Id", "Name", grade.SubjectId);
            return View(grade);
        }

        [Authorize(Roles = "Teacher,Administrator")]
        public async Task<IActionResult> Edit(int id)
        {
            var grade = await _gradeService.GetGradeByIdAsync(id);
            if (grade == null)
            {
                return NotFound();
            }

            ViewBag.StudentId = new SelectList(await _studentService.GetAllStudentsAsync(), "Id", "LastName", grade.StudentId);
            ViewBag.SubjectId = new SelectList(await _subjectService.GetAllSubjectsAsync(), "Id", "Name", grade.SubjectId);
            return View(grade);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Teacher,Administrator")]
        public async Task<IActionResult> Edit(int id, Grade grade)
        {
            if (id != grade.Id)
            {
                return NotFound();
            }

            ModelState.Remove("Student");
            ModelState.Remove("Subject");
            ModelState.Remove("Issuer");

            if (ModelState.IsValid)
            {
                var existingGrade = await _gradeService.GetGradeByIdAsync(id);
                if (existingGrade == null) return NotFound();

                existingGrade.Value = grade.Value;

                await _gradeService.UpdateGradeAsync(existingGrade);
                TempData["SuccessMessage"] = "Оценката е коригирана успешно!";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.StudentId = new SelectList(await _studentService.GetAllStudentsAsync(), "Id", "LastName", grade.StudentId);
            ViewBag.SubjectId = new SelectList(await _subjectService.GetAllSubjectsAsync(), "Id", "Name", grade.SubjectId);
            return View(grade);
        }

        [HttpGet]
        [Authorize(Roles = "Teacher,Administrator")]
        public async Task<IActionResult> Delete(int id)
        {
            var grade = await _gradeService.GetGradeByIdAsync(id);
            if (grade == null)
            {
                return NotFound();
            }

            return View(grade);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Teacher,Administrator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _gradeService.DeleteGradeAsync(id);
            TempData["SuccessMessage"] = "Оценката е изтрита успешно!";
            return RedirectToAction(nameof(Index));
        }
    }
}