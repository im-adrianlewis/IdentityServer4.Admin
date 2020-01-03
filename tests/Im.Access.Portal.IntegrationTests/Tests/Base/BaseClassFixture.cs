using System.Net.Http;
using Im.Access.Portal;
using Microsoft.AspNetCore.Mvc.Testing;
using Im.Access.Portal.IntegrationTests.Common;
using Xunit;

namespace Im.Access.Portal.IntegrationTests.Tests.Base
{
    public class BaseClassFixture : IClassFixture<WebApplicationFactory<Startup>>
    {
        protected readonly HttpClient _client;

        public BaseClassFixture(WebApplicationFactory<Startup> factory)
        {
            _client = factory.SetupClient();
        }
    }
}