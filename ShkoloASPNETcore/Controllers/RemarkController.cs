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
    public class RemarkController : Controller
    {
        private readonly ShkoloDbContext _context;

        public RemarkController(ShkoloDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var remarks = await _context.Remarks
                .Include(r => r.Student)
                .Include(r => r.Subject)
                .ToListAsync();
            return View(remarks);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Students = new SelectList(await _context.Students.ToListAsync(), "Id", "EnrollmentNumber");
            ViewBag.Subjects = new SelectList(await _context.Subjects.ToListAsync(), "Id", "Name");
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
                _context.Add(remark);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Забележката/Похвалата е добавена успешно!";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Students = new SelectList(await _context.Students.ToListAsync(), "Id", "EnrollmentNumber", remark.StudentId);
            ViewBag.Subjects = new SelectList(await _context.Subjects.ToListAsync(), "Id", "Name", remark.SubjectId);
            return View(remark);
        }
    }
}