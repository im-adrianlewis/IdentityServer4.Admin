using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Skoruba.IdentityServer4.Admin.EntityFramework.Shared.Services
{
    public interface IUserScreenNameStore<TUser> : IUserStore<TUser>
        where TUser : class
    {
        Task<string> GetScreenNameAsync(TUser user);

        Task<bool> SetScreenNameAsync(TUser user, string screenName);

        Task<TUser> FindByScreenNameAsync(string screenName);
    }
}