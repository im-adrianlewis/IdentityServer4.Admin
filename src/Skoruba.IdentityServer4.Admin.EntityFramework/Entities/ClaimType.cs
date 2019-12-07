using System;

namespace Skoruba.IdentityServer4.Admin.EntityFramework.Entities
{
    public class ClaimType
    {
        public Guid Id { get; set; }

        public Guid TenantConfigurationId { get; set; }

        public string TypeName { get; set; }

        public string Text { get; set; }

        public string Lead { get; set; }
        
        public bool IsVisibleOnRegistration { get; set; }
        
        public int SortOrder { get; set; }

        public virtual TenantConfiguration TenantConfiguration { get; set; }
    }
}