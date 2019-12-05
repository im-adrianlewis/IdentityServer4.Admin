using System;
using System.ComponentModel.DataAnnotations;
using Skoruba.IdentityServer4.Admin.BusinessLogic.Identity.Dtos.Identity.Base;
using Skoruba.IdentityServer4.Admin.BusinessLogic.Identity.Dtos.Identity.Interfaces;

namespace Skoruba.IdentityServer4.Admin.BusinessLogic.Identity.Dtos.Identity
{
    public class UserDto<TKey> : BaseUserDto<TKey>, IUserDto
    {        
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9_@\-\.\+]+$")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public bool EmailConfirmed { get; set; }

        public string PhoneNumber { get; set; }

        public bool PhoneNumberConfirmed { get; set; }

        public bool LockoutEnabled { get; set; }

        public bool TwoFactorEnabled { get; set; }

        public int AccessFailedCount { get; set; }

        public DateTimeOffset? LockoutEnd { get; set; }

        [Required]
        public string TenantId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTimeOffset? RegistrationDate { get; set; }

        public DateTimeOffset CreateDate { get; set; }

        public string UserType { get; set; }

        public string AuthenticationType { get; set; }

        public DateTimeOffset? LastLoggedInDate { get; set; }

        public DateTimeOffset LastUpdatedDate { get; set; }

        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string City { get; set; }

        public string County { get; set; }

        public string Country { get; set; }

        public string Postcode { get; set; }

        public string UserBiography { get; set; }

        public bool FirstPartyIm { get; set; }

        public DateTimeOffset FirstPartyImUpdatedDate { get; set; }

        public string RegistrationIpAddress { get; set; }

        public string LastLoggedInIpAddress { get; set; }

        [Required]
        public string ScreenName { get; set; }
    }
}
