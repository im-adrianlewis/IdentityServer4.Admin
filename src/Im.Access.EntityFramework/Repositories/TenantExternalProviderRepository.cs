using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Im.Access.EntityFramework.Entities;
using Im.Access.EntityFramework.Interfaces;
using Im.Access.EntityFramework.Repositories.Interfaces;

namespace Im.Access.EntityFramework.Repositories
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