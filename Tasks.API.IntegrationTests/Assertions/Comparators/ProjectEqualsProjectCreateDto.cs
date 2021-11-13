using Tasks.API.IntegrationTests.Common.Utils;
using Tasks.Domain.Projects.Dtos;
using Tasks.Domain.Projects.Entities;

namespace Tasks.API.IntegrationTests.Assertions.Comparators
{
    public class ProjectEqualsProjectCreateDto
    {
        public void Equals(ProjectCreateDto source, Project target) => Equals(target, source);
        public void Equals(Project source, ProjectCreateDto target)
        {
            AssertExtensions.AreEqualObjects(source, target);
        }
    }
}