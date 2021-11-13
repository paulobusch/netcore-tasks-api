using Tasks.API.IntegrationTests.Common.Utils;
using Tasks.Domain.Projects.Dtos.Works;
using Tasks.Domain.Projects.Entities;

namespace Tasks.API.IntegrationTests.Assertions.Comparators
{
    public class ProjectEqualsProjectWorkListDto
    {
        public void Equals(ProjectWorkListDto source, Project target) => Equals(target, source);
        public void Equals(Project source, ProjectWorkListDto target)
        {
            AssertExtensions.AreEqualObjects(source, target);
        }
    }
}