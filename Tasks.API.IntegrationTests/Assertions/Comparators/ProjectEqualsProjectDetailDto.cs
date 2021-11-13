using Tasks.API.IntegrationTests.Common.Utils;
using Tasks.Domain.Projects.Dtos;
using Tasks.Domain.Projects.Entities;

namespace Tasks.API.IntegrationTests.Assertions.Comparators
{
    public class ProjectEqualsProjectDetailDto
    {
        public void Equals(ProjectDetailDto source, Project target) => Equals(target, source);
        public void Equals(Project source, ProjectDetailDto target)
        {
            AssertExtensions.AreEqualObjects(source, target);
        }
    }
}