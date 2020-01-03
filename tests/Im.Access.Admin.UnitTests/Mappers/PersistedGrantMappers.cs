using System;
using FluentAssertions;
using Im.Access.Admin.BusinessLogic.Mappers;
using Im.Access.Admin.UnitTests.Mocks;
using Xunit;

namespace Im.Access.Admin.UnitTests.Mappers
{
    public class PersistedGrantMappers
    {
        [Fact]
        public void CanMapPersistedGrantToModel()
        {
            var persistedGrantKey = Guid.NewGuid().ToString();

            //Generate entity
            var persistedGrant = PersistedGrantMock.GenerateRandomPersistedGrant(persistedGrantKey);

            //Try map to DTO
            var persistedGrantDto = persistedGrant.ToModel();

            //Asert
            persistedGrantDto.Should().NotBeNull();

            persistedGrant.Should().BeEquivalentTo(persistedGrantDto, opt => opt
                .Excluding(o => o.SubjectName));
        }
    }
}