using IdentityServer4.ResponseHandling;

namespace Skoruba.IdentityServer4.STS.Identity.Services
{
    public class RegistrationInteractionResponse : InteractionResponse
    {
        public bool IsCreate { get; set; }
    }
}