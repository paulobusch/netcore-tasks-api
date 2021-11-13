using Tasks.API.IntegrationTests.Common.Utils;
using Tasks.Domain.Projects.Entities;
using Tasks.Domain.Works.Dtos;

namespace Tasks.API.IntegrationTests.Assertions.Comparators
{
    public class ProjectEqualsWorkClientDto
    {
        public void Equals(WorkClientDto source, Project target) => Equals(target, source);
        public void Equals(Project source, WorkClientDto target)
        {
            AssertExtensions.AreEqualObjects(source, target);
        }
    }
}