using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Im.Access.Portal;
using Microsoft.AspNetCore.Mvc.Testing;
using Im.Access.Portal.IntegrationTests.Tests.Base;
using Xunit;

namespace Im.Access.Portal.IntegrationTests.Tests
{
    public class HomeControllerTests : BaseClassFixture
    {
        public HomeControllerTests(WebApplicationFactory<Startup> factory) : base(factory)
        {
        }

        [Fact]
        public async Task EveryoneHasAccessToHomepage()
        {
            _client.DefaultRequestHeaders.Clear();

            // Act
            var response = await _client.GetAsync("/home/index");

            // Assert
            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}