using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Skoruba.IdentityServer4.Admin.EntityFramework.Shared.Constants;
using Skoruba.IdentityServer4.Admin.EntityFramework.Shared.Entities.Identity;

namespace Skoruba.IdentityServer4.Admin.EntityFramework.Shared.DbContexts
{
    public class AdminIdentityDbContext : IdentityDbContext<UserIdentity, UserIdentityRole, string, UserIdentityUserClaim, UserIdentityUserRole, UserIdentityUserLogin, UserIdentityRoleClaim, UserIdentityUserToken>
    {
        public AdminIdentityDbContext(DbContextOptions<AdminIdentityDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            ConfigureIdentityContext(builder);
        }

        private void ConfigureIdentityContext(ModelBuilder builder)
        {
            builder.Entity<UserIdentity>(b =>
            {
                b.Property(e => e.TenantId).HasMaxLength(128).IsRequired();
                b.Property(u => u.RegistrationDate).IsRequired(false);
                b.Property(u => u.CreateDate).IsRequired();
                b.Property(u => u.LastUpdatedDate).IsRequired();
                b.Property(u => u.LastLoggedInDate).IsRequired(false);
                b.Property(u => u.RegistrationIpAddress).HasMaxLength(50).IsRequired(false);
                b.Property(u => u.LastLoggedInIpAddress).HasMaxLength(50).IsRequired(false);
                b.Property(u => u.Address1).HasMaxLength(255);
                b.Property(u => u.Address2).HasMaxLength(255);
                b.Property(u => u.City).HasMaxLength(50);
                b.Property(u => u.County).HasMaxLength(50);
                b.Property(u => u.Country).HasMaxLength(100);
                b.Property(u => u.Postcode).HasMaxLength(20);
                b.Property(u => u.UserBiography).HasMaxLength(4000);
                b.Property(u => u.FirstPartyIm);
                b.Property(e => e.ScreenName).HasMaxLength(100).IsRequired();
                b.Property(e => e.UserType).HasMaxLength(50).IsRequired(false);

                b.HasIndex(u => u.NormalizedUserName).HasName("UserNameIndex").IsUnique(false);
                b.HasIndex(u => u.NormalizedEmail).HasName("EmailIndex").IsUnique(false);

                b.HasIndex(u => new { u.TenantId, u.NormalizedUserName }).HasName("Tenant_UserNameIndex").IsUnique();
                b.HasIndex(u => new { u.TenantId, u.NormalizedEmail }).HasName("Tenant_EmailIndex").IsUnique();
                b.HasIndex(u => new { u.TenantId, u.ScreenName }).HasName("Tenant_ScreenNameIndex").IsUnique();
            });

            builder.Entity<UserIdentityRole>().ToTable(TableConsts.IdentityRoles);
            builder.Entity<UserIdentityRoleClaim>().ToTable(TableConsts.IdentityRoleClaims);
            builder.Entity<UserIdentityUserRole>().ToTable(TableConsts.IdentityUserRoles);

            builder.Entity<UserIdentity>().ToTable(TableConsts.IdentityUsers);
            builder.Entity<UserIdentityUserLogin>().ToTable(TableConsts.IdentityUserLogins);
            builder.Entity<UserIdentityUserClaim>(b =>
            {
                b.Property(u => u.ClaimUpdatedDate).IsRequired();
                b.ToTable(TableConsts.IdentityUserClaims);
            });
            builder.Entity<UserIdentityUserToken>().ToTable(TableConsts.IdentityUserTokens);
        }
    }
}