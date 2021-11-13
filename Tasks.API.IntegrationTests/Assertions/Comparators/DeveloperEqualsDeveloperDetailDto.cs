using Tasks.API.IntegrationTests.Common.Utils;
using Tasks.Domain.Developers.Dtos;
using Tasks.Domain.Developers.Entities;

namespace Tasks.API.IntegrationTests.Assertions.Comparators
{
    public class DeveloperEqualsDeveloperDetailDto
    {
        public void Equals(DeveloperDetailDto source, Developer target) => Equals(target, source);
        public void Equals(Developer source, DeveloperDetailDto target)
        {
            AssertExtensions.AreEqualObjects(source, target);
        }
    }
}