using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShkoloASPNETcore.Infrastructure.Data;
using ShkoloASPNETcore.Infrastructure.Data.Models;
using ShkoloASPNETcore.Services.Contracts;

namespace ShkoloASPNETcore.Services
{
    public class GradeService : IGradeService
    {
        private readonly ShkoloDbContext _context;

        public GradeService(ShkoloDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Grade>> GetAllGradesAsync()
        {
            return await _context.Grades
                .Include(g => g.Student)
                .Include(g => g.Subject)
                .ToListAsync();
        }

        public async Task<Grade?> GetGradeByIdAsync(int id)
        {
            return await _context.Grades
                .Include(g => g.Student)
                .Include(g => g.Subject)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task AddGradeAsync(Grade grade)
        {
            await _context.Grades.AddAsync(grade);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateGradeAsync(Grade grade)
        {
            _context.Grades.Update(grade);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteGradeAsync(int id)
        {
            var grade = await _context.Grades.FindAsync(id);
            if (grade != null)
            {
                _context.Grades.Remove(grade);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> GradeExistsAsync(int id)
        {
            return await _context.Grades.AnyAsync(e => e.Id == id);
        }
    }
}