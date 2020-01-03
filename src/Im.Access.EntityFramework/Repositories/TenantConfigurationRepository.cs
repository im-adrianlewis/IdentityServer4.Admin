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
    public class TenantConfigurationRepository<TTenantConfigDbContext> : ITenantConfigurationRepository
        where TTenantConfigDbContext : DbContext, IAdminTenantConfigDbContext
    {
        private readonly TTenantConfigDbContext _context;

        public TenantConfigurationRepository(TTenantConfigDbContext context)
        {
            _context = context;
        }

        public async Task<IList<TenantConfiguration>> GetAllAsync()
        {
            return await _context.TenantConfigurations.ToListAsync();
        }

        public Task<TenantConfiguration> GetByTenantId(string tenantId)
        {
            return _context.TenantConfigurations.FirstOrDefaultAsync(x => x.Tenant == tenantId);
        }

        public async Task<(IdentityResult result, Guid id)> AddAsync(TenantConfiguration configuration)
        {
            try
            {
                _context.TenantConfigurations.Add(configuration);
                await _context.SaveChangesAsync();
                return (IdentityResult.Success, configuration.Id);
            }
            catch (Exception exception)
            {
                return (IdentityResult.Failed(
                    new[]
                    {
                        new IdentityError
                        {
                            Code = "TCR001",
                            Description = exception.Message
                        }
                    }), Guid.Empty);
            }
        }

        public async Task<IdentityResult> UpdateAsync(TenantConfiguration configuration)
        {
            if (configuration.Id == Guid.Empty)
            {
                var existingConfiguration = await _context
                    .TenantConfigurations
                    .AsNoTracking()
                    .FirstOrDefaultAsync(e => e.Tenant == configuration.Tenant);
                if (existingConfiguration == null)
                {
                    return IdentityResult.Failed(
                        new IdentityError
                        {
                            Code = "TCR002",
                            Description = "Tenant not found"
                        });
                }

                configuration.Id = existingConfiguration.Id;
            }

            _context.Attach(configuration).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(string tenantId)
        {
            try
            {
                var configuration = await _context
                    .TenantConfigurations
                    .FirstOrDefaultAsync(x => x.Tenant == tenantId);
                if (configuration == null)
                {
                    return IdentityResult.Success;
                }

                _context.TenantConfigurations.Remove(configuration);
                await _context.SaveChangesAsync();
                return IdentityResult.Success;
            }
            catch (Exception e)
            {
                return IdentityResult.Failed(
                    new IdentityError
                    {
                        Code = "TCR003",
                        Description = "Failed to remove tenant configuration"
                    });
            }
        }
    }
}
