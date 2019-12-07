using Microsoft.EntityFrameworkCore;
using Skoruba.IdentityServer4.Admin.EntityFramework.Entities;
using Skoruba.IdentityServer4.Admin.EntityFramework.Interfaces;

namespace Skoruba.IdentityServer4.Admin.EntityFramework.Shared.DbContexts
{
    public class AdminTenantConfigDbContext : DbContext, IAdminTenantConfigDbContext
    {
        public AdminTenantConfigDbContext(DbContextOptions<AdminTenantConfigDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TenantConfiguration> TenantConfigurations { get; set; }
        
        public virtual DbSet<ExternalProvider> ExternalProviders { get; set; }
        
        public virtual DbSet<PasswordPolicy> PasswordPolicies { get; set; }
        
        public virtual DbSet<ClaimType> ClaimTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<TenantConfiguration>(
                b =>
                {
                    b.Property(e => e.Id).ValueGeneratedOnAdd();
                    b.Property(e => e.Tenant).IsRequired().HasMaxLength(200);
                    b.Property(e => e.EmailVerification).IsRequired();
                    b.Property(e => e.FirstPartyRequired).IsRequired();
                    b.Property(e => e.IsSecondPageRegistration).IsRequired();
                    b.Property(e => e.SecondPageRegistrationUrl).HasMaxLength(500);
                    b.Property(e => e.ContactUsUrl).HasMaxLength(200);
                    b.Property(e => e.SiteUrl).HasMaxLength(200);
                    b.Property(e => e.LoginUrl).HasMaxLength(200);
                    b.Property(e => e.GoogleTagManagerId).HasMaxLength(100);
                    b.Property(e => e.OptimizelyId).HasMaxLength(100);
                    b.Property(e => e.PermutiveProjectId).HasMaxLength(100);
                    b.Property(e => e.PermutiveApiKey).HasMaxLength(100);
                    b.Property(e => e.RegisterUrl).HasMaxLength(100);
                    b.Property(e => e.IsVanityUrl).IsRequired();
                    b.Property(e => e.ExternalRegistrationUrl).HasMaxLength(100);
                    b.Property(e => e.StyleManifestUrl).HasMaxLength(100);
                    b.Property(e => e.GoogleAnalyticsId).HasMaxLength(100);
                    b.Property(e => e.ReCaptchaSiteKey).HasMaxLength(100);
                    b.Property(e => e.ReCaptchaSecretKey).HasMaxLength(100);
                    b.Property(e => e.IsReCaptchaEnabled).IsRequired();
                    b.HasKey(e => e.Id);
                    b.HasIndex(e => e.Tenant).HasName("IX_Tenant").IsUnique();
                    b.HasMany(e => e.ClaimTypes)
                        .WithOne(e => e.TenantConfiguration)
                        .HasForeignKey(e => e.TenantConfigurationId);
                    b.HasMany(e => e.ExternalProviders)
                        .WithOne(e => e.TenantConfiguration)
                        .HasForeignKey(e => e.TenantConfigurationId);
                    b.HasOne(e => e.PasswordPolicy)
                        .WithOne(e => e.TenantConfiguration)
                        .HasForeignKey<PasswordPolicy>(e => e.TenantConfigurationId);
                });

            builder.Entity<ClaimType>(
                b =>
                {
                    b.Property(e => e.Id).ValueGeneratedOnAdd();
                    b.Property(e => e.TenantConfigurationId).IsRequired();
                    b.Property(e => e.TypeName).HasMaxLength(200).IsRequired();
                    b.Property(e => e.Text).HasMaxLength(255);
                    b.Property(e => e.Lead).HasMaxLength(255);
                    b.Property(e => e.IsVisibleOnRegistration).IsRequired();
                    b.Property(e => e.SortOrder).IsRequired();
                    b.HasIndex(e => new {e.TenantConfigurationId, e.TypeName}).HasName("IX_Tenant_TypeName").IsUnique();
                });

            builder.Entity<ExternalProvider>(
                b =>
                {
                    b.Property(e => e.Id).ValueGeneratedOnAdd();
                    b.Property(e => e.TenantConfigurationId).IsRequired();
                    b.Property(e => e.Enabled).IsRequired();
                    b.Property(e => e.ConsumerKey).HasMaxLength(200).IsRequired();
                    b.Property(e => e.Secret).HasMaxLength(200).IsRequired();
                    b.Property(e => e.AuthenticationType).HasMaxLength(200).IsRequired();
                    b.Property(e => e.Caption).HasMaxLength(200).IsRequired();
                    b.Property(e => e.Callback).HasMaxLength(200).IsRequired();
                });

            builder.Entity<PasswordPolicy>(
                b =>
                {
                    b.Property(e => e.Id).ValueGeneratedOnAdd();
                    b.Property(e => e.TenantConfigurationId).IsRequired();
                    b.Property(e => e.RequireDigit).IsRequired();
                    b.Property(e => e.RequireUpperCase).IsRequired();
                    b.Property(e => e.RequireLowerCase).IsRequired();
                    b.Property(e => e.RequireNonAlphaNumeric).IsRequired();
                    b.Property(e => e.MinimumLength).IsRequired();
                });
        }
    }
}
