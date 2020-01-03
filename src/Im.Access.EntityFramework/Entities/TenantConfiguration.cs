using System;
using System.Collections.Generic;

namespace Im.Access.EntityFramework.Entities
{
    public class TenantConfiguration
    {
        public Guid Id { get; set; }

        public string Tenant { get; set; }

        public bool EmailVerification { get; set; }

        public bool FirstPartyRequired { get; set; }

        public bool IsSecondPageRegistration { get; set; }

        public string SecondPageRegistrationUrl { get; set; }

        public string ContactUsUrl { get; set; }

        public string SiteUrl { get; set; }

        public string LoginUrl { get; set; }

        public string GoogleTagManagerId { get; set; }

        public string OptimizelyId { get; set; }

        public string PermutiveProjectId { get; set; }

        public string PermutiveApiKey { get; set; }

        public string RegisterUrl { get; set; }

        public bool IsVanityUrl { get; set; }

        public string ExternalRegistrationUrl { get; set; }

        public string StyleManifestUrl { get; set; }

        public string GoogleAnalyticsId { get; set; }

        public string ReCaptchaSiteKey { get; set; }

        public string ReCaptchaSecretKey { get; set; }

        public bool IsReCaptchaEnabled { get; set; }

        public virtual ICollection<ClaimType> ClaimTypes { get; set; }

        public virtual ICollection<ExternalProvider> ExternalProviders { get; set; }

        public virtual PasswordPolicy PasswordPolicy { get; set; }
    }
}
