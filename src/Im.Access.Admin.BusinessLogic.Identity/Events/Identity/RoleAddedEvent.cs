using Skoruba.AuditLogging.Events;

namespace Im.Access.Admin.BusinessLogic.Identity.Events.Identity
{
    public class RoleAddedEvent<TRoleDto> : AuditEvent
    {
        public TRoleDto Role { get; set; }

        public RoleAddedEvent(TRoleDto role)
        {
            Role = role;
        }
    }
}