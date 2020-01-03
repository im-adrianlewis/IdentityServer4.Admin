using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Im.Access.EntityFramework.Entities;

namespace Im.Access.EntityFramework.Repositories.Interfaces
{
    public interface ITenantExternalProviderRepository
    {
        Task<IList<ExternalProvider>> GetAllAsync(string tenantId);

        Task<(IdentityResult result, Guid id)> AddAsync(ExternalProvider provider);

        Task<IdentityResult> UpdateAsync(ExternalProvider provider);

        Task<IdentityResult> DeleteAsync(ExternalProvider provider);
    }
}