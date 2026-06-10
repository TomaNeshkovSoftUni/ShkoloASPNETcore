using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using ShkoloASPNETcore.Core.Services.Contracts;
using ShkoloASPNETcore.Infrastructure.Data;
using ShkoloASPNETcore.Infrastructure.Data.Models;

namespace ShkoloASPNETcore.Services
{
    public class AbsenceService : IAbsenceService
    {
        private readonly ShkoloDbContext _context;

        public AbsenceService(ShkoloDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Absence>> GetAllAbsencesAsync()
        {
            return await _context.Absences
                .Include(a => a.Student)
                .Include(a => a.Subject)
                .ToListAsync();
        }

        public async Task AddAbsenceAsync(Absence absence)
        {
            await _context.Absences.AddAsync(absence);
            await _context.SaveChangesAsync();
        }
    }
}
