using System.Threading.Tasks;
using IdentityServer4.ResponseHandling;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;

namespace Im.Access.Portal.Services
{
    public class RegistrationCapableAuthorizeResponseGenerator : AuthorizeResponseGenerator
    {
        public RegistrationCapableAuthorizeResponseGenerator(
            ISystemClock clock,
            ITokenService tokenService,
            IKeyMaterialService keyMaterialService,
            IAuthorizationCodeStore authorizationCodeStore,
            ILogger<AuthorizeResponseGenerator> logger,
            IEventService events) : base(clock, tokenService, keyMaterialService, authorizationCodeStore, logger, events)
        {
        }

        public override Task<AuthorizeResponse> CreateResponseAsync(ValidatedAuthorizeRequest request)
        {
            if (request.PromptMode == CreateOidcConstants.PromptModes.Create)
            {
                return Task.FromResult<AuthorizeResponse>(new RegisterPageResponse(request));
            }

            return base.CreateResponseAsync(request);
        }
    }
}