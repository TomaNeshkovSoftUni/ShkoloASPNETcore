using System.Collections.Generic;
using System.Threading.Tasks;
using ShkoloASPNETcore.Infrastructure.Data.Models;

namespace ShkoloASPNETcore.Core.Services.Contracts
{
    public interface IAbsenceService
    {
        Task<IEnumerable<Absence>> GetAllAbsencesAsync();
        Task AddAbsenceAsync(Absence absence);
        Task<Absence?> GetAbsenceByIdAsync(int id);
        Task UpdateAbsenceAsync(Absence absence);
        Task DeleteAbsenceAsync(int id);
    }
}