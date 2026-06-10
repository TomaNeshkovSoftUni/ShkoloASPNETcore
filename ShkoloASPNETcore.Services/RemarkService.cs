using System.Collections.Generic;
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
                .ToListAsync();
        }

        public async Task AddRemarkAsync(Remark remark)
        {
            await _context.Remarks.AddAsync(remark);
            await _context.SaveChangesAsync();
        }
    }
}