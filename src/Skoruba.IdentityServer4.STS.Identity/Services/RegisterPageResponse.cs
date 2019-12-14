using System;
using System.Threading.Tasks;
using IdentityServer4.Configuration;
using IdentityServer4.Extensions;
using IdentityServer4.ResponseHandling;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Skoruba.IdentityServer4.STS.Identity.Services
{
    public class RegisterPageResponse : AuthorizeResponse
    {
        private readonly ValidatedAuthorizeRequest _request;
        private IdentityServerOptions _options;

        public RegisterPageResponse(ValidatedAuthorizeRequest request)
        {
            _request = request ?? throw new ArgumentNullException(nameof(request));
        }

        public RegisterPageResponse(ValidatedAuthorizeRequest request, IdentityServerOptions options) : this(request)
        {
            _options = options;
        }

        public Task ExecuteAsync(HttpContext context)
        {
            Init(context);
            var returnUrl = (context
                                 .GetIdentityServerBasePath()
                                 .EnsureTrailingSlash() + "connect/authorize/callback")
                .AddQueryString(_request.Raw.ToQueryString());

            var registerUrl = (_options.UserInteraction as RegistrationCapableUserInteractionOptions)?.RegisterUrl;
            if (!registerUrl.IsLocalUrl())
            {
                returnUrl = context.GetIdentityServerHost()
                                .EnsureTrailingSlash() + returnUrl.RemoveLeadingSlash();
            }

            var redirectUrl = registerUrl
                .AddQueryString((_options.UserInteraction as RegistrationCapableUserInteractionOptions)?.RegisterReturnUrlParameter, returnUrl);
            context.Response.RedirectToAbsoluteUrl(redirectUrl);
            return Task.CompletedTask;
        }

        private void Init(HttpContext context)
        {
            _options ??= context.RequestServices.GetRequiredService<IdentityServerOptions>();
        }
    }
}