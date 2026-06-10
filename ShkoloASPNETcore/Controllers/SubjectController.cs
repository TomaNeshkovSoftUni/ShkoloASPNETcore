using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShkoloASPNETcore.Infrastructure.Data.Models;
using ShkoloASPNETcore.Services.Contracts;

namespace ShkoloASPNETcore.Web.Controllers
{
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

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var subject = await _subjectService.GetSubjectByIdAsync(id.Value);
            if (subject == null) return NotFound();

            return View(subject);
        }

        public async Task<IActionResult> Create()
        {
            ViewData["TeacherId"] = new SelectList(await _teacherService.GetAllTeachersAsync(), "Id", "ApplicationUserId");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,TeacherId")] Subject subject)
        {
            if (ModelState.IsValid)
            {
                await _subjectService.AddSubjectAsync(subject);
                return RedirectToAction(nameof(Index));
            }
            ViewData["TeacherId"] = new SelectList(await _teacherService.GetAllTeachersAsync(), "Id", "ApplicationUserId", subject.TeacherId);
            return View(subject);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var subject = await _subjectService.GetSubjectByIdAsync(id.Value);
            if (subject == null) return NotFound();

            ViewData["TeacherId"] = new SelectList(await _teacherService.GetAllTeachersAsync(), "Id", "ApplicationUserId", subject.TeacherId);
            return View(subject);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,TeacherId")] Subject subject)
        {
            if (id != subject.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    await _subjectService.UpdateSubjectAsync(subject);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _subjectService.SubjectExistsAsync(subject.Id))
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
            ViewData["TeacherId"] = new SelectList(await _teacherService.GetAllTeachersAsync(), "Id", "ApplicationUserId", subject.TeacherId);
            return View(subject);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var subject = await _subjectService.GetSubjectByIdAsync(id.Value);
            if (subject == null) return NotFound();

            return View(subject);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _subjectService.DeleteSubjectAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}