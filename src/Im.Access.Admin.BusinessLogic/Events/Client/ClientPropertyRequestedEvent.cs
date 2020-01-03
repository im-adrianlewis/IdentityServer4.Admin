using Skoruba.AuditLogging.Events;
using Im.Access.Admin.BusinessLogic.Dtos.Configuration;

namespace Im.Access.Admin.BusinessLogic.Events.Client
{
    public class ClientPropertyRequestedEvent : AuditEvent
    {
        public ClientPropertiesDto ClientProperties { get; set; }

        public ClientPropertyRequestedEvent(ClientPropertiesDto clientProperties)
        {
            ClientProperties = clientProperties;
        }
    }
}