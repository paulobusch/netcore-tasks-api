using MockQueryable.Moq;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Tasks.Domain._Common.Enums;
using Tasks.Domain._Common.Results;
using Tasks.Domain.Developers.Repositories;
using Tasks.Domain.External.Services;
using Tasks.Domain.Projects.Repositories;
using Tasks.Domain.Works.Dtos;
using Tasks.Domain.Works.Entities;
using Tasks.Domain.Works.Repositories;
using Tasks.Services.Works;
using Tasks.UnitTests._Common;
using Tasks.UnitTests._Common.Random;
using Xunit;

namespace Tasks.UnitTests.Works.Services
{
    public class WorkServiceTests : BaseTest
    {
        private readonly Mock<IWorkRepository> _workRepository;
        private readonly Mock<IDeveloperRepository> _developerRepository;
        private readonly Mock<IProjectRepository> _projectRepository;
        private readonly Mock<IMockyService> _mockyRepository;

        public WorkServiceTests(TasksFixture fixture) : base(fixture)
        {
            _workRepository = new Mock<IWorkRepository>();
            _developerRepository = new Mock<IDeveloperRepository>();
            _projectRepository = new Mock<IProjectRepository>();
            _mockyRepository = new Mock<IMockyService>();
        }

        public static IEnumerable<object[]> WorkCreateData()
        {
            var validStart = DateTime.Now.AddMinutes(-5);
            var validEnd = DateTime.Now;
            yield return new object[] { Status.Invalid, DateTime.Now, validEnd, true, true };
            yield return new object[] { Status.Invalid, DateTime.Now.AddMinutes(5), validEnd, true, true };
            yield return new object[] { Status.Invalid, validStart, DateTime.Now.AddMinutes(5), true, true };
            yield return new object[] { Status.NotFund, validStart, validEnd, false, true };
            yield return new object[] { Status.NotFund, validStart, validEnd, true, false };
            yield return new object[] { Status.NotAllowed, validStart, validEnd, true, true };
            yield return new object[] { Status.Error, validStart, validEnd, true, true };
            yield return new object[] { Status.Success, validStart, validEnd, true, true };
        }

        [Theory]
        [MemberData(nameof(WorkCreateData))]
        public async void CreateWorkTest(
            Status expectedStatus,
            DateTime startTime,
            DateTime endTime,
            bool withProject = false,
            bool withDeveloper = false
        )
        {
            var developer = EntitiesFactory.NewDeveloper().Get();
            var project = EntitiesFactory.NewProject(developerIds: new[] { developer.Id }).Save();

            var workDto = new WorkCreateDto
            {
                DeveloperId = developer.Id,
                ProjectId = project.Id,
                StartTime = startTime,
                EndTime = endTime,
                Comment = RandomHelper.RandomString(180),
                Hours = 10
            };
            var worksPersisted = new List<Work>();
            _projectRepository.Setup(p => p.ExistAsync(project.Id)).ReturnsAsync(withProject);
            _projectRepository.Setup(p => p.GetDeveloperProjectIdAsync(project.Id, developer.Id))
                .ReturnsAsync(project.DeveloperProjects.Single().Id);
            _projectRepository.Setup(p => p.ExistDeveloperVinculatedAsync(project.Id, developer.Id))
                .ReturnsAsync(expectedStatus != Status.NotAllowed);
            _developerRepository.Setup(p => p.ExistAsync(developer.Id)).ReturnsAsync(withDeveloper);
            _workRepository.Setup(d => d.CreateAsync(Capture.In(worksPersisted)));
            _mockyRepository.Setup(m => m.SendNotificationAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new Result<bool>(expectedStatus != Status.Error));

            var service = new WorkService(
                _workRepository.Object,
                _developerRepository.Object,
                _projectRepository.Object,
                _mockyRepository.Object
            );
            var result = await service.CreateWorkAsync(workDto);

            Assert.Equal(expectedStatus, result.Status);
            if (expectedStatus == Status.Success)
            {
                _workRepository.Verify(d => d.CreateAsync(It.IsAny<Work>()), Times.Once);
                _mockyRepository.Verify(d => d.SendNotificationAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
                var work = worksPersisted.Single();
                Assert.Equal(workDto.Comment, work.Comment);
                Assert.Equal(workDto.StartTime, work.StartTime);
                Assert.Equal(workDto.EndTime, work.EndTime);
                Assert.Equal(workDto.Hours, work.Hours);
            }
        }

        public static IEnumerable<object[]> WorkUpdateData()
        {
            var validStart = DateTime.Now.AddMinutes(-5);
            var validEnd = DateTime.Now;
            yield return new object[] { Status.Invalid, DateTime.Now, validEnd };
            yield return new object[] { Status.Invalid, DateTime.Now.AddMinutes(5), validEnd };
            yield return new object[] { Status.Invalid, validStart, DateTime.Now.AddMinutes(5) };
            yield return new object[] { Status.NotFund, validStart, validEnd };
            yield return new object[] { Status.Success, validStart, validEnd };
        }

        [Theory]
        [MemberData(nameof(WorkUpdateData))]
        public async void UpdateWorkTest(
            Status expectedStatus,
            DateTime startTime,
            DateTime endTime
        )
        {
            var developer = EntitiesFactory.NewDeveloper().Save();
            var project = EntitiesFactory.NewProject(developerIds: new[] { developer.Id }).Save();
            var work = EntitiesFactory.NewWork(
                id: Guid.NewGuid(),
                developerProjectId: project.DeveloperProjects.Single().Id
            ).Get();

            var workDto = new WorkUpdateDto
            {
                Id = work.Id,
                StartTime = startTime,
                EndTime = endTime,
                Comment = RandomHelper.RandomString(180),
                Hours = 250
            };
            _workRepository.Setup(d => d.ExistAsync(workDto.Id))
                .ReturnsAsync(expectedStatus != Status.NotFund);
            _workRepository.Setup(d => d.GetByIdAsync(workDto.Id))
                .ReturnsAsync(work);
            _mockyRepository.Setup(m => m.SendNotificationAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new Result<bool>(expectedStatus != Status.Error));

            var service = new WorkService(
                _workRepository.Object,
                _developerRepository.Object,
                _projectRepository.Object,
                _mockyRepository.Object
            );
            var result = await service.UpdateWorkAsync(workDto);

            Assert.Equal(expectedStatus, result.Status);
            if (expectedStatus == Status.Success)
            {
                _workRepository.Verify(d => d.UpdateAsync(work), Times.Once);
                _mockyRepository.Verify(d => d.SendNotificationAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
                Assert.Equal(workDto.Comment, work.Comment);
                Assert.Equal(workDto.StartTime, work.StartTime);
                Assert.Equal(workDto.EndTime, work.EndTime);
                Assert.Equal(workDto.Hours, work.Hours);
            }
        }

        public static IEnumerable<object[]> WorkDeleteData()
        {
            yield return new object[] { Status.Error };
            yield return new object[] { Status.NotFund };
            yield return new object[] { Status.Success };
        }

        [Theory]
        [MemberData(nameof(WorkDeleteData))]
        public async void DeleteWorkTest(
            Status expectedStatus
        )
        {
            var developer = EntitiesFactory.NewDeveloper().Save();
            var project = EntitiesFactory.NewProject(developerIds: new[] { developer.Id }).Save();

            var work = EntitiesFactory.NewWork(Guid.NewGuid(), project.DeveloperProjects.Single().Id).Get();
            _workRepository.Setup(d => d.ExistAsync(work.Id))
                .ReturnsAsync(expectedStatus != Status.NotFund);
            _workRepository.Setup(d => d.GetByIdAsync(work.Id))
                .ReturnsAsync(work);
            _mockyRepository.Setup(m => m.SendNotificationAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new Result<bool>(expectedStatus != Status.Error));

            var service = new WorkService(
                _workRepository.Object,
                _developerRepository.Object,
                _projectRepository.Object,
                _mockyRepository.Object
            );
            var result = await service.DeleteWorkAsync(work.Id);

            Assert.Equal(expectedStatus, result.Status);
            if (expectedStatus == Status.Success)
            {
                _workRepository.Verify(d => d.DeleteAsync(work), Times.Once);
                _mockyRepository.Verify(d => d.SendNotificationAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            }
        }
    }
}
