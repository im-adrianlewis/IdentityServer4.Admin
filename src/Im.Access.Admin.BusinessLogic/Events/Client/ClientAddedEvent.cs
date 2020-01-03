using Skoruba.AuditLogging.Events;
using Im.Access.Admin.BusinessLogic.Dtos.Configuration;

namespace Im.Access.Admin.BusinessLogic.Events.Client
{
    public class ClientAddedEvent : AuditEvent
    {
        public ClientDto Client { get; set; }

        public ClientAddedEvent(ClientDto client)
        {
            Client = client;
        }
    }
}