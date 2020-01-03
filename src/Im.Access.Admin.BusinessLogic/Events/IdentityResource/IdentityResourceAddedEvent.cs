using Skoruba.AuditLogging.Events;
using Im.Access.Admin.BusinessLogic.Dtos.Configuration;

namespace Im.Access.Admin.BusinessLogic.Events.IdentityResource
{
    public class IdentityResourceAddedEvent : AuditEvent
    {
        public IdentityResourceDto IdentityResource { get; set; }

        public IdentityResourceAddedEvent(IdentityResourceDto identityResource)
        {
            IdentityResource = identityResource;
        }
    }
}