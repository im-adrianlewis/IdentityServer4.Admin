using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Skoruba.IdentityServer4.Admin.EntityFramework.Entities;

namespace Skoruba.IdentityServer4.Admin.EntityFramework.Repositories.Interfaces
{
    public interface ITenantPasswordPolicyRepository
    {
        Task<PasswordPolicy> FindByTenantAsync(string tenantId);

        Task<IdentityResult> UpdateAsync(string tenantId, PasswordPolicy policy);
    }
}