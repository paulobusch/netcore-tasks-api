using Tasks.API.IntegrationTests.Common.Utils;
using Tasks.Domain.Projects.Dtos;
using Tasks.Domain.Projects.Entities;

namespace Tasks.API.IntegrationTests.Assertions.Comparators
{
    public class ProjectEqualsProjectListDto
    {
        public void Equals(ProjectListDto source, Project target) => Equals(target, source);
        public void Equals(Project source, ProjectListDto target)
        {
            AssertExtensions.AreEqualObjects(source, target);
        }
    }
}