using System;

namespace Skoruba.IdentityServer4.Admin.BusinessLogic.Identity.Dtos.Identity.Interfaces
{
    public interface IUserDto : IBaseUserDto
    {
        string UserName { get; set; }
        string Email { get; set; }
        bool EmailConfirmed { get; set; }
        string PhoneNumber { get; set; }
        bool PhoneNumberConfirmed { get; set; }
        bool LockoutEnabled { get; set; }
        bool TwoFactorEnabled { get; set; }
        int AccessFailedCount { get; set; }
        DateTimeOffset? LockoutEnd { get; set; }
        string TenantId { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        DateTimeOffset? RegistrationDate { get; set; }
        DateTimeOffset CreateDate { get; set; }
        string UserType { get; set; }
        string AuthenticationType { get; set; }
        DateTimeOffset? LastLoggedInDate { get; set; }
        DateTimeOffset LastUpdatedDate { get; set; }
        string Address1 { get; set; }
        string Address2 { get; set; }
        string City { get; set; }
        string County { get; set; }
        string Country { get; set; }
        string Postcode { get; set; }
        string UserBiography { get; set; }
        bool FirstPartyIm { get; set; }
        DateTimeOffset FirstPartyImUpdatedDate { get; set; }
        string RegistrationIpAddress { get; set; }
        string LastLoggedInIpAddress { get; set; }
        string ScreenName { get; set; }
    }
}
