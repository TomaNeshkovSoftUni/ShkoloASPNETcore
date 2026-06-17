using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShkoloASPNETcore.Infrastructure.Data;
using ShkoloASPNETcore.Infrastructure.Data.Models;
using ShkoloASPNETcore.Infrastructure.Data.Enums;
using ShkoloASPNETcore.Web.Models;

namespace ShkoloASPNETcore.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ShkoloDbContext _context;

        public HomeController(UserManager<ApplicationUser> userManager, ShkoloDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var model = new DashboardViewModel();
            bool isStaff = User.IsInRole("Teacher") || User.IsInRole("Administrator");
            model.IsStaff = isStaff;

            if (isStaff)
            {
                model.FullName = $"{user.FirstName} {user.LastName}";
                model.Value = await _context.Grades.AnyAsync()
                    ? await _context.Grades.AverageAsync(g => g.Value)
                    : 0;
                model.GradesCount = await _context.Grades.CountAsync();

                model.AbsencesCount = await _context.Absences.AnyAsync()
                    ? (double)await _context.Absences.SumAsync(a => a.Type == AbsenceType.Закъснение ? 0.5m : 1.0m)
                    : 0;

                model.FeedbackCount = await _context.Remarks.CountAsync();
            }
            else
            {
                var student = await _context.Students
                    .FirstOrDefaultAsync(s => s.ApplicationUserId == user.Id);

                if (student == null)
                {
                    return View("AccessDenied");
                }

                var grades = await _context.Grades
                    .Where(g => g.StudentId == student.Id)
                    .ToListAsync();

                model.FullName = $"{user.FirstName} {user.LastName}";
                model.Value = grades.Any() ? grades.Average(g => g.Value) : 0;
                model.GradesCount = grades.Count;

                model.AbsencesCount = await _context.Absences.Where(a => a.StudentId == student.Id).AnyAsync()
                    ? (double)await _context.Absences
                        .Where(a => a.StudentId == student.Id)
                        .SumAsync(a => a.Type == AbsenceType.Закъснение ? 0.5m : 1.0m)
                    : 0;

                model.FeedbackCount = await _context.Remarks
                    .CountAsync(r => r.StudentId == student.Id);

                var allStudentsAverages = await _context.Grades
                    .GroupBy(g => g.StudentId)
                    .Select(g => g.Average(x => x.Value))
                    .ToListAsync();

                model.SchoolRank = allStudentsAverages.Count(avg => avg > model.Value) + 1;
            }

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}