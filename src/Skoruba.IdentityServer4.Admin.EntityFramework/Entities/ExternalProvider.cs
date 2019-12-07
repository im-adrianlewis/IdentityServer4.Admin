using System;

namespace Skoruba.IdentityServer4.Admin.EntityFramework.Entities
{
    public class ExternalProvider
    {
        public Guid Id { get; set; }

        public Guid TenantConfigurationId { get; set; }

        public bool Enabled { get; set; }

        public string ConsumerKey { get; set; }

        public string Secret { get; set; }

        public string AuthenticationType { get; set; }

        public string Caption { get; set; }

        public string Callback { get; set; }

        public virtual TenantConfiguration TenantConfiguration { get; set; }
    }
}