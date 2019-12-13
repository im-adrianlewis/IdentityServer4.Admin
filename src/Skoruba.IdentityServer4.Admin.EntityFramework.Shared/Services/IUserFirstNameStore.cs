using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Skoruba.IdentityServer4.Admin.EntityFramework.Shared.Services
{
    public interface IUserFirstNameStore<TUser> : IUserStore<TUser>
        where TUser : class
    {
        Task<string> GetFirstNameAsync(TUser user);

        Task<bool> SetFirstNameAsync(TUser user, string firstName);
    }
}