using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShkoloASPNETcore.Infrastructure.Data.Models;
using ShkoloASPNETcore.Services.Contracts;
using ShkoloASPNETcore.Web.Models;

namespace ShkoloASPNETcore.Web.Controllers
{
    [Authorize(Roles = "Administrator,Teacher")]
    public class SubjectController : Controller
    {
        private readonly ISubjectService _subjectService;
        private readonly ITeacherService _teacherService;

        public SubjectController(ISubjectService subjectService, ITeacherService teacherService)
        {
            _subjectService = subjectService;
            _teacherService = teacherService;
        }

        public async Task<IActionResult> Index()
        {
            var subjects = await _subjectService.GetAllSubjectsAsync();
            return View(subjects);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Teachers = new SelectList(await _teacherService.GetAllTeachersAsync(), "Id", "LastName");
            return View(new SubjectFormViewModel());
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var subject = await _subjectService.GetSubjectByIdAsync(id);
            if (subject == null)
            {
                return NotFound();
            }
            return View(subject);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SubjectFormViewModel model)
        {
            if (ModelState.IsValid)
            {

                var subject = new Subject
                {
                    Name = model.Name,
                    TeacherId = model.TeacherId
                };

                await _subjectService.AddSubjectAsync(subject);

                TempData["SuccessMessage"] = "Предметът е добавен успешно!";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Teachers = new SelectList(await _teacherService.GetAllTeachersAsync(), "Id", "LastName", model.TeacherId);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var subject = await _subjectService.GetSubjectByIdAsync(id);
            if (subject == null) return NotFound();

            var model = new SubjectFormViewModel
            {
                Id = subject.Id,
                Name = subject.Name,
                TeacherId = subject.TeacherId
            };

            ViewBag.Teachers = new SelectList(await _teacherService.GetAllTeachersAsync(), "Id", "LastName", subject.TeacherId);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SubjectFormViewModel model)
        {
            if (id != model.Id) return NotFound();

            if (ModelState.IsValid)
            {

                var subject = new Subject
                {
                    Id = model.Id,
                    Name = model.Name,
                    TeacherId = model.TeacherId
                };

                await _subjectService.UpdateSubjectAsync(subject);

                TempData["SuccessMessage"] = "Предметът е коригиран успешно!";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Teachers = new SelectList(await _teacherService.GetAllTeachersAsync(), "Id", "LastName", model.TeacherId);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var subject = await _subjectService.GetSubjectByIdAsync(id);
            if (subject == null) return NotFound();
            return View(subject);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _subjectService.DeleteSubjectAsync(id);
                TempData["SuccessMessage"] = "Предметът е изтрит успешно!";
            }
            catch (DbUpdateException)
            {
                TempData["ErrorMessage"] = "ГРЕШКА: Този предмет не може да бъде изтрит, защото към него има свързани оценки, отсъствия или забележки. Първо изтрийте тях!";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}