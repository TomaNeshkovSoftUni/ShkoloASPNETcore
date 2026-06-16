using ShkoloASPNETcore.Infrastructure.Data.Models;

namespace ShkoloASPNETcore.Services.Contracts
{
    public interface IGradeService
    {
        Task<IEnumerable<Grade>> GetAllGradesAsync();
        Task AddGradeAsync(Grade grade);
        Task UpdateGradeAsync(Grade grade);
        Task DeleteGradeAsync(int id);
        Task<Grade?> GetGradeByIdAsync(int id);
        Task<IEnumerable<Grade>> GetGradesByStudentIdAsync(int studentId);
    }
}