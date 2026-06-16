using System;
using System.Collections.Generic;
using System.Text;
using ShkoloASPNETcore.Infrastructure.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShkoloASPNETcore.Services.Contracts
{
    public interface IStudentService
    {
        Task<IEnumerable<Student>> GetAllStudentsAsync();
        Task AddStudentAsync(Student student, string userId);
        Task<Student?> GetStudentByIdAsync(int id);
        Task UpdateStudentAsync(Student student);
        Task DeleteStudentAsync(int id);
        Task<Student?> GetStudentByUserIdAsync(string userId);
    }
}