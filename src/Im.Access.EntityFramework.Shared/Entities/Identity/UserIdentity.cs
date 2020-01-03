using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Im.Access.EntityFramework.Shared.Entities.Identity
{
	public class UserIdentity : IdentityUser
	{
        public virtual string TenantId { get; set; }

        public virtual string FirstName { get; set; }

        public virtual string LastName { get; set; }

        public virtual DateTimeOffset? RegistrationDate { get; set; }

        public virtual DateTimeOffset CreateDate { get; set; }

        public virtual string UserType { get; set; }

        public virtual string AuthenticationType { get; set; }

        public virtual DateTimeOffset? LastLoggedInDate { get; set; }

        public virtual DateTimeOffset LastUpdatedDate { get; set; }

        public virtual string Address1 { get; set; }

        public virtual string Address2 { get; set; }

        public virtual string City { get; set; }

        public virtual string County { get; set; }

        public virtual string Country { get; set; }

        public virtual string Postcode { get; set; }

        public virtual string UserBiography { get; set; }

        public virtual bool FirstPartyIm { get; set; }

        public virtual DateTimeOffset FirstPartyImUpdatedDate { get; set; }

        public virtual string RegistrationIpAddress { get; set; }

        public virtual string LastLoggedInIpAddress { get; set; }

        public virtual string ScreenName { get; set; }
	}
}