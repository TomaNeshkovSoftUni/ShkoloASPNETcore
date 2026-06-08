using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShkoloASPNETcore.Infrastructure.Data.Models;
using ShkoloASPNETcore.Services.Contracts;
using ShkoloASPNETcore.Services.Contracts.ShkoloASPNETcore.Services.Contracts;

namespace ShkoloASPNETcore.Web.Controllers
{
    public class StudentController : Controller
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        public async Task<IActionResult> Index()
        {
            var students = await _studentService.GetAllStudentsAsync();
            return View(students);
        }

        [HttpGet]
        public IActionResult Create()
        {   
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Student student)
        {
            student.EnrollmentNumber = "TEMP-123";

            ModelState.Remove("Grades");
            ModelState.Remove("ApplicationUser");
            ModelState.Remove("ApplicationUserId");
            ModelState.Remove("EnrollmentNumber");

            if (!ModelState.IsValid)
            {
                return View(student);
            }

            // pass the ID to the service adn the service will find the user
            await _studentService.AddStudentAsync(student, "1");

            return RedirectToAction(nameof(Index));
        }
    }
}