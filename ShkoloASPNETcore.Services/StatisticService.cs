using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShkoloASPNETcore.Infrastructure.Data;
using ShkoloASPNETcore.Services.Contracts;

namespace ShkoloASPNETcore.Services
{
    public class StatisticsService : IStatisticsService
    {
        private readonly ShkoloDbContext _context;

        public StatisticsService(ShkoloDbContext context)
        {
            _context = context;
        }

        public async Task<int> GetTotalStudentsCountAsync()
        {
            return await _context.Students.CountAsync();
        }

        public async Task<int> GetTotalTeachersCountAsync()
        {
            return await _context.Teachers.CountAsync();
        }
    }
}