using IdentityServer4.Configuration;

namespace Skoruba.IdentityServer4.STS.Identity.Services
{
    public class RegistrationCapableUserInteractionOptions : UserInteractionOptions
    {
        public string RegisterUrl { get; set; }

        public string RegisterReturnUrlParameter { get; set; }
    }
}