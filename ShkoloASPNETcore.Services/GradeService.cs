using Microsoft.EntityFrameworkCore;
using ShkoloASPNETcore.Infrastructure.Data;
using ShkoloASPNETcore.Infrastructure.Data.Models;
using ShkoloASPNETcore.Services.Contracts;

namespace ShkoloASPNETcore.Services;

public class GradeService : IGradeService
{
    private readonly ShkoloDbContext _context;

    public GradeService(ShkoloDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Grade>> GetAllGradesAsync()
    {
        return await _context.Grades.ToListAsync();
    }

    public async Task<Student?> GetStudentWithUserAsync(int studentId)
    {
        return await _context.Students
            .Include(s => s.ApplicationUser)
            .FirstOrDefaultAsync(s => s.Id == studentId);
    }

    public async Task<IEnumerable<Subject>> GetAllSubjectsAsync()
    {
        return await _context.Subjects.ToListAsync();
    }

    public async Task AddGradeAsync(Grade grade)
    {
        grade.DateIssued = DateTime.Now;
        await _context.Grades.AddAsync(grade);
        await _context.SaveChangesAsync();
    }

    public async Task<int?> DeleteGradeAsync(int id)
    {
        var grade = await _context.Grades.FindAsync(id);

        if (grade == null)
        {
            return null;
        }

        int studentId = grade.StudentId;

        _context.Grades.Remove(grade);
        await _context.SaveChangesAsync();

        return studentId;
    }
}