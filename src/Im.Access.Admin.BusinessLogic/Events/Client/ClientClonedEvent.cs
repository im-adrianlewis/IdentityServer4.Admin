using Skoruba.AuditLogging.Events;
using Im.Access.Admin.BusinessLogic.Dtos.Configuration;

namespace Im.Access.Admin.BusinessLogic.Events.Client
{
    public class ClientClonedEvent : AuditEvent
    {
        public ClientCloneDto Client { get; set; }

        public ClientClonedEvent(ClientCloneDto client)
        {
            Client = client;
        }
    }
}