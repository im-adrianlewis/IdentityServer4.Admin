using Skoruba.AuditLogging.Events;

namespace Im.Access.Admin.BusinessLogic.Identity.Events.Identity
{
    public class RoleRequestedEvent<TRoleDto> : AuditEvent
    {
        public TRoleDto Role { get; set; }

        public RoleRequestedEvent(TRoleDto role)
        {
            Role = role;
        }
    }
}