using Tasks.API.IntegrationTests.Common.Utils;
using Tasks.Domain.Developers.Dtos;
using Tasks.Domain.Developers.Entities;

namespace Tasks.API.IntegrationTests.Assertions.Comparators
{
    public class DeveloperEqualsDeveloperListDto
    {
        public void Equals(DeveloperListDto source, Developer target) => Equals(target, source);
        public void Equals(Developer source, DeveloperListDto target)
        {
            AssertExtensions.AreEqualObjects(source, target);
        }
    }
}