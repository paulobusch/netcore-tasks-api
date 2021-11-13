using Tasks.API.IntegrationTests.Common.Utils;
using Tasks.Domain.Developers.Dtos;
using Tasks.Domain.Developers.Entities;

namespace Tasks.API.IntegrationTests.Assertions.Comparators
{
    public class DeveloperEqualsDeveloperUpdateDto
    {
        public void Equals(DeveloperUpdateDto source, Developer target) => Equals(target, source);
        public void Equals(Developer source, DeveloperUpdateDto target)
        {
            AssertExtensions.AreEqualObjects(source, target);
        }
    }
}