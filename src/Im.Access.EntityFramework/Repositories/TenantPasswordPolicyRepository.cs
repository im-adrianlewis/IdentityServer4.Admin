using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Im.Access.EntityFramework.Entities;
using Im.Access.EntityFramework.Interfaces;
using Im.Access.EntityFramework.Repositories.Interfaces;

namespace Im.Access.EntityFramework.Repositories
{
    public class TenantPasswordPolicyRepository<TTenantConfigDbContext> : ITenantPasswordPolicyRepository
        where TTenantConfigDbContext : DbContext, IAdminTenantConfigDbContext
    {
        private readonly TTenantConfigDbContext _context;

        public TenantPasswordPolicyRepository(TTenantConfigDbContext context)
        {
            _context = context;
        }

        public Task<PasswordPolicy> FindByTenantAsync(string tenantId)
        {
            return _context
                .PasswordPolicies
                .Include(p => p.TenantConfiguration)
                .FirstOrDefaultAsync(p => p.TenantConfiguration.Tenant == tenantId);
        }

        public async Task<IdentityResult> UpdateAsync(string tenantId, PasswordPolicy policy)
        {
            try
            {
                var configuration = await _context
                    .TenantConfigurations
                    .Include(c => c.PasswordPolicy)
                    .FirstOrDefaultAsync(c => c.Tenant == tenantId);

                if (configuration == null)
                {
                    return IdentityResult.Failed(
                        new IdentityError
                        {
                            Code = "PPR001",
                            Description = "Tenant not found"
                        });
                }

                if (configuration.PasswordPolicy == null)
                {
                    configuration.PasswordPolicy = policy;
                    policy.TenantConfiguration = configuration;
                    policy.Id = Guid.Empty;
                    _context.PasswordPolicies.Add(policy);
                }
                else
                {
                    configuration.PasswordPolicy.RequireDigit = policy.RequireDigit;
                    configuration.PasswordPolicy.RequireLowerCase = policy.RequireLowerCase;
                    configuration.PasswordPolicy.RequireUpperCase = policy.RequireUpperCase;
                    configuration.PasswordPolicy.RequireNonAlphaNumeric = policy.RequireNonAlphaNumeric;
                    configuration.PasswordPolicy.MinimumLength = policy.MinimumLength;
                }

                await _context.SaveChangesAsync();

                return IdentityResult.Success;
            }
            catch (Exception e)
            {
                return IdentityResult.Failed(
                    new IdentityError
                    {
                        Code = "PPR001",
                        Description = "Failed to add/update password policy"
                    });
            }
        }
    }
}