using System;
using System.Collections.Generic;
using System.Text;
using global::ShkoloASPNETcore.Infrastructure.Models.ShkoloClone.Infrastructure.Data.Models;

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
