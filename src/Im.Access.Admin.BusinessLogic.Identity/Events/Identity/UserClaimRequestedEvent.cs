﻿using Skoruba.AuditLogging.Events;

namespace Im.Access.Admin.BusinessLogic.Identity.Events.Identity
{
    public class UserClaimRequestedEvent<TUserClaimsDto> : AuditEvent
    {
        public TUserClaimsDto UserClaims { get; set; }

        public UserClaimRequestedEvent(TUserClaimsDto userClaims)
        {
            UserClaims = userClaims;
        }
    }
}