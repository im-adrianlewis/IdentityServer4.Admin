using System.Net.Http;
using Im.Access.Admin.Configuration.Interfaces;
using Im.Access.Admin.IntegrationTests.Common;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Im.Access.Admin.IntegrationTests.Tests.Base
{
    public class BaseClassFixture : IClassFixture<WebApplicationFactory<Startup>>
    {
        protected readonly WebApplicationFactory<Startup> Factory;
        protected readonly HttpClient Client;

        public BaseClassFixture(WebApplicationFactory<Startup> factory)
        {
            Factory = factory;
            Client = factory.SetupClient();
            Factory.CreateClient();
        }

        protected virtual void SetupAdminClaimsViaHeaders()
        {
            using (var scope = Factory.Services.CreateScope())
            {
                var configuration = scope.ServiceProvider.GetRequiredService<IRootConfiguration>();
                Client.SetAdminClaimsViaHeaders(configuration.AdminConfiguration);
            }
        }
    }
}