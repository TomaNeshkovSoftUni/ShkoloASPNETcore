using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShkoloASPNETcore.Infrastructure.Data;
using ShkoloASPNETcore.Infrastructure.Data.Models;
using ShkoloASPNETcore.Services.Contracts;

namespace ShkoloASPNETcore.Services
{
    public class SubjectService : ISubjectService
    {
        private readonly ShkoloDbContext _context;

        public SubjectService(ShkoloDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Subject>> GetAllSubjectsAsync()
        {
            return await _context.Subjects.Include(s => s.Teacher).ToListAsync();
        }

        public async Task<Subject?> GetSubjectByIdAsync(int id)
        {
            return await _context.Subjects
                .Include(s => s.Teacher)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task AddSubjectAsync(Subject subject)
        {
            await _context.Subjects.AddAsync(subject);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateSubjectAsync(Subject subject)
        {
            _context.Subjects.Update(subject);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteSubjectAsync(int id)
        {
            var subject = await _context.Subjects.FindAsync(id);
            if (subject != null)
            {
                _context.Subjects.Remove(subject);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> SubjectExistsAsync(int id)
        {
            return await _context.Subjects.AnyAsync(e => e.Id == id);
        }
    }
}