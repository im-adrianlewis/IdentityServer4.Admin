using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Im.Access.EntityFramework.Shared.Services
{
    public interface IUserTenantStore<TUser> : IUserStore<TUser>
        where TUser : class
    {
        string TenantId { get; set; }

        Task<string> GetTenantAsync(TUser user);

        Task<bool> SetTenantAsync(TUser user, string tenantId);
    }
}