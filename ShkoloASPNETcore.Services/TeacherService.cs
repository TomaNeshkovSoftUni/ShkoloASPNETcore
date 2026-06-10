using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShkoloASPNETcore.Infrastructure.Data;
using ShkoloASPNETcore.Infrastructure.Data.Models;
using ShkoloASPNETcore.Services.Contracts;

namespace ShkoloASPNETcore.Services
{
    public class TeacherService : ITeacherService
    {
        private readonly ShkoloDbContext _context;

        public TeacherService(ShkoloDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Teacher>> GetAllTeachersAsync()
        {
            return await _context.Teachers.Include(t => t.ApplicationUser).ToListAsync();
        }

        public async Task<Teacher?> GetTeacherByIdAsync(int id)
        {
            return await _context.Teachers
                .Include(t => t.ApplicationUser)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task AddTeacherAsync(Teacher teacher)
        {
            await _context.Teachers.AddAsync(teacher);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTeacherAsync(Teacher teacher)
        {
            _context.Teachers.Update(teacher);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTeacherAsync(int id)
        {
            var teacher = await _context.Teachers.FindAsync(id);
            if (teacher != null)
            {
                _context.Teachers.Remove(teacher);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> TeacherExistsAsync(int id)
        {
            return await _context.Teachers.AnyAsync(e => e.Id == id);
        }
    }
}