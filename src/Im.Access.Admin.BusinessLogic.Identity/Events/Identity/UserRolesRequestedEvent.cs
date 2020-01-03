using Skoruba.AuditLogging.Events;

namespace Im.Access.Admin.BusinessLogic.Identity.Events.Identity
{
    public class UserRolesRequestedEvent<TUserRolesDto> : AuditEvent
    {
        public TUserRolesDto Roles { get; set; }

        public UserRolesRequestedEvent(TUserRolesDto roles)
        {
            Roles = roles;
        }
    }
}