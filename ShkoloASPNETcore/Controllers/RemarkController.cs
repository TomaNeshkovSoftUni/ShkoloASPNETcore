using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShkoloASPNETcore.Infrastructure.Data;
using ShkoloASPNETcore.Infrastructure.Data.Models;
using ShkoloASPNETcore.Services.Contracts;

namespace ShkoloASPNETcore.Web.Controllers
{
    public class RemarkController : Controller
    {
        private readonly IRemarkService _remarkService;
        private readonly ShkoloDbContext _context;

        public RemarkController(IRemarkService remarkService, ShkoloDbContext context)
        {
            _remarkService = remarkService;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var remarks = await _remarkService.GetAllRemarksAsync();
            return View(remarks);
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

            ViewBag.Students = new SelectList(await _context.Students.ToListAsync(), "Id", "Id", remark.StudentId);
            ViewBag.Subjects = new SelectList(await _context.Subjects.ToListAsync(), "Id", "Name", remark.SubjectId);
            return View(remark);
        }
    }
}