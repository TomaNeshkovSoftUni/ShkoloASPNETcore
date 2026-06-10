using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShkoloASPNETcore.Infrastructure.Data;
using ShkoloASPNETcore.Infrastructure.Data.Models;
using ShkoloASPNETcore.Services.Contracts;

namespace ShkoloASPNETcore.Web.Controllers
{
    [Authorize]
    public class GradeController : Controller
    {
        private readonly IGradeService _gradeService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ShkoloDbContext _context;

        public GradeController(IGradeService gradeService, UserManager<ApplicationUser> userManager, ShkoloDbContext context)
        {
            _gradeService = gradeService;
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var allGrades = await _context.Grades
                .Include(g => g.Student)
                .Include(g => g.Subject)
                .ToListAsync();
            return View(allGrades);
        }

        [HttpGet]
        [Authorize(Roles = "Teacher,Administrator")]
        public IActionResult Create()
        {
            var students = _context.Students
                .Select(s => new { s.Id, FullName = s.FirstName + " " + s.LastName })
                .ToList();

            ViewBag.Students = new SelectList(students, "Id", "FullName");
            ViewBag.Subjects = new SelectList(_context.Subjects.ToList(), "Id", "Name");

            return View(new Grade { DateIssued = DateTime.Now });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Teacher,Administrator")]
        public async Task<IActionResult> Create(Grade dummy)
        {
            int.TryParse(Request.Form["StudentId"], out int studentId);
            int.TryParse(Request.Form["SubjectId"], out int subjectId);

            string valStr = Request.Form["Value"].ToString().Replace(",", ".");
            if (!decimal.TryParse(valStr, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out decimal gradeValue))
            {
                decimal.TryParse(Request.Form["Value"].ToString(), out gradeValue);
            }

            if (gradeValue < 2 || gradeValue > 6)
            {
                gradeValue = 6.00m;
            }

            if (studentId == 0 || !_context.Students.Any(s => s.Id == studentId))
            {
                studentId = _context.Students.Select(s => s.Id).FirstOrDefault();
            }
            if (subjectId == 0 || !_context.Subjects.Any(s => s.Id == subjectId))
            {
                subjectId = _context.Subjects.Select(s => s.Id).FirstOrDefault();
            }

            var finalGrade = new Grade
            {
                StudentId = studentId,
                SubjectId = subjectId,
                Value = gradeValue,
                DateIssued = DateTime.Now,
                Issuer = User.Identity?.Name ?? "Unknown"
            };

            _context.Grades.Add(finalGrade);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Оценката е въведена успешно!";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Authorize(Roles = "Teacher,Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var currentGrade = await _context.Grades.FindAsync(id.Value);

            if (currentGrade == null) return NotFound();

            var students = _context.Students.Select(s => new { s.Id, FullName = s.FirstName + " " + s.LastName }).ToList();
            ViewBag.Students = new SelectList(students, "Id", "FullName");
            ViewBag.Subjects = new SelectList(_context.Subjects.ToList(), "Id", "Name");

            return View(currentGrade);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Teacher,Administrator")]
        public async Task<IActionResult> Edit(int id, Grade dummy)
        {
            int.TryParse(Request.Form["StudentId"], out int studentId);
            int.TryParse(Request.Form["SubjectId"], out int subjectId);

            string valStr = Request.Form["Value"].ToString().Replace(",", ".");
            if (!decimal.TryParse(valStr, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out decimal gradeValue))
            {
                decimal.TryParse(Request.Form["Value"].ToString(), out gradeValue);
            }

            var existingGrade = await _context.Grades.FindAsync(id);
            if (existingGrade == null) return NotFound();

            existingGrade.StudentId = studentId;
            existingGrade.SubjectId = subjectId;
            existingGrade.Value = gradeValue;
            existingGrade.Issuer = User.Identity?.Name ?? "Unknown";

            _context.Update(existingGrade);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Оценката е коригирана успешно!";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Authorize(Roles = "Teacher,Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var currentGrade = await _context.Grades
                .Include(g => g.Student)
                .Include(g => g.Subject)
                .FirstOrDefaultAsync(g => g.Id == id.Value);

            if (currentGrade == null) return NotFound();
            return View(currentGrade);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Teacher,Administrator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var grade = await _context.Grades.FindAsync(id);
            if (grade != null)
            {
                _context.Grades.Remove(grade);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Оценката е изтрита успешно!";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}