using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic;
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
            get => GetTenantStore().TenantId;
            set => GetTenantStore().TenantId = value;
        }

        public virtual bool SupportsTenant => Store is IUserTenantStore<UserIdentity>;

        public virtual bool SupportsFirstName => Store is IUserFirstNameStore<UserIdentity>;

        public virtual bool SupportsLastName => Store is IUserLastNameStore<UserIdentity>;

        public virtual bool SupportsScreenName => Store is IUserScreenNameStore<UserIdentity>;

        public override Task<IdentityResult> CreateAsync(UserIdentity user)
        {
            user.CreateDate = user.LastUpdatedDate = DateTimeOffset.UtcNow;
            return base.CreateAsync(user);
        }

        public override Task<IdentityResult> CreateAsync(UserIdentity user, string password)
        {
            user.CreateDate = user.LastUpdatedDate = DateTimeOffset.UtcNow;
            return base.CreateAsync(user, password);
        }

        public override Task<IdentityResult> UpdateAsync(UserIdentity user)
        {
            user.LastUpdatedDate = DateTimeOffset.UtcNow;
            return base.UpdateAsync(user);
        }

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

        public override async Task<IdentityResult> AccessFailedAsync(UserIdentity user)
        {
            var result = await base.AccessFailedAsync(user);
            if (user.LockoutEnabled)
            {
                if (user.LockoutEnd.HasValue && user.LockoutEnd > DateTimeOffset.UtcNow)
                {
                    return result;
                }

                var advancedLockoutOptions = Options.Lockout as AdvancedLockoutOptions;
                if (advancedLockoutOptions != null &&
                    user.AccessFailedCount >= advancedLockoutOptions.MaxFailedAttemptsBeforeWarning)
                {
                    var attemptsRemaining = Options.Lockout.MaxFailedAccessAttempts - user.AccessFailedCount;
                    var errorMessage = attemptsRemaining > 1
                        ? $"Account will be locked out after {attemptsRemaining} attempts"
                        : "Account will be locked out after 1 attempt";
                    return IdentityResult.Failed(
                        new IdentityError
                        {
                            Code = "UM0001",
                            Description = errorMessage
                        });
                }
            }

            return result;
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

    public class AdvancedLockoutOptions : LockoutOptions
    {
        public int MaxFailedAttemptsBeforeWarning { get; set; }
    }
}
