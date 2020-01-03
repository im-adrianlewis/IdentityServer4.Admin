using Skoruba.AuditLogging.Events;
using Im.Access.Admin.BusinessLogic.Dtos.Configuration;

namespace Im.Access.Admin.BusinessLogic.Events.ApiResource
{
    public class ApiResourceDeletedEvent : AuditEvent
    {
        public ApiResourceDto ApiResource { get; set; }

        public ApiResourceDeletedEvent(ApiResourceDto apiResource)
        {
            ApiResource = apiResource;
        }
    }
}