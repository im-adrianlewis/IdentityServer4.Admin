using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Im.Access.EntityFramework.Shared.Services
{
    public interface IUserLastNameStore<TUser> : IUserStore<TUser>
        where TUser : class
    {
        Task<string> GetLastNameAsync(TUser user);

        Task<bool> SetLastNameAsync(TUser user, string lastName);
    }
}