using ShkoloASPNETcore.Infrastructure.Data.Models;

namespace ShkoloASPNETcore.Services.Contracts;

public interface IGradeService
{
    Task<IEnumerable<Grade>> GetAllGradesAsync();
    Task<Student?> GetStudentWithUserAsync(int studentId);
    Task<IEnumerable<Subject>> GetAllSubjectsAsync();
    Task AddGradeAsync(Grade grade);
    Task<int?> DeleteGradeAsync(int id);
}