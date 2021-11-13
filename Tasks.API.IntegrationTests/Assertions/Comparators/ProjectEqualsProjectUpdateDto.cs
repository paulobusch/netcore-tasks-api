using Tasks.API.IntegrationTests.Common.Utils;
using Tasks.Domain.Projects.Dtos;
using Tasks.Domain.Projects.Entities;

namespace Tasks.API.IntegrationTests.Assertions.Comparators
{
    public class ProjectEqualsProjectUpdateDto
    {
        public void Equals(ProjectUpdateDto source, Project target) => Equals(target, source);
        public void Equals(Project source, ProjectUpdateDto target)
        {
            AssertExtensions.AreEqualObjects(source, target);
        }
    }
}