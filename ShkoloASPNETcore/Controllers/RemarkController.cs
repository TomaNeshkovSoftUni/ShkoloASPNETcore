using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShkoloASPNETcore.Infrastructure.Data.Models;
using ShkoloASPNETcore.Services.Contracts;

namespace ShkoloASPNETcore.Web.Controllers
{
    [Authorize]
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

        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var allRemarks = await _remarkService.GetAllRemarksAsync();

            if (currentUser == null) return Challenge();

            if (User.IsInRole("Student") || (!User.IsInRole("Teacher") && !User.IsInRole("Administrator")))
            {
                var studentRemarks = allRemarks.Where(r =>
                    r.Student != null &&
                    r.Student.FirstName == currentUser.FirstName &&
                    r.Student.LastName == currentUser.LastName).ToList();
                return View(studentRemarks);
            }

            return View(allRemarks);
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
        public async Task<IActionResult> Create(Remark remark)
        {
            ModelState.Remove("Student");
            ModelState.Remove("Subject");

            if (ModelState.IsValid)
            {
                await _remarkService.AddRemarkAsync(remark);
                TempData["SuccessMessage"] = "Забележката е добавена успешно!";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Students = new SelectList(await _studentService.GetAllStudentsAsync(), "Id", "LastName", remark.StudentId);
            ViewBag.Subjects = new SelectList(await _subjectService.GetAllSubjectsAsync(), "Id", "Name", remark.SubjectId);
            return View(remark);
        }
    }
}