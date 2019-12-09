using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Skoruba.IdentityServer4.Admin.EntityFramework.Shared.Entities.Identity;

namespace Skoruba.IdentityServer4.Admin.EntityFramework.Shared.Services
{
    public class MultiTenantUserManager : UserManager<UserIdentity>
    {
        public MultiTenantUserManager(
            IUserStore<UserIdentity> store,
            IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<UserIdentity> passwordHasher,
            IEnumerable<IUserValidator<UserIdentity>> userValidators,
            IEnumerable<IPasswordValidator<UserIdentity>> passwordValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            IServiceProvider services,
            ILogger<UserManager<UserIdentity>> logger) : 
            base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
        }

        public virtual string TenantId
        {
            get { return GetTenantStore().TenantId; }
            set { GetTenantStore().TenantId = value; }
        }

        public virtual bool SupportsTenant => Store is IUserTenantStore<UserIdentity>;

        public virtual bool SupportsFirstName => Store is IUserFirstNameStore<UserIdentity>;

        public virtual bool SupportsLastName => Store is IUserLastNameStore<UserIdentity>;

        public virtual bool SupportsScreenName => Store is IUserScreenNameStore<UserIdentity>;

        public virtual async Task<string> GetFirstNameAsync(string userId)
        {
            var store = GetFirstNameStore();
            var user = await FindByIdAsync(userId).ConfigureAwait(false);
            if (user == null)
            {
                throw new InvalidOperationException("User not found.");
            }
            return await store.GetFirstNameAsync(user).ConfigureAwait(false);
        }

        public virtual async Task<IdentityResult> SetFirstNameAsync(string userId, string firstName)
        {
            var store = GetFirstNameStore();
            var user = await FindByIdAsync(userId).ConfigureAwait(false);
            if (user == null)
            {
                throw new InvalidOperationException("User not found.");
            }

            await store.SetFirstNameAsync(user, firstName).ConfigureAwait(false);
            return await UpdateAsync(user).ConfigureAwait(false);
        }

        public virtual async Task<string> GetLastNameAsync(string userId)
        {
            var store = GetLastNameStore();
            var user = await FindByIdAsync(userId).ConfigureAwait(false);
            if (user == null)
            {
                throw new InvalidOperationException("User not found.");
            }
            return await store.GetLastNameAsync(user).ConfigureAwait(false);
        }

        public virtual async Task<IdentityResult> SetLastNameAsync(string userId, string lastName)
        {
            var store = GetLastNameStore();
            var user = await FindByIdAsync(userId).ConfigureAwait(false);
            if (user == null)
            {
                throw new InvalidOperationException("User not found.");
            }

            await store.SetLastNameAsync(user, lastName).ConfigureAwait(false);
            return await UpdateAsync(user).ConfigureAwait(false);
        }

        public virtual async Task<string> GetScreenNameAsync(string userId)
        {
            var store = GetScreenNameStore();
            var user = await FindByIdAsync(userId).ConfigureAwait(false);
            if (user == null)
            {
                throw new InvalidOperationException("User not found.");
            }
            return await store.GetScreenNameAsync(user).ConfigureAwait(false);
        }

        internal IUserFirstNameStore<UserIdentity> GetFirstNameStore()
        {
            var store = Store as IUserFirstNameStore<UserIdentity>;
            if (store == null)
            {
                throw new NotSupportedException("Store is not IUserFirstNameStore<>");
            }
            return store;
        }

        internal IUserLastNameStore<UserIdentity> GetLastNameStore()
        {
            var store = Store as IUserLastNameStore<UserIdentity>;
            if (store == null)
            {
                throw new NotSupportedException("Store is not IUserLastNameStore<>");
            }
            return store;
        }

        internal IUserScreenNameStore<UserIdentity> GetScreenNameStore()
        {
            var store = Store as IUserScreenNameStore<UserIdentity>;
            if (store == null)
            {
                throw new NotSupportedException("Store is not IUserScreenNameStore<>");
            }
            return store;
        }

