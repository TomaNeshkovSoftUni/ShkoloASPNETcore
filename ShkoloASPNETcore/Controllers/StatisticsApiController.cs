using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShkoloASPNETcore.Services.Contracts;

namespace ShkoloASPNETcore.Web.Controllers
{
    [ApiController]
    [Route("api/statistics")]
    public class StatisticsApiController : ControllerBase
    {
        private readonly IStatisticsService _statisticsService;

        public StatisticsApiController(IStatisticsService statisticsService)
        {
            _statisticsService = statisticsService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCounts()
        {
            var totalStudents = await _statisticsService.GetTotalStudentsCountAsync();
            var totalTeachers = await _statisticsService.GetTotalTeachersCountAsync();

            return Ok(new { students = totalStudents, teachers = totalTeachers });
        }
    }
}