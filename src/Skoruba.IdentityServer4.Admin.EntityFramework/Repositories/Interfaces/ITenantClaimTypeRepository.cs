using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Skoruba.IdentityServer4.Admin.EntityFramework.Entities;

namespace Skoruba.IdentityServer4.Admin.EntityFramework.Repositories.Interfaces
{
    public interface ITenantClaimTypeRepository
    {
        Task<IList<ClaimType>> GetAllAsync(string tenantId);

        Task<(IdentityResult, Guid)> AddAsync(ClaimType claimType);

        Task<(IdentityResult, Guid)> UpdateAsync(ClaimType claimType);

        Task<IdentityResult> DeleteAsync(ClaimType claimType);
    }
}