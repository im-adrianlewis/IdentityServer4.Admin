using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Skoruba.IdentityServer4.Admin.EntityFramework.Shared.Services
{
    public interface IUserLastNameStore<TUser> : IUserStore<TUser>
        where TUser : class
    {
        Task<string> GetLastNameAsync(TUser user);

        Task<bool> SetLastNameAsync(TUser user, string lastName);
    }
}