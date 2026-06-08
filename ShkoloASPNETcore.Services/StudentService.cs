using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using ShkoloASPNETcore.Infrastructure.Data;
using ShkoloASPNETcore.Infrastructure.Data.Models;
using ShkoloASPNETcore.Services.Contracts;
using ShkoloASPNETcore.Services.Contracts.ShkoloASPNETcore.Services.Contracts;

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
            // temp explicit fk value
            student.ApplicationUserId = userId;

            // this stops EF from trying to update the ApplicationUser object itself
            student.ApplicationUser = null;

            await _context.Students.AddAsync(student);
            await _context.SaveChangesAsync();
        }
    }
}

