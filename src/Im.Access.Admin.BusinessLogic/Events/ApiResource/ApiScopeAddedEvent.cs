using Skoruba.AuditLogging.Events;
using Im.Access.Admin.BusinessLogic.Dtos.Configuration;

namespace Im.Access.Admin.BusinessLogic.Events.ApiResource
{
    public class ApiScopeAddedEvent : AuditEvent
    {
        public ApiScopesDto ApiScope { get; set; }

        public ApiScopeAddedEvent(ApiScopesDto apiScope)
        {
            ApiScope = apiScope;
        }
    }
}