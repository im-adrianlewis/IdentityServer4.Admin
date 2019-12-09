﻿using System;
using Microsoft.AspNetCore.Identity;

namespace Skoruba.IdentityServer4.Admin.EntityFramework.Shared.Entities.Identity
{
    public class UserIdentityUserClaim : IdentityUserClaim<string>
    {
        public virtual DateTimeOffset ClaimUpdatedDate { get; set; }
    }
}
