using IdentityServer4.Configuration;

namespace Im.Access.Portal.Services
{
    public class RegistrationCapableUserInteractionOptions : UserInteractionOptions
    {
        public string RegisterUrl { get; set; }

        public string RegisterReturnUrlParameter { get; set; }
    }
}