using System;

namespace Skoruba.IdentityServer4.Admin.EntityFramework.Entities
{
    public class PasswordPolicy
    {
        public Guid Id { get; set; }

        public Guid TenantConfigurationId { get; set; }

        public bool RequireDigit { get; set; }

        public bool RequireUpperCase { get; set; }

        public bool RequireLowerCase { get; set; }

        public bool RequireNonAlphaNumeric { get; set; }

        public int MinimumLength { get; set; }

        public virtual TenantConfiguration TenantConfiguration { get; set; }
    }
}