using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Skoruba.IdentityServer4.Admin.EntityFramework.Entities;
using Skoruba.IdentityServer4.Admin.EntityFramework.Interfaces;
using Skoruba.IdentityServer4.Admin.EntityFramework.Repositories.Interfaces;

namespace Skoruba.IdentityServer4.Admin.EntityFramework.Repositories
{
    public class TenantClaimTypeRepository<TTenantConfigDbContext> : ITenantClaimTypeRepository
        where TTenantConfigDbContext : DbContext, IAdminTenantConfigDbContext
    {
        private readonly TTenantConfigDbContext _context;

        public TenantClaimTypeRepository(TTenantConfigDbContext context)
        {
            _context = context;
        }

        public Task<IList<ClaimType>> GetAllAsync(string tenantId)
        {
            throw new NotImplementedException();
        }

        public Task<(IdentityResult, Guid)> AddAsync(ClaimType claimType)
        {
            throw new NotImplementedException();
        }

        public Task<(IdentityResult, Guid)> UpdateAsync(ClaimType claimType)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> DeleteAsync(ClaimType claimType)
        {
            throw new NotImplementedException();
        }
    }
}