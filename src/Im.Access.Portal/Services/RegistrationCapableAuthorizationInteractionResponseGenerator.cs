using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.ResponseHandling;
using IdentityServer4.Services;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;

namespace Im.Access.Portal.Services
{
    public class RegistrationCapableAuthorizationInteractionResponseGenerator : AuthorizeInteractionResponseGenerator
    {
        public RegistrationCapableAuthorizationInteractionResponseGenerator(
            ISystemClock clock,
            ILogger<AuthorizeInteractionResponseGenerator> logger,
            IConsentService consent,
            IProfileService profile) : base(clock, logger, consent, profile)
        {
        }

        protected override async Task<InteractionResponse> ProcessConsentAsync(ValidatedAuthorizeRequest request, ConsentResponse consent = null)
        {
            // Call our new registration hook point
            var result = await ProcessRegistrationAsync(request);
            if (result.IsCreate || result.IsError)
            {
                return result;
            }

            // Continue through to the consent handling logic in base class
            return await base.ProcessConsentAsync(request, consent);
        }

        protected virtual Task<RegistrationInteractionResponse> ProcessRegistrationAsync(ValidatedAuthorizeRequest request)
        {
            // Test for registration
            if (request.PromptMode == CreateOidcConstants.PromptModes.Create)
            {
                Logger.LogInformation("Showing registration: request contains prompt={0}", request.PromptMode);
                request.RemovePrompt();
                return Task.FromResult(new RegistrationInteractionResponse { IsCreate = true });
            }

            return Task.FromResult(new RegistrationInteractionResponse());
        }
    }
}
