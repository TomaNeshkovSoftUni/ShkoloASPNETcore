using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShkoloASPNETcore.Infrastructure.Data;
using ShkoloASPNETcore.Infrastructure.Data.Models;
using ShkoloASPNETcore.Services.Contracts;

namespace ShkoloASPNETcore.Services
{
    public class RemarkService : IRemarkService
    {
        private readonly ShkoloDbContext _context;

        public RemarkService(ShkoloDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Remark>> GetAllRemarksAsync()
        {
            return await _context.Remarks
                .Include(r => r.Student)
                .Include(r => r.Subject)
                .Include(r => r.Subject.Teacher)
                .ToListAsync();
        }
        public async Task<IEnumerable<Remark>> GetRemarksByStudentIdAsync(int studentId)
        {
            return await _context.Remarks
                .Include(r => r.Student)
                .Include(r => r.Subject)
                .Include(r => r.Subject.Teacher)
                .Where(r => r.StudentId == studentId)
                .ToListAsync();
        }

        public async Task<Remark?> GetRemarkByIdAsync(int id)
        {
            return await _context.Remarks
                .Include(r => r.Student)
                .Include(r => r.Subject)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task AddRemarkAsync(Remark remark)
        {
            await _context.Remarks.AddAsync(remark);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateRemarkAsync(Remark remark)
        {
            _context.Remarks.Update(remark);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteRemarkAsync(int id)
        {
            var remark = await _context.Remarks.FindAsync(id);
            if (remark != null)
            {
                _context.Remarks.Remove(remark);
                await _context.SaveChangesAsync();
            }
        }
    }
}