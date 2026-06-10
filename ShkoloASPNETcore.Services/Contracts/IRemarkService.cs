using System.Collections.Generic;
using System.Threading.Tasks;
using ShkoloASPNETcore.Infrastructure.Data.Models;

namespace ShkoloASPNETcore.Services.Contracts
{
    public interface IRemarkService
    {
        Task<IEnumerable<Remark>> GetAllRemarksAsync();
        Task AddRemarkAsync(Remark remark);
    }
}