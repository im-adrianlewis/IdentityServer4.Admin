﻿using System;
using System.Reflection;
using IdentityServer4.EntityFramework.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Skoruba.AuditLogging.EntityFramework.DbContexts;
using Skoruba.AuditLogging.EntityFramework.Entities;
using Skoruba.IdentityServer4.Admin.EntityFramework.Interfaces;

namespace Skoruba.IdentityServer4.Admin.EntityFramework.PostgreSQL.Extensions
{
    public static class DatabaseExtensions
    {
        /// <summary>
        /// Register DbContexts for IdentityServer ConfigurationStore and PersistedGrants, Identity and Logging
        /// Configure the connection strings in AppSettings.json
        /// </summary>
        /// <typeparam name="TConfigurationDbContext"></typeparam>
        /// <typeparam name="TPersistedGrantDbContext"></typeparam>
        /// <typeparam name="TLogDbContext"></typeparam>
        /// <typeparam name="TIdentityDbContext"></typeparam>
        /// <typeparam name="TAuditLoggingDbContext"></typeparam>
        /// <typeparam name="TTenantConfigDbContext"></typeparam>
        /// <param name="services"></param>
        /// <param name="identityConnectionString"></param>
        /// <param name="configurationConnectionString"></param>
        /// <param name="persistedGrantConnectionString"></param>
        /// <param name="errorLoggingConnectionString"></param>
        /// <param name="auditLoggingConnectionString"></param>
        /// <param name="tenantConfigConnectionString"></param>
        public static void RegisterNpgSqlDbContexts<TIdentityDbContext, TConfigurationDbContext,
            TPersistedGrantDbContext, TLogDbContext, TAuditLoggingDbContext, TTenantConfigDbContext>(
            this IServiceCollection services,
            string identityConnectionString,
            string configurationConnectionString,
            string persistedGrantConnectionString,
            string errorLoggingConnectionString,
            string auditLoggingConnectionString,
            string tenantConfigConnectionString)
            where TIdentityDbContext : DbContext
            where TPersistedGrantDbContext : DbContext, IAdminPersistedGrantDbContext
            where TConfigurationDbContext : DbContext, IAdminConfigurationDbContext
            where TLogDbContext : DbContext, IAdminLogDbContext
            where TAuditLoggingDbContext : DbContext, IAuditLoggingDbContext<AuditLog>
            where TTenantConfigDbContext : DbContext, IAdminTenantConfigDbContext
        {
            var migrationsAssembly = typeof(DatabaseExtensions).GetTypeInfo().Assembly.GetName().Name;

            // Config DB for identity
            services.AddDbContext<TIdentityDbContext>(options =>
                options.UseNpgsql(identityConnectionString, sql => sql.MigrationsAssembly(migrationsAssembly)));

            // Config DB from existing connection
            services.AddConfigurationDbContext<TConfigurationDbContext>(options =>
                options.ConfigureDbContext = b =>
                    b.UseNpgsql(configurationConnectionString, sql => sql.MigrationsAssembly(migrationsAssembly)));

            // Operational DB from existing connection
            services.AddOperationalDbContext<TPersistedGrantDbContext>(options => options.ConfigureDbContext = b =>
                b.UseNpgsql(persistedGrantConnectionString, sql => sql.MigrationsAssembly(migrationsAssembly)));

            // Log DB from existing connection
            services.AddDbContext<TLogDbContext>(options => options.UseNpgsql(errorLoggingConnectionString,
                optionsSql => optionsSql.MigrationsAssembly(migrationsAssembly)));

            // Audit logging connection
            services.AddDbContext<TAuditLoggingDbContext>(options => options.UseNpgsql(auditLoggingConnectionString,
                optionsSql => optionsSql.MigrationsAssembly(migrationsAssembly)));

            // Config DB for multi-tenant support
            services.AddDbContext<TTenantConfigDbContext>(options => options.UseNpgsql(tenantConfigConnectionString,
                optionsSql => optionsSql.MigrationsAssembly(migrationsAssembly)));
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
        /// <param name="identityConnectionString"></param>
        /// <param name="configurationConnectionString"></param>
        /// <param name="persistedGrantConnectionString"></param>
        /// <param name="tenantConfigConnectionString"></param>
        public static void RegisterNpgSqlDbContexts<TIdentityDbContext, TConfigurationDbContext,
            TPersistedGrantDbContext, TTenantConfigDbContext>(
            this IServiceCollection services,
            string identityConnectionString,
            string configurationConnectionString,
            string persistedGrantConnectionString,
            string tenantConfigConnectionString)
            where TIdentityDbContext : DbContext
            where TPersistedGrantDbContext : DbContext, IAdminPersistedGrantDbContext
            where TConfigurationDbContext : DbContext, IAdminConfigurationDbContext
            where TTenantConfigDbContext : DbContext, IAdminTenantConfigDbContext
        {
            var migrationsAssembly = typeof(DatabaseExtensions).GetTypeInfo().Assembly.GetName().Name;

            // Config DB for identity
            services.AddDbContext<TIdentityDbContext>(options => options.UseNpgsql(identityConnectionString, sql => sql.MigrationsAssembly(migrationsAssembly)));

            // Config DB from existing connection
            services.AddConfigurationDbContext<TConfigurationDbContext>(options => options.ConfigureDbContext = b => b.UseNpgsql(configurationConnectionString, sql => sql.MigrationsAssembly(migrationsAssembly)));

            // Operational DB from existing connection
            services.AddOperationalDbContext<TPersistedGrantDbContext>(options => options.ConfigureDbContext = b => b.UseNpgsql(persistedGrantConnectionString, sql => sql.MigrationsAssembly(migrationsAssembly)));

            // Config DB for multi-tenant support
            services.AddDbContext<TTenantConfigDbContext>(options => options.UseNpgsql(tenantConfigConnectionString,
                optionsSql => optionsSql.MigrationsAssembly(migrationsAssembly)));
        }
    }
}