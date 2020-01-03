using Skoruba.AuditLogging.Events;
using Im.Access.Admin.BusinessLogic.Identity.Dtos.Identity;

namespace Im.Access.Admin.BusinessLogic.Identity.Events.Identity
{
    public class UserRequestedEvent<TUserDto> : AuditEvent
    {
        public TUserDto UserDto { get; set; }

        public UserRequestedEvent(TUserDto userDto)
        {
            UserDto = userDto;
        }
    }
}