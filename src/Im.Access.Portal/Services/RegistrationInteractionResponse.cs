using IdentityServer4.ResponseHandling;

namespace Im.Access.Portal.Services
{
    public class RegistrationInteractionResponse : InteractionResponse
    {
        public bool IsCreate { get; set; }
    }
}