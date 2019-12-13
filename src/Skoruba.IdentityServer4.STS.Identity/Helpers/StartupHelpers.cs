﻿using System;
using System.Collections.Generic;
using System.Globalization;
using IdentityServer4.EntityFramework.Storage;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SendGrid;
using Serilog;
using Skoruba.IdentityServer4.STS.Identity.Configuration;
using Skoruba.IdentityServer4.STS.Identity.Configuration.ApplicationParts;
using Skoruba.IdentityServer4.STS.Identity.Configuration.Constants;
using Skoruba.IdentityServer4.STS.Identity.Configuration.Interfaces;
using Skoruba.IdentityServer4.STS.Identity.Helpers.Localization;
using Skoruba.IdentityServer4.STS.Identity.Services;
using ILogger = Microsoft.Extensions.Logging.ILogger;
using System.Linq;
using Microsoft.Extensions.Hosting;
using Skoruba.IdentityServer4.Admin.EntityFramework.Entities;
using Skoruba.IdentityServer4.Admin.EntityFramework.Interfaces;
using Skoruba.IdentityServer4.Admin.EntityFramework.MySql.Extensions;
using Skoruba.IdentityServer4.Admin.EntityFramework.PostgreSQL.Extensions;
using Skoruba.IdentityServer4.Admin.EntityFramework.Shared.Configuration;
using Skoruba.IdentityServer4.Admin.EntityFramework.Shared.Services;
using Skoruba.IdentityServer4.Admin.EntityFramework.SqlServer.Extensions;
using Skoruba.IdentityServer4.Admin.EntityFramework.Shared.Entities.Identity;

namespace Skoruba.IdentityServer4.STS.Identity.Helpers
{
    public static class StartupHelpers
    {
        /// <summary>
        /// Register services for MVC and localization including available languages
        /// </summary>
        /// <param name="services"></param>
        public static void AddMvcWithLocalization<TUser, TKey>(this IServiceCollection services, IConfiguration configuration)
            where TUser : IdentityUser<TKey>
            where TKey : IEquatable<TKey>
        {
            services.AddLocalization(opts => { opts.ResourcesPath = ConfigurationConsts.ResourcesPath; });

            services.TryAddTransient(typeof(IGenericControllerLocalizer<>), typeof(GenericControllerLocalizer<>));

            services.AddControllersWithViews(o =>
                {
                    o.Conventions.Add(new GenericControllerRouteConvention());
                })
                .AddViewLocalization(
                    LanguageViewLocationExpanderFormat.Suffix,
                    opts => { opts.ResourcesPath = ConfigurationConsts.ResourcesPath; })
                .AddDataAnnotationsLocalization()
                .ConfigureApplicationPartManager(m =>
                {
                    m.FeatureProviders.Add(new GenericTypeControllerFeatureProvider<TUser, TKey>());
                });

            var cultureConfiguration = configuration.GetSection(nameof(CultureConfiguration)).Get<CultureConfiguration>();
            services.Configure<RequestLocalizationOptions>(
                opts =>
                {
                    // If cultures are specified in the configuration, use them (making sure they are among the available cultures),
                    // otherwise use all the available cultures
                    var supportedCultureCodes = (cultureConfiguration?.Cultures?.Count > 0 ?
                        cultureConfiguration.Cultures.Intersect(CultureConfiguration.AvailableCultures) :
                        CultureConfiguration.AvailableCultures).ToArray();

                    if (!supportedCultureCodes.Any()) supportedCultureCodes = CultureConfiguration.AvailableCultures;
                    var supportedCultures = supportedCultureCodes.Select(c => new CultureInfo(c)).ToList();

                    // If the default culture is specified use it, otherwise use CultureConfiguration.DefaultRequestCulture ("en")
                    var defaultCultureCode = string.IsNullOrEmpty(cultureConfiguration?.DefaultCulture) ?
                        CultureConfiguration.DefaultRequestCulture : cultureConfiguration?.DefaultCulture;

                    // If the default culture is not among the supported cultures, use the first supported culture as default
                    if (!supportedCultureCodes.Contains(defaultCultureCode)) defaultCultureCode = supportedCultureCodes.FirstOrDefault();

                    opts.DefaultRequestCulture = new RequestCulture(defaultCultureCode);
                    opts.SupportedCultures = supportedCultures;
                    opts.SupportedUICultures = supportedCultures;
                });
        }

        /// <summary>
        /// Using of Forwarded Headers and Referrer Policy
        /// </summary>
        /// <param name="app"></param>
        public static void UseSecurityHeaders(this IApplicationBuilder app)
        {
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseHsts(options => options.MaxAge(days: 365));
            app.UseReferrerPolicy(options => options.NoReferrer());
        }

        /// <summary>
        /// Add email senders - configuration of sendgrid, smtp senders
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void AddEmailSenders(this IServiceCollection services, IConfiguration configuration)
        {
            var smtpConfiguration = configuration.GetSection(nameof(SmtpConfiguration)).Get<SmtpConfiguration>();
            var sendGridConfiguration = configuration.GetSection(nameof(SendgridConfiguration)).Get<SendgridConfiguration>();

            if (sendGridConfiguration != null && !string.IsNullOrWhiteSpace(sendGridConfiguration.ApiKey))
            {
                services.AddSingleton<ISendGridClient>(_ => new SendGridClient(sendGridConfiguration.ApiKey));
                services.AddSingleton(sendGridConfiguration);
                services.AddTransient<IEmailSender, SendgridEmailSender>();
            }
            else if (smtpConfiguration != null && !string.IsNullOrWhiteSpace(smtpConfiguration.Host))
            {
                services.AddSingleton(smtpConfiguration);
                services.AddTransient<IEmailSender, SmtpEmailSender>();
            }
            else
            {
                services.AddSingleton<IEmailSender, EmailSender>();
            }
        }

        /// <summary>
        /// Register DbContexts for IdentityServer ConfigurationStore and PersistedGrants and Identity
        /// Configure the connection strings in AppSettings.json
        /// </summary>
        /// <typeparam name="TConfigurationDbContext"></typeparam>
        /// <typeparam name="TPersistedGrantDbContext"></typeparam>
        /// <typeparam name="TIdentityDbContext"></typeparam>
        /// <typeparam name="TTenantConfigDbContext"></typeparam>
        /// <param name="services"></param>
        /// <param name="environment"></param>
        /// <param name="configuration"></param>
        public static void RegisterDbContexts<TIdentityDbContext, TConfigurationDbContext, TPersistedGrantDbContext, TTenantConfigDbContext>(this IServiceCollection services, IWebHostEnvironment environment, IConfiguration configuration)
            where TIdentityDbContext : DbContext
            where TPersistedGrantDbContext : DbContext, IAdminPersistedGrantDbContext
            where TConfigurationDbContext : DbContext, IAdminConfigurationDbContext
            where TTenantConfigDbContext : DbContext, IAdminTenantConfigDbContext
        {
            if (environment.IsStaging())
            {
                services.RegisterDbContextsInMemory<TIdentityDbContext, TConfigurationDbContext, TPersistedGrantDbContext, TTenantConfigDbContext>();
            }
            else
            {
                services.RegisterDbContexts<TIdentityDbContext, TConfigurationDbContext, TPersistedGrantDbContext, TTenantConfigDbContext>(configuration);
            }
        }


        /// <summary>
        /// Register DbContexts for IdentityServer ConfigurationStore and PersistedGrants and Identity
        /// Configure the connection strings in AppSettings.json
        /// </summary>
        /// <typeparam name="TConfigurationDbContext"></typeparam>
        /// <typeparam name="TPersistedGrantDbContext"></typeparam>
        /// <typeparam name="TIdentityDbContext"></typeparam>
        /// <typeparam name="TTenantConfigDbContext"></typeparam>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void RegisterDbContexts<TIdentityDbContext, TConfigurationDbContext, TPersistedGrantDbContext, TTenantConfigDbContext>(this IServiceCollection services, IConfiguration configuration)
            where TIdentityDbContext : DbContext
            where TPersistedGrantDbContext : DbContext, IAdminPersistedGrantDbContext
            where TConfigurationDbContext : DbContext, IAdminConfigurationDbContext
            where TTenantConfigDbContext : DbContext, IAdminTenantConfigDbContext
        {
            var databaseProvider = configuration.GetSection(nameof(DatabaseProviderConfiguration)).Get<DatabaseProviderConfiguration>();

            var identityConnectionString = configuration.GetConnectionString(ConfigurationConsts.IdentityDbConnectionStringKey);
            var configurationConnectionString = configuration.GetConnectionString(ConfigurationConsts.ConfigurationDbConnectionStringKey);
            var persistedGrantsConnectionString = configuration.GetConnectionString(ConfigurationConsts.PersistedGrantDbConnectionStringKey);
            var tenantConfigConnectionString = configuration.GetConnectionString(ConfigurationConsts.AdminTenantConfigDbConnectionStringKey);

            switch (databaseProvider.ProviderType)
            {
                case DatabaseProviderType.SqlServer:
                    services.RegisterSqlServerDbContexts<TIdentityDbContext, TConfigurationDbContext, TPersistedGrantDbContext, TTenantConfigDbContext>(identityConnectionString, configurationConnectionString, persistedGrantsConnectionString, tenantConfigConnectionString);
                    break;
                case DatabaseProviderType.PostgreSQL:
                    services.RegisterNpgSqlDbContexts<TIdentityDbContext, TConfigurationDbContext, TPersistedGrantDbContext, TTenantConfigDbContext>(identityConnectionString, configurationConnectionString, persistedGrantsConnectionString, tenantConfigConnectionString);
                    break;
                case DatabaseProviderType.MySql:
                    services.RegisterMySqlDbContexts<TIdentityDbContext, TConfigurationDbContext, TPersistedGrantDbContext, TTenantConfigDbContext>(identityConnectionString, configurationConnectionString, persistedGrantsConnectionString, tenantConfigConnectionString);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(databaseProvider.ProviderType), $@"The value needs to be one of {string.Join(", ", Enum.GetNames(typeof(DatabaseProviderType)))}.");
            }
        }

        /// <summary>
        /// Register InMemory DbContexts for IdentityServer ConfigurationStore and PersistedGrants and Identity
        /// Configure the connection strings in AppSettings.json
        /// </summary>
        /// <typeparam name="TConfigurationDbContext"></typeparam>
        /// <typeparam name="TPersistedGrantDbContext"></typeparam>
        /// <typeparam name="TIdentityDbContext"></typeparam>
        /// <typeparam name="TTenantConfigDbContext"></typeparam>
        /// <param name="services"></param>
        public static void RegisterDbContextsInMemory<TIdentityDbContext, TConfigurationDbContext, TPersistedGrantDbContext, TTenantConfigDbContext>(
            this IServiceCollection services)
            where TIdentityDbContext : DbContext
            where TPersistedGrantDbContext : DbContext, IAdminPersistedGrantDbContext
            where TConfigurationDbContext : DbContext, IAdminConfigurationDbContext
            where TTenantConfigDbContext : DbContext, IAdminTenantConfigDbContext
        {
            var identityDatabaseName = Guid.NewGuid().ToString();
            var configurationDatabaseName = Guid.NewGuid().ToString();
            var operationalDatabaseName = Guid.NewGuid().ToString();
            var tenantConfigDatabaseName = Guid.NewGuid().ToString();

            services.AddDbContext<TIdentityDbContext>(optionsBuilder =>
                optionsBuilder.UseInMemoryDatabase(identityDatabaseName));

            services.AddConfigurationDbContext<TConfigurationDbContext>(options =>
            {
                options.ConfigureDbContext = b => b.UseInMemoryDatabase(configurationDatabaseName);
            });

            services.AddOperationalDbContext<TPersistedGrantDbContext>(options =>
            {
                options.ConfigureDbContext = b => b.UseInMemoryDatabase(operationalDatabaseName);
            });

            services.AddDbContext<TTenantConfigDbContext>(optionsBuilder =>
                optionsBuilder.UseInMemoryDatabase(tenantConfigDatabaseName));
        }

        /// <summary>
        /// Add services for authentication, including Identity model, IdentityServer4 and external providers
        /// </summary>
        /// <typeparam name="TIdentityDbContext">DbContext for Identity</typeparam>
        /// <typeparam name="TUserIdentity">User Identity class</typeparam>
        /// <typeparam name="TUserIdentityRole">User Identity Role class</typeparam>
        /// <typeparam name="TConfigurationDbContext"></typeparam>
        /// <typeparam name="TPersistedGrantDbContext"></typeparam>
        /// <typeparam name="TTenantConfigDbContext"></typeparam>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void AddAuthenticationServices<TConfigurationDbContext, TPersistedGrantDbContext, TIdentityDbContext, TTenantConfigDbContext, TUserIdentity, TUserIdentityRole>(this IServiceCollection services, IConfiguration configuration) 
            where TIdentityDbContext : DbContext
            where TPersistedGrantDbContext : DbContext, IAdminPersistedGrantDbContext
            where TConfigurationDbContext : DbContext, IAdminConfigurationDbContext
            where TTenantConfigDbContext : DbContext, IAdminTenantConfigDbContext
            where TUserIdentity : UserIdentity
            where TUserIdentityRole : IdentityRole<string>
        {
            var loginConfiguration = GetLoginConfiguration(configuration);
            var registrationConfiguration = GetRegistrationConfiguration(configuration);

            services
                .AddSingleton(registrationConfiguration)
                .AddSingleton(loginConfiguration)
                .AddScoped<UserResolver<TUserIdentity>>()
                .AddIdentity<TUserIdentity, TUserIdentityRole>(options =>
                {
                    options.User.RequireUniqueEmail = true;
                    options.Lockout = new AdvancedLockoutOptions();
                })
                .AddUserManager<MultiTenantUserManager>()
                .AddUserStore<MultiTenantUserStore<TUserIdentity, TUserIdentityRole, TIdentityDbContext, UserIdentityUserClaim, UserIdentityUserRole, UserIdentityUserLogin, UserIdentityUserToken, UserIdentityRoleClaim>>()
                .AddEntityFrameworkStores<TIdentityDbContext>()
                .AddDefaultTokenProviders();

            services.Configure<IISOptions>(iis =>
            {
                iis.AuthenticationDisplayName = "Windows";
                iis.AutomaticAuthentication = false;
            });

            var authenticationBuilder = services.AddAuthentication();

            AddExternalProviders(authenticationBuilder, configuration);
            AddMultiTenantExternalProviders<TTenantConfigDbContext>(authenticationBuilder, services);

            AddIdentityServer<TConfigurationDbContext, TPersistedGrantDbContext, TUserIdentity>(services, configuration);
        }

        /// <summary>
        /// Get configuration for login
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        private static LoginConfiguration GetLoginConfiguration(IConfiguration configuration)
        {
            var loginConfiguration = configuration.GetSection(nameof(LoginConfiguration)).Get<LoginConfiguration>();

            // Cannot load configuration - use default configuration values
            if (loginConfiguration == null)
            {
                return new LoginConfiguration();
            }

            return loginConfiguration;
        }

        /// <summary>
        /// Get configuration for registration
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        private static RegisterConfiguration GetRegistrationConfiguration(IConfiguration configuration)
        {
            var registerConfiguration = configuration.GetSection(nameof(RegisterConfiguration)).Get<RegisterConfiguration>();

            // Cannot load configuration - use default configuration values
            if (registerConfiguration == null)
            {
                return new RegisterConfiguration();
            }

            return registerConfiguration;
        }

        /// <summary>
        /// Add configuration for IdentityServer4
        /// </summary>
        /// <typeparam name="TUserIdentity"></typeparam>
        /// <typeparam name="TConfigurationDbContext"></typeparam>
        /// <typeparam name="TPersistedGrantDbContext"></typeparam>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        private static void AddIdentityServer<TConfigurationDbContext, TPersistedGrantDbContext, TUserIdentity>(
            IServiceCollection services,
            IConfiguration configuration)
            where TPersistedGrantDbContext : DbContext, IAdminPersistedGrantDbContext
            where TConfigurationDbContext : DbContext, IAdminConfigurationDbContext
            where TUserIdentity : class
        {
            var builder = services.AddIdentityServer(options =>
                {
                    options.Events.RaiseErrorEvents = true;
                    options.Events.RaiseInformationEvents = true;
                    options.Events.RaiseFailureEvents = true;
                    options.Events.RaiseSuccessEvents = true;
                })
                .AddConfigurationStore<TConfigurationDbContext>()
                .AddOperationalStore<TPersistedGrantDbContext>()
                .AddAspNetIdentity<TUserIdentity>();

            builder.AddCustomSigningCredential(configuration);
            builder.AddCustomValidationKey(configuration);
        }

        /// <summary>
        /// Add external providers
        /// </summary>
        /// <param name="authenticationBuilder"></param>
        /// <param name="configuration"></param>
        private static void AddExternalProviders(
            AuthenticationBuilder authenticationBuilder,
            IConfiguration configuration)
        {
            var externalProviderConfiguration = configuration.GetSection(nameof(ExternalProvidersConfiguration)).Get<ExternalProvidersConfiguration>();

            if (externalProviderConfiguration.UseGitHubProvider)
            {
                authenticationBuilder.AddGitHub(options =>
                {
                    options.ClientId = externalProviderConfiguration.GitHubClientId;
                    options.ClientSecret = externalProviderConfiguration.GitHubClientSecret;
                    options.Scope.Add("user:email");
                });
            }
        }

        private static void AddMultiTenantExternalProviders<TTenantConfigurationDbContext>(
            AuthenticationBuilder authenticationBuilder, IServiceCollection services)
            where TTenantConfigurationDbContext : DbContext, IAdminTenantConfigDbContext
        {
            var providerFactory = new DefaultServiceProviderFactory();
            var sp = providerFactory.CreateServiceProvider(services);
            try
            {
                var context = sp.GetRequiredService<TTenantConfigurationDbContext>();
                var externalProviders = context
                    .ExternalProviders
                    .Where(e => e.Enabled)
                    .ToList();

                AddFacebookProviders(authenticationBuilder, externalProviders.Where(e => e.Caption == "Facebook"));
                AddGoogleProviders(authenticationBuilder, externalProviders.Where(e => e.Caption == "Google"));
                AddTwitterProviders(authenticationBuilder, externalProviders.Where(e => e.Caption == "Twitter"));
            }
            finally
            {
                var disp = sp as IDisposable;
                disp?.Dispose();
            }
        }

        private static void AddFacebookProviders(AuthenticationBuilder authenticationBuilder, IEnumerable<ExternalProvider> externalProviders)
        {
            // TODO: Add code to handle failures
            foreach (var externalProvider in externalProviders)
            {
                authenticationBuilder.AddFacebook(
                    externalProvider.AuthenticationType,
                    options =>
                    {
                        options.AppId = externalProvider.ConsumerKey;
                        options.AppSecret = externalProvider.Secret;
                        options.CallbackPath = externalProvider.Callback;
                        options.Scope.Add("public_profile");
                        options.Scope.Add("email");
                    });
            }
        }

        private static void AddGoogleProviders(AuthenticationBuilder authenticationBuilder, IEnumerable<ExternalProvider> externalProviders)
        {
            // TODO: Add code to handle failures
            foreach (var externalProvider in externalProviders)
            {
                authenticationBuilder.AddGoogle(
                    externalProvider.AuthenticationType,
                    options =>
                    {
                        options.ClientId = externalProvider.ConsumerKey;
                        options.ClientSecret = externalProvider.Secret;
                        options.CallbackPath = externalProvider.Callback;
                        options.Scope.Add("profile");
                        options.Scope.Add("email");
                    });
            }
        }

        private static void AddTwitterProviders(AuthenticationBuilder authenticationBuilder, IEnumerable<ExternalProvider> externalProviders)
        {
            // TODO: Add code to handle failures
            foreach (var externalProvider in externalProviders)
            {
                authenticationBuilder.AddTwitter(
                    externalProvider.AuthenticationType,
                    options =>
                    {
                        options.ConsumerKey = externalProvider.ConsumerKey;
                        options.ConsumerSecret = externalProvider.Secret;
                        options.CallbackPath = externalProvider.Callback;
                    });
            }
        }

        /// <summary>
        /// Register middleware for localization
        /// </summary>
        /// <param name="app"></param>
        public static void UseMvcLocalizationServices(this IApplicationBuilder app)
        {
            var options = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(options.Value);
        }

        /// <summary>
        /// Add authorization policies
        /// </summary>
        /// <param name="services"></param>
        /// <param name="rootConfiguration"></param>
        public static void AddAuthorizationPolicies(this IServiceCollection services,
                IRootConfiguration rootConfiguration)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy(AuthorizationConsts.AdministrationPolicy,
                    policy => policy.RequireRole(rootConfiguration.AdminConfiguration.AdministrationRole));
            });
        }
    }
}
