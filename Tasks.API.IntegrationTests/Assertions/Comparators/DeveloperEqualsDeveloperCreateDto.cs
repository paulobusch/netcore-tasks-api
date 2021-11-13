using Tasks.API.IntegrationTests.Common.Utils;
using Tasks.Domain.Developers.Dtos;
using Tasks.Domain.Developers.Entities;

namespace Tasks.API.IntegrationTests.Assertions.Comparators
{
    public class DeveloperEqualsDeveloperCreateDto
    {
        public void Equals(DeveloperCreateDto source, Developer target) => Equals(target, source);
        public void Equals(Developer source, DeveloperCreateDto target)
        {
            AssertExtensions.AreEqualObjects(source, target);
        }
    }
}