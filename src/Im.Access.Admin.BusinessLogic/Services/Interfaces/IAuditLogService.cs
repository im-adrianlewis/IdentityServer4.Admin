using System;
using System.Threading.Tasks;
using Im.Access.Admin.BusinessLogic.Dtos.Log;

namespace Im.Access.Admin.BusinessLogic.Services.Interfaces
{
    public interface IAuditLogService
    {
        Task<AuditLogsDto> GetAsync(AuditLogFilterDto filters);

        Task DeleteLogsOlderThanAsync(DateTime deleteOlderThan);
    }
}
