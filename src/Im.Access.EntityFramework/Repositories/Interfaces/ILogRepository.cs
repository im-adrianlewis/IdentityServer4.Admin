using System;
using System.Threading.Tasks;
using Im.Access.EntityFramework.Entities;
using Im.Access.EntityFramework.Extensions.Common;

namespace Im.Access.EntityFramework.Repositories.Interfaces
{
    public interface ILogRepository
    {
        Task<PagedList<Log>> GetLogsAsync(string search, int page = 1, int pageSize = 10);

        Task DeleteLogsOlderThanAsync(DateTime deleteOlderThan);
    }
}