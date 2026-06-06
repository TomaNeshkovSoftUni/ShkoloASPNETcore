using global::ShkoloASPNETcore.Services.Contracts.ShkoloASPNETcore.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using ShkoloASPNETcore.Services.Contracts;

namespace ShkoloASPNETcore.Web.Controllers
{

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
        }
    }
}
