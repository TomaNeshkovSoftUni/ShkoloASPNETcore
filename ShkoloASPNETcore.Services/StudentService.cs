using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShkoloASPNETcore.Infrastructure.Data;
using ShkoloASPNETcore.Infrastructure.Data.Models;
using ShkoloASPNETcore.Services.Contracts;

namespace ShkoloASPNETcore.Services
{
    public class StudentService : IStudentService
    {
        private readonly ShkoloDbContext _context;

        public StudentService(ShkoloDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Student>> GetAllStudentsAsync()
        {
            return await _context.Students.ToListAsync();
        }

        public async Task AddStudentAsync(Student student, string userId)
        {
            student.ApplicationUserId = userId;
            await _context.Students.AddAsync(student);
            await _context.SaveChangesAsync();
        }

        public async Task<Student?> GetStudentByIdAsync(int id)
        {
            return await _context.Students.FindAsync(id);
        }

        public async Task UpdateStudentAsync(Student student)
        {
            _context.Students.Update(student);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteStudentAsync(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student != null)
            {
                _context.Students.Remove(student);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<Student?> GetStudentByUserIdAsync(string userId)
        {
            return await _context.Students
                .FirstOrDefaultAsync(s => s.ApplicationUserId == userId);
        }
    }
}