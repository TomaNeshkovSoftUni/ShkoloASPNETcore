using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShkoloASPNETcore.Infrastructure.Data.Models;
using ShkoloASPNETcore.Services.Contracts;
using ShkoloASPNETcore.Web.Models;

namespace ShkoloASPNETcore.Web.Controllers
{
    [Authorize] // Позволява на ВСИЧКИ логнати потребители (включително ученици) да влизат тук
    public class RemarkController : Controller
    {
        private readonly IRemarkService _remarkService;
        private readonly IStudentService _studentService;
        private readonly ISubjectService _subjectService;
        private readonly UserManager<ApplicationUser> _userManager;

        public RemarkController(
            IRemarkService remarkService,
            IStudentService studentService,
            ISubjectService subjectService,
            UserManager<ApplicationUser> userManager)
        {
            _remarkService = remarkService;
            _studentService = studentService;
            _subjectService = subjectService;
            _userManager = userManager;
        }

        // СИГУРЕН INDEX МЕТОД: Филтрира спрямо ролята
        public async Task<IActionResult> Index()
        {
            // 1. Ако потребителят е Учител или Администратор - вижда абсолютно всичко
            if (User.IsInRole("Teacher") || User.IsInRole("Administrator"))
            {
                var allRemarks = await _remarkService.GetAllRemarksAsync();
                return View(allRemarks);
            }

            // 2. Ако е Ученик - намираме неговия профил и му показваме само неговите отзиви
            var userId = _userManager.GetUserId(User);
            if (userId == null) return Challenge();

            var student = await _studentService.GetStudentByUserIdAsync(userId);
            if (student == null)
            {
                // Защита: Ако профилът на ученика още не е генериран в базата
                return View(new List<Remark>());
            }

            var studentRemarks = await _remarkService.GetRemarksByStudentIdAsync(student.Id);
            return View(studentRemarks);
        }

        [HttpGet]
        [Authorize(Roles = "Teacher,Administrator")] // Само те могат да създават
        public async Task<IActionResult> Create()
        {
            ViewBag.Students = new SelectList(await _studentService.GetAllStudentsAsync(), "Id", "LastName");
            ViewBag.Subjects = new SelectList(await _subjectService.GetAllSubjectsAsync(), "Id", "Name");
            return View(new RemarkFormViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Teacher,Administrator")]
        public async Task<IActionResult> Create(RemarkFormViewModel model)
        {
            if (ModelState.IsValid)
            {
                var currentUser = await _userManager.GetUserAsync(User);

                var remark = new Remark
                {
                    StudentId = model.StudentId,
                    SubjectId = model.SubjectId,
                    Type = model.Type,
                    Label = model.SelectedText,
                    Comment = model.Comment,
                    Issuer = $"{currentUser?.FirstName} {currentUser?.LastName}",
                    DateIssued = DateTime.Now
                };

                await _remarkService.AddRemarkAsync(remark);
                TempData["SuccessMessage"] = "Отзивът е добавен успешно!";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Students = new SelectList(await _studentService.GetAllStudentsAsync(), "Id", "LastName", model.StudentId);
            ViewBag.Subjects = new SelectList(await _subjectService.GetAllSubjectsAsync(), "Id", "Name", model.SubjectId);
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Teacher,Administrator")] // Само те могат да редактират
        public async Task<IActionResult> Edit(int id)
        {
            var remark = await _remarkService.GetRemarkByIdAsync(id);
            if (remark == null) return NotFound();

            var model = new RemarkFormViewModel
            {
                Id = remark.Id,
                StudentId = remark.StudentId,
                SubjectId = remark.SubjectId,
                Type = remark.Type,
                SelectedText = remark.Label,
                Comment = remark.Comment
            };

            ViewBag.Students = new SelectList(await _studentService.GetAllStudentsAsync(), "Id", "LastName", remark.StudentId);
            ViewBag.Subjects = new SelectList(await _subjectService.GetAllSubjectsAsync(), "Id", "Name", remark.SubjectId);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Teacher,Administrator")]
        public async Task<IActionResult> Edit(int id, RemarkFormViewModel model)
        {
            if (id != model.Id) return NotFound();

            if (ModelState.IsValid)
            {
                var existingRemark = await _remarkService.GetRemarkByIdAsync(id);
                if (existingRemark == null) return NotFound();

                existingRemark.Type = model.Type;
                existingRemark.SubjectId = model.SubjectId;
                existingRemark.Label = model.SelectedText;
                existingRemark.Comment = model.Comment;

                await _remarkService.UpdateRemarkAsync(existingRemark);
                TempData["SuccessMessage"] = "Отзивът е коригиран успешно!";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Students = new SelectList(await _studentService.GetAllStudentsAsync(), "Id", "LastName", model.StudentId);
            ViewBag.Subjects = new SelectList(await _subjectService.GetAllSubjectsAsync(), "Id", "Name", model.SubjectId);
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Teacher,Administrator")]
        public async Task<IActionResult> Delete(int id)
        {
            var remark = await _remarkService.GetRemarkByIdAsync(id);
            if (remark == null) return NotFound();

            return View(remark);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Teacher,Administrator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _remarkService.DeleteRemarkAsync(id);
            TempData["SuccessMessage"] = "Отзивът е изтрит успешно!";
            return RedirectToAction(nameof(Index));
        }
    }
}