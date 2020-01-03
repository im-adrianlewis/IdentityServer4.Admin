using Microsoft.EntityFrameworkCore;
using Im.Access.EntityFramework.Entities;

namespace Im.Access.EntityFramework.Interfaces
{
    public interface IAdminTenantConfigDbContext
    {
        DbSet<TenantConfiguration> TenantConfigurations { get; set; }

        DbSet<ExternalProvider> ExternalProviders { get; set; }

        DbSet<PasswordPolicy> PasswordPolicies { get; set; }

        DbSet<ClaimType> ClaimTypes { get; set; }
    }
}
