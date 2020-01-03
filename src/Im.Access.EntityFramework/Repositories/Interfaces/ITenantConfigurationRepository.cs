using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Im.Access.EntityFramework.Entities;

namespace Im.Access.EntityFramework.Repositories.Interfaces
{
    public interface ITenantConfigurationRepository
    {
        Task<IList<TenantConfiguration>> GetAllAsync();

        Task<TenantConfiguration> GetByTenantId(string tenantId);

        Task<(IdentityResult result, Guid id)> AddAsync(TenantConfiguration configuration);

        Task<IdentityResult> UpdateAsync(TenantConfiguration configuration);

        Task<IdentityResult> DeleteAsync(string tenantId);
    }
}
