using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Im.Access.Portal;
using Microsoft.AspNetCore.Mvc.Testing;
using Im.Access.Portal.IntegrationTests.Common;
using Im.Access.Portal.IntegrationTests.Mocks;
using Im.Access.Portal.IntegrationTests.Tests.Base;
using Xunit;

namespace Im.Access.Portal.IntegrationTests.Tests
{
    public class GrantsControllerTests : BaseClassFixture
    {
        public GrantsControllerTests(WebApplicationFactory<Startup> factory) : base(factory)
        {
        }

        [Fact]
        public async Task AuthorizeUserCanAccessGrantsView()
        {
            // Clear headers
            _client.DefaultRequestHeaders.Clear();

            // Register new user
            var registerFormData = UserMocks.GenerateRegisterData();
            var registerResponse = await UserMocks.RegisterNewUserAsync(_client, registerFormData);

            // Get cookie with user identity for next request
            _client.PutCookiesOnRequest(registerResponse);

            // Act
            var response = await _client.GetAsync("/Grants/Index");

            // Assert
            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task UnAuthorizeUserCannotAccessGrantsView()
        {
            // Clear headers
            _client.DefaultRequestHeaders.Clear();

            // Act
            var response = await _client.GetAsync("/Grants/Index");

            // Assert      
            response.StatusCode.Should().Be(HttpStatusCode.Redirect);

            //The redirect to login
            response.Headers.Location.ToString().Should().Contain("Account/Login");
        }
    }
}
