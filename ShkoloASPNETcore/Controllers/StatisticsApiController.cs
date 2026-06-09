using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShkoloASPNETcore.Infrastructure.Data;

namespace ShkoloASPNETcore.Web.Controllers
{
    [ApiController]
    [Route("api/statistics")]
    public class StatisticsApiController : ControllerBase
    {
        private readonly ShkoloDbContext _context;

        public StatisticsApiController(ShkoloDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetCounts()
        {
            var totalStudents = await _context.Students.CountAsync();
            var totalTeachers = await _context.Teachers.CountAsync();

            return Ok(new { students = totalStudents, teachers = totalTeachers });
        }
    }
}