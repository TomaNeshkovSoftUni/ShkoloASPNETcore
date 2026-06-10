using System.Collections.Generic;
using System.Threading.Tasks;
using ShkoloASPNETcore.Infrastructure.Data.Models;

namespace ShkoloASPNETcore.Services.Contracts
{
    public interface ITeacherService
    {
        Task<IEnumerable<Teacher>> GetAllTeachersAsync();

        Task<Teacher?> GetTeacherByIdAsync(int id);

        Task AddTeacherAsync(Teacher teacher, string userId);

        Task UpdateTeacherAsync(Teacher teacher);

        Task DeleteTeacherAsync(int id);

        Task<bool> TeacherExistsAsync(int id);
    }
}