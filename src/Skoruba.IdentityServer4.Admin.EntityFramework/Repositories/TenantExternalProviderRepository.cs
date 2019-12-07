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
    public class TenantExternalProviderRepository<TTenantConfigDbContext> : ITenantExternalProviderRepository
        where TTenantConfigDbContext : DbContext, IAdminTenantConfigDbContext
    {
        private readonly TTenantConfigDbContext _context;

        public TenantExternalProviderRepository(TTenantConfigDbContext context)
        {
            _context = context;
        }

        public Task<IList<ExternalProvider>> GetAllAsync(string tenantId)
        {
            throw new NotImplementedException();
        }

        public Task<(IdentityResult result, Guid id)> AddAsync(ExternalProvider provider)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> UpdateAsync(ExternalProvider provider)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> DeleteAsync(ExternalProvider provider)
        {
            throw new NotImplementedException();
        }
    }
}