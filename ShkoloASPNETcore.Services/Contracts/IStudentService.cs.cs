using System;
using System.Collections.Generic;
using System.Text;
using ShkoloASPNETcore.Infrastructure.Data.Models;


namespace ShkoloASPNETcore.Services.Contracts
{

    namespace ShkoloASPNETcore.Services.Contracts
    {
        public interface IStudentService
        {
            Task<IEnumerable<Student>> GetAllStudentsAsync();
        }
    }
}
