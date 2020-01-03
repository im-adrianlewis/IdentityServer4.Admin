using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Im.Access.EntityFramework.Shared.Services
{
    public interface IUserFirstNameStore<TUser> : IUserStore<TUser>
        where TUser : class
    {
        Task<string> GetFirstNameAsync(TUser user);

        Task<bool> SetFirstNameAsync(TUser user, string firstName);
    }
}