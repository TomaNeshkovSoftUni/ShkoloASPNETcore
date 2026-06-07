using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using ShkoloASPNETcore.Infrastructure.Data;
using ShkoloASPNETcore.Infrastructure.Data.Models;
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
    }
}
