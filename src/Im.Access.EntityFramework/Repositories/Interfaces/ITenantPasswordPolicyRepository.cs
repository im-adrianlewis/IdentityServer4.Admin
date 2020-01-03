using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Im.Access.EntityFramework.Entities;

namespace Im.Access.EntityFramework.Repositories.Interfaces
{
    public interface ITenantPasswordPolicyRepository
    {
        Task<PasswordPolicy> FindByTenantAsync(string tenantId);

        Task<IdentityResult> UpdateAsync(string tenantId, PasswordPolicy policy);
    }
}