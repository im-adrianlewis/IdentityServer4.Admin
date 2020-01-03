using Skoruba.AuditLogging.Events;
using Im.Access.Admin.BusinessLogic.Dtos.Configuration;

namespace Im.Access.Admin.BusinessLogic.Events.Client
{
    public class ClientRequestedEvent : AuditEvent
    {
        public ClientDto ClientDto { get; set; }

        public ClientRequestedEvent(ClientDto clientDto)
        {
            ClientDto = clientDto;
        }
    }
}