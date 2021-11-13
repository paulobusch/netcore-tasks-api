using Moq;
using System;
using MockQueryable.Moq;
using System.Collections.Generic;
using System.Linq;
using Tasks.Domain._Common.Enums;
using Tasks.Domain.Projects.Dtos;
using Tasks.Domain.Projects.Entities;
using Tasks.Domain.Projects.Repositories;
using Tasks.Service.Projects;
using Tasks.UnitTests._Common;
using Tasks.UnitTests._Common.Random;
using Xunit;
using Tasks.Domain.Works.Repositories;

namespace Tasks.UnitTests.Projects.Services
{
    public class ProjectServiceTests : BaseTest
    {
        private readonly Mock<IProjectRepository> _projectRepository;
        private readonly Mock<IWorkRepository> _workRepository;

        public ProjectServiceTests(TasksFixture fixture) : base(fixture)
        { 
            _projectRepository = new Mock<IProjectRepository>();
            _workRepository = new Mock<IWorkRepository>();
        }
        public static IEnumerable<object[]> ProjectCreateData()
        {
            yield return new object[] { Status.Conflict };
            yield return new object[] { Status.Success };
        }

        [Theory]
        [MemberData(nameof(ProjectCreateData))]
        public async void CreateTest(Status expectedStatus)
        {
            var projectDto = new ProjectCreateDto
            {
                Title = RandomHelper.RandomString(),
                Description = RandomHelper.RandomString(300)
            };
            var projectsPersisted = new List<Project>();
            _projectRepository.Setup(d => d.CreateAsync(Capture.In(projectsPersisted)));
            _projectRepository.Setup(d => d.ExistByTitleAsync(projectDto.Title, default))
                .ReturnsAsync(expectedStatus == Status.Conflict);

            var service = new ProjectService(_projectRepository.Object, _workRepository.Object);
            var result = await service.CreateProjectAsync(projectDto);

            Assert.Equal(expectedStatus, result.Status);
            if (expectedStatus == Status.Success)
            {
                _projectRepository.Verify(d => d.CreateAsync(It.IsAny<Project>()), Times.Once);
                var project = projectsPersisted.Single();
                Assert.Equal(projectDto.Title, project.Title);
                Assert.Equal(projectDto.Description, project.Description);
            }
            else
            {
                _projectRepository.Verify(d => d.CreateAsync(It.IsAny<Project>()), Times.Never);
            }
        }

        public static IEnumerable<object[]> ProjectUpdateData()
        {
            yield return new object[] { Status.NotFund };
            yield return new object[] { Status.Conflict };
            yield return new object[] { Status.Success };
        }

        [Theory]
        [MemberData(nameof(ProjectUpdateData))]
        public async void UpdateTest(Status expectedStatus)
        {
            var projectDto = new ProjectUpdateDto
            {
                Id = Guid.NewGuid(),
                Title = RandomHelper.RandomString(),
                Description = RandomHelper.RandomString(350)
            };
            var project = EntitiesFactory.NewProject(id: projectDto.Id).Get();
            var projects = new[] { project }.AsQueryable().BuildMock();
            _projectRepository.Setup(d => d.ExistAsync(projectDto.Id))
                .ReturnsAsync(expectedStatus != Status.NotFund);
            _projectRepository.Setup(d => d.Query())
                .Returns(projects.Object);
            _projectRepository.Setup(d => d.ExistByTitleAsync(projectDto.Title, projectDto.Id))
                .ReturnsAsync(expectedStatus == Status.Conflict);

            var service = new ProjectService(_projectRepository.Object, _workRepository.Object);
            var result = await service.UpdateProjectAsync(projectDto);

            Assert.Equal(expectedStatus, result.Status);
            if (expectedStatus == Status.Success)
            {
                _projectRepository.Verify(d => d.UpdateAsync(project), Times.Once);
                Assert.Equal(projectDto.Title, project.Title);
                Assert.Equal(projectDto.Description, project.Description);
            }
            else
            {
                _projectRepository.Verify(d => d.UpdateAsync(project), Times.Never);
            }
        }

        public static IEnumerable<object[]> ProjectDeleteData()
        {
            yield return new object[] { Status.NotFund };
            yield return new object[] { Status.Success };
        }

        [Theory]
        [MemberData(nameof(ProjectDeleteData))]
        public async void DeleteTest(Status expectedStatus)
        {
            var project = EntitiesFactory.NewProject().Get();
            _projectRepository.Setup(d => d.ExistAsync(project.Id))
                .ReturnsAsync(expectedStatus != Status.NotFund);
            _projectRepository.Setup(d => d.GetByIdAsync(project.Id))
                .ReturnsAsync(project);

            var service = new ProjectService(_projectRepository.Object, _workRepository.Object);
            var result = await service.DeleteProjectAsync(project.Id);

            Assert.Equal(expectedStatus, result.Status);
            if (expectedStatus == Status.Success)
            {
                _projectRepository.Verify(d => d.DeleteAsync(project), Times.Once);
            }
            else
            {
                _projectRepository.Verify(d => d.DeleteAsync(project), Times.Never);
            }
        }
    }
}
