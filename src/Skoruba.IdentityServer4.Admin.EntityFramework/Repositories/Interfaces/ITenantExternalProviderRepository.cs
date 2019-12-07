using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Skoruba.IdentityServer4.Admin.EntityFramework.Entities;

namespace Skoruba.IdentityServer4.Admin.EntityFramework.Repositories.Interfaces
{
    public interface ITenantExternalProviderRepository
    {
        Task<IList<ExternalProvider>> GetAllAsync(string tenantId);

        Task<(IdentityResult result, Guid id)> AddAsync(ExternalProvider provider);

        Task<IdentityResult> UpdateAsync(ExternalProvider provider);

        Task<IdentityResult> DeleteAsync(ExternalProvider provider);
    }
}