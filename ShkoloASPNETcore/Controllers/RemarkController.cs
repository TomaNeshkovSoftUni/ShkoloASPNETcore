using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShkoloASPNETcore.Infrastructure.Data.Models;
using ShkoloASPNETcore.Services.Contracts;

namespace ShkoloASPNETcore.Web.Controllers
{
    public class RemarkController : Controller
    {
        private readonly IRemarkService _remarkService;
        private readonly IStudentService _studentService;
        private readonly ISubjectService _subjectService;

        public RemarkController(
            IRemarkService remarkService,
            IStudentService studentService,
            ISubjectService subjectService)
        {
            _remarkService = remarkService;
            _studentService = studentService;
            _subjectService = subjectService;
        }

        public async Task<IActionResult> Index()
        {
            var remarks = await _remarkService.GetAllRemarksAsync();
            return View(remarks);
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
        public async Task<IActionResult> Create(Remark remark)
        {
            ModelState.Remove("Student");
            ModelState.Remove("Subject");

            if (ModelState.IsValid)
            {
                await _remarkService.AddRemarkAsync(remark);
                TempData["SuccessMessage"] = "Забележката/Похвалата е добавена успешно!";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Students = new SelectList(await _studentService.GetAllStudentsAsync(), "Id", "Id", remark.StudentId);
            ViewBag.Subjects = new SelectList(await _subjectService.GetAllSubjectsAsync(), "Id", "Name", remark.SubjectId);
            return View(remark);
        }
    }
}