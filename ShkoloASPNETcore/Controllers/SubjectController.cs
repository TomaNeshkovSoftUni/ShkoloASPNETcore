using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShkoloASPNETcore.Infrastructure.Data;
using ShkoloASPNETcore.Infrastructure.Data.Models;

namespace ShkoloASPNETcore.Web.Controllers
{
    [Authorize(Roles = "Administrator,Teacher")]
    public class SubjectController : Controller
    {
        private readonly ShkoloDbContext _context;

        public SubjectController(ShkoloDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Subjects.ToListAsync());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new Subject());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Subject subject)
        {
            ModelState.Remove("TeacherId");
            ModelState.Remove("Teacher");

            if (ModelState.IsValid)
            {
                var anyTeacher = await _context.Teachers.FirstOrDefaultAsync();

                if (anyTeacher == null)
                {
                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    anyTeacher = new Teacher
                    {
                        FirstName = "Служебен",
                        LastName = "Профил",
                        Department = "Администрация",
                        ApplicationUserId = userId
                    };
                    _context.Teachers.Add(anyTeacher);
                    await _context.SaveChangesAsync();
                }

                subject.TeacherId = anyTeacher.Id;

                _context.Subjects.Add(subject);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Предметът е добавен успешно!";
                return RedirectToAction(nameof(Index));
            }

            return View(subject);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var subject = await _context.Subjects.FindAsync(id);
            if (subject == null) return NotFound();
            return View(subject);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Subject subject)
        {
            if (id != subject.Id) return NotFound();

            ModelState.Remove("TeacherId");
            ModelState.Remove("Teacher");

            if (ModelState.IsValid)
            {
                var anyTeacher = await _context.Teachers.FirstOrDefaultAsync();

                if (anyTeacher == null)
                {
                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    anyTeacher = new Teacher
                    {
                        FirstName = "Служебен",
                        LastName = "Профил",
                        Department = "Администрация",
                        ApplicationUserId = userId
                    };
                    _context.Teachers.Add(anyTeacher);
                    await _context.SaveChangesAsync();
                }

                subject.TeacherId = anyTeacher.Id;

                _context.Update(subject);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Предметът е коригиран успешно!";
                return RedirectToAction(nameof(Index));
            }
            return View(subject);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var subject = await _context.Subjects.FindAsync(id);
            if (subject == null) return NotFound();
            return View(subject);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var subject = await _context.Subjects.FindAsync(id);
            if (subject != null)
            {
                _context.Subjects.Remove(subject);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Предметът е изтрит успешно!";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}