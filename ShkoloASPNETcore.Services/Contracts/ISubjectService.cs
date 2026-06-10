using System.Collections.Generic;
using System.Threading.Tasks;
using ShkoloASPNETcore.Infrastructure.Data.Models;

namespace ShkoloASPNETcore.Services.Contracts
{
    public interface ISubjectService
    {
        Task<IEnumerable<Subject>> GetAllSubjectsAsync();
        Task<Subject?> GetSubjectByIdAsync(int id);
        Task AddSubjectAsync(Subject subject);
        Task UpdateSubjectAsync(Subject subject);
        Task DeleteSubjectAsync(int id);
        Task<bool> SubjectExistsAsync(int id);
    }
}