using System.Threading.Tasks;

namespace ShkoloASPNETcore.Services.Contracts
{
    public interface IStatisticsService
    {
        Task<int> GetTotalStudentsCountAsync();
        Task<int> GetTotalTeachersCountAsync();
    }
}