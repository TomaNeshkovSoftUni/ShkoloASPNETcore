using System.Collections.Generic;
using System.Threading.Tasks;
using ShkoloASPNETcore.Infrastructure.Data.Models;

namespace ShkoloASPNETcore.Services.Contracts
{
    public interface IGradeService
    {
        Task<IEnumerable<Grade>> GetAllGradesAsync();
        Task<Grade?> GetGradeByIdAsync(int id);
        Task AddGradeAsync(Grade grade);
        Task UpdateGradeAsync(Grade grade);
        Task DeleteGradeAsync(int id);
        Task<bool> GradeExistsAsync(int id);
    }
}