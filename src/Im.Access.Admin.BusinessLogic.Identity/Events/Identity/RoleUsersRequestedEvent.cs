using Skoruba.AuditLogging.Events;
using Im.Access.Admin.BusinessLogic.Identity.Dtos.Identity;

namespace Im.Access.Admin.BusinessLogic.Identity.Events.Identity
{
    public class RoleUsersRequestedEvent<TUsersDto> : AuditEvent
    {
        public TUsersDto Users { get; set; }

        public RoleUsersRequestedEvent(TUsersDto users)
        {
            Users = users;
        }
    }
}