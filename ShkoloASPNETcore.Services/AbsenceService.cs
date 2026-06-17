using System.Collections.Generic;
using System.Threading.Tasks;
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
                .Include(a => a.Subject.Teacher)
                .ToListAsync();
        }

        public async Task AddAbsenceAsync(Absence absence)
        {
            await _context.Absences.AddAsync(absence);
            await _context.SaveChangesAsync();
        }

        public async Task<Absence?> GetAbsenceByIdAsync(int id)
        {
            return await _context.Absences
                .Include(a => a.Student)
                .Include(a => a.Subject)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task UpdateAbsenceAsync(Absence absence)
        {
            _context.Absences.Update(absence);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAbsenceAsync(int id)
        {
            var absence = await _context.Absences.FindAsync(id);
            if (absence != null)
            {
                _context.Absences.Remove(absence);
                await _context.SaveChangesAsync();
            }
        }
    }
}