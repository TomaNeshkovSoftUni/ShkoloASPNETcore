using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShkoloASPNETcore.Infrastructure.Data;
using ShkoloASPNETcore.Infrastructure.Data.Enums;
using ShkoloASPNETcore.Infrastructure.Data.Models;

namespace ShkoloASPNETcore.Web.Controllers
{
    public class AbsenceController : Controller
    {
        private readonly ShkoloDbContext _context;

        public AbsenceController(ShkoloDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var absences = await _context.Absences
                .Include(a => a.Student)
                .Include(a => a.Subject)
                .ToListAsync();
            return View(absences);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Students = new SelectList(await _context.Students.ToListAsync(), "Id", "Id");
            ViewBag.Subjects = new SelectList(await _context.Subjects.ToListAsync(), "Id", "Name");
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
                _context.Add(absence);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Отсъствието/Закъснението е добавено успешно!";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Students = new SelectList(await _context.Students.ToListAsync(), "Id", "Id", absence.StudentId);
            ViewBag.Subjects = new SelectList(await _context.Subjects.ToListAsync(), "Id", "Name", absence.SubjectId);
            return View(absence);
        }
    }
}