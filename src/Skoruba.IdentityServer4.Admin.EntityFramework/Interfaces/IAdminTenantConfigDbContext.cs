using Microsoft.EntityFrameworkCore;
using Skoruba.IdentityServer4.Admin.EntityFramework.Entities;

namespace Skoruba.IdentityServer4.Admin.EntityFramework.Interfaces
{
    public interface IAdminTenantConfigDbContext
    {
        DbSet<TenantConfiguration> TenantConfigurations { get; set; }

        DbSet<ExternalProvider> ExternalProviders { get; set; }

        DbSet<PasswordPolicy> PasswordPolicies { get; set; }

        DbSet<ClaimType> ClaimTypes { get; set; }
    }
}