        internal IUserTenantStore<UserIdentity> GetTenantStore()
        {
            var store = Store as IUserTenantStore<UserIdentity>;
            if (store == null)
            {
                throw new NotSupportedException("Store is not IUserTenantStore<>");
            }
            return store;
        }
    }

    public enum UserType
    {
        Registered,
        Ghost
    }

    public class MultiTenantUserStore<TUser, TRole, TContext, TUserClaim, TUserRole, TUserLogin, TUserToken, TRoleClaim>
        : UserStore<TUser, TRole, TContext, string, TUserClaim, TUserRole, TUserLogin, TUserToken, TRoleClaim>,
            IUserTenantStore<TUser>,
            IUserScreenNameStore<TUser>,
            IUserFirstNameStore<TUser>,
            IUserLastNameStore<TUser>
        where TUser : UserIdentity
        where TRole : IdentityRole<string>
        where TContext : DbContext
        where TUserClaim : IdentityUserClaim<string>, new()
        where TUserRole : IdentityUserRole<string>, new()
        where TUserLogin : IdentityUserLogin<string>, new()
        where TUserToken : IdentityUserToken<string>, new()
        where TRoleClaim : IdentityRoleClaim<string>, new()
    {
        private DbSet<TUserLogin> _logins;

        public MultiTenantUserStore(TContext context, IdentityErrorDescriber describer = null) : base(context, describer)
        {
        }

        public virtual string TenantId { get; set; }

        private DbSet<TUserLogin> UserLogins => _logins ?? (_logins = Context.Set<TUserLogin>());

        public override Task<IdentityResult> CreateAsync(TUser user, CancellationToken cancellationToken = new CancellationToken())
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (user.TenantId == null)
            {
                if (string.IsNullOrEmpty(TenantId))
                {
                    TenantId = "Immediate";
                }

                ThrowIfTenantIdInvalid();
                user.TenantId = TenantId;
            }

            if (string.IsNullOrEmpty(user.ScreenName))
            {
                user.ScreenName = Guid.NewGuid().ToString("N");
            }

            var now = DateTimeOffset.UtcNow;
            user.CreateDate = user.LastUpdatedDate = now;

            if (user.UserType != UserType.Ghost.ToString() && user.RegistrationDate == null)
            {
                user.RegistrationDate = now;
            }
            
            return base.CreateAsync(user, cancellationToken);
        }

        public override Task<IdentityResult> UpdateAsync(TUser user, CancellationToken cancellationToken = new CancellationToken())
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            
            // Update last updated date
            var now = DateTimeOffset.UtcNow;
            user.LastUpdatedDate = now;

            // Ensure registration date is set when necessary
            if (user.UserType != UserType.Ghost.ToString() && user.RegistrationDate == null)
            {
                user.RegistrationDate = now;
            }

            return base.UpdateAsync(user, cancellationToken);
        }

        public override Task<TUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken = new CancellationToken())
        {
            return Users.FirstOrDefaultAsync(
                u => u.NormalizedUserName == normalizedUserName && u.TenantId == TenantId, cancellationToken);
        }

        public override Task<TUser> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken = new CancellationToken())
        {
            return Users.FirstOrDefaultAsync(
                u => u.NormalizedEmail == normalizedEmail && u.TenantId == TenantId, cancellationToken);
        }

        public override async Task<TUser> FindByLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken = new CancellationToken())
        {
            var userLogin = await UserLogins.SingleOrDefaultAsync(
                l => l.LoginProvider == loginProvider && l.ProviderKey == providerKey, cancellationToken);

            return await FindByIdAsync(userLogin.UserId, cancellationToken);
        }

        /// <summary>
        /// Gets the tenant for the specified user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        public virtual Task<string> GetTenantAsync(TUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            return Task.FromResult(user.TenantId);
        }

        /// <summary>
        /// Sets the tenant for the specified user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="tenantId">The tenant identifier.</param>
        /// <returns></returns>
        public virtual Task<bool> SetTenantAsync(TUser user, string tenantId)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            user.TenantId = tenantId;
            return Task.FromResult(true);
        }

        /// <summary>
        /// Gets the screen name from the specified user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">user</exception>
        public virtual Task<string> GetScreenNameAsync(TUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            return Task.FromResult(user.ScreenName);
        }

        /// <summary>
        /// Sets the screen name for the specified user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="screenName">Name of the screen.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">user</exception>
        public virtual Task<bool> SetScreenNameAsync(TUser user, string screenName)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            user.ScreenName = screenName;
            return Task.FromResult(true);
        }

        /// <summary>
        /// Finds the user by screen name.
        /// </summary>
        /// <param name="screenName">Name of the screen.</param>
        /// <returns></returns>
        public virtual Task<TUser> FindByScreenNameAsync(string screenName)
        {
            ThrowIfTenantIdInvalid();

            return Users.FirstOrDefaultAsync(
                u =>
                u.ScreenName.ToUpper() == screenName.ToUpper() && 
                u.TenantId.Equals(TenantId));
        }

        public Task<string> GetFirstNameAsync(TUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            return Task.FromResult(user.FirstName);
        }

        public Task<bool> SetFirstNameAsync(TUser user, string firstName)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            user.FirstName = firstName;
            return Task.FromResult(true);
        }

        public Task<string> GetLastNameAsync(TUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            return Task.FromResult(user.LastName);
        }

        public Task<bool> SetLastNameAsync(TUser user, string lastName)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            user.LastName = lastName;
            return Task.FromResult(true);
        }

        private void ThrowIfTenantIdInvalid()
        {
            if (EqualityComparer<string>.Default.Equals(TenantId, default(string)))
                throw new InvalidOperationException("The TenantId has not been set.");
        }
    }

    public interface IUserFirstNameStore<TUser> : IUserStore<TUser>
        where TUser : class
    {
        Task<string> GetFirstNameAsync(TUser user);

        Task<bool> SetFirstNameAsync(TUser user, string firstName);
    }

    public interface IUserLastNameStore<TUser> : IUserStore<TUser>
        where TUser : class
    {
        Task<string> GetLastNameAsync(TUser user);

        Task<bool> SetLastNameAsync(TUser user, string lastName);
    }

    public interface IUserScreenNameStore<TUser> : IUserStore<TUser>
        where TUser : class
    {
        Task<string> GetScreenNameAsync(TUser user);

        Task<bool> SetScreenNameAsync(TUser user, string screenName);

        Task<TUser> FindByScreenNameAsync(string screenName);
    }

    public interface IUserTenantStore<TUser> : IUserStore<TUser>
        where TUser : class
    {
        string TenantId { get; set; }

        Task<string> GetTenantAsync(TUser user);

        Task<bool> SetTenantAsync(TUser user, string tenantId);
    }
}
