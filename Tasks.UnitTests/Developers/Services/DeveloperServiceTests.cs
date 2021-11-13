using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Tasks.Domain._Common.Crypto;
using Tasks.Domain._Common.Enums;
using Tasks.Domain._Common.Results;
using Tasks.Domain.Developers.Dtos;
using Tasks.Domain.Developers.Entities;
using Tasks.Domain.Developers.Repositories;
using Tasks.Domain.External.Services;
using Tasks.Domain.Works.Repositories;
using Tasks.Service.Developers;
using Tasks.UnitTests._Common;
using Tasks.UnitTests._Common.Random;
using Xunit;

namespace Tasks.UnitTests.Developers.Services
{
    public class DeveloperServiceTests : BaseTest
    {
        private readonly Mock<IDeveloperRepository> _developerRepository;
        private readonly Mock<IWorkRepository> _workRepository;
        private readonly Mock<IMockyService> _mockyRepository;

        public DeveloperServiceTests(TasksFixture fixture) : base(fixture)
        {
            _developerRepository = new Mock<IDeveloperRepository>();
            _workRepository = new Mock<IWorkRepository>();
            _mockyRepository = new Mock<IMockyService>();
        }

        public static IEnumerable<object[]> DeveloperCreateData()
        {
            yield return new object[] { Status.Invalid, false  };
            yield return new object[] { Status.Conflict, true };
            yield return new object[] { Status.Success, true };
        }

        [Theory]
        [MemberData(nameof(DeveloperCreateData))]
        public async void CreateDeveloperTest(
            Status expectedStatus,
            bool validCPF = false
        ) {
            var developerDto = new DeveloperCreateDto
            {
                Name = RandomHelper.RandomString(),
                Login = RandomHelper.RandomString(),
                Password = RandomHelper.RandomNumbers(),
                CPF = RandomHelper.RandomNumbers(11)
            };
            var developersPersisted = new List<Developer>();
            _developerRepository.Setup(d => d.CreateAsync(Capture.In(developersPersisted)));
            _developerRepository.Setup(d => d.ExistByLoginAsync(developerDto.Login, default))
                .ReturnsAsync(expectedStatus == Status.Conflict);
            _mockyRepository.Setup(m => m.ValidateCPFAsync(developerDto.CPF))
                .ReturnsAsync(new Result<bool>(validCPF));

            var service = new DeveloperService(_developerRepository.Object, _workRepository.Object, _mockyRepository.Object);
            var result = await service.CreateDeveloperAsync(developerDto);

            Assert.Equal(expectedStatus, result.Status);
            if (expectedStatus == Status.Success)
            {
                _developerRepository.Verify(d => d.CreateAsync(It.IsAny<Developer>()), Times.Once);
                _mockyRepository.Verify(d => d.ValidateCPFAsync(developerDto.CPF), Times.Once);
                var developer = developersPersisted.Single();
                Assert.Equal(developerDto.Name, developer.Name);
                Assert.Equal(developerDto.Login, developer.Login);
                Assert.Equal(developerDto.CPF, developer.CPF);
                Assert.Equal(MD5Crypto.Encode(developerDto.Password), developer.PasswordHash);
            } else { 
                _developerRepository.Verify(d => d.CreateAsync(It.IsAny<Developer>()), Times.Never);
            }
        }

        public static IEnumerable<object[]> DeveloperUpdateData()
        {
            yield return new object[] { Status.NotFund };
            yield return new object[] { Status.Conflict };
            yield return new object[] { Status.Success };
        }

        [Theory]
        [MemberData(nameof(DeveloperUpdateData))]
        public async void UpdateDeveloperTest(Status expectedStatus)
        {
            var developerDto = new DeveloperUpdateDto
            {
                Id = Guid.NewGuid(),
                Name = RandomHelper.RandomString(),
                Login = RandomHelper.RandomString()
            };
            var developer = EntitiesFactory.NewDeveloper(id: developerDto.Id).Get();
            _developerRepository.Setup(d => d.ExistAsync(developerDto.Id))
                .ReturnsAsync(expectedStatus != Status.NotFund);
            _developerRepository.Setup(d => d.GetByIdAsync(developerDto.Id))
                .ReturnsAsync(developer);
            _developerRepository.Setup(d => d.ExistByLoginAsync(developerDto.Login, developerDto.Id))
                .ReturnsAsync(expectedStatus == Status.Conflict);

            var service = new DeveloperService(_developerRepository.Object, _workRepository.Object, _mockyRepository.Object);
            var result = await service.UpdateDeveloperAsync(developerDto);

            Assert.Equal(expectedStatus, result.Status);
            if (expectedStatus == Status.Success)
            {
                _developerRepository.Verify(d => d.UpdateAsync(developer), Times.Once);
                Assert.Equal(developerDto.Name, developer.Name);
                Assert.Equal(developerDto.Login, developer.Login);
            }
            else
            {
                _developerRepository.Verify(d => d.UpdateAsync(developer), Times.Never);
            }
        }

        public static IEnumerable<object[]> DeveloperDeleteData()
        {
            yield return new object[] { Status.NotFund };
            yield return new object[] { Status.Success };
        }

        [Theory]
        [MemberData(nameof(DeveloperDeleteData))]
        public async void DeleteDeveloperTest(Status expectedStatus)
        {
            var developer = EntitiesFactory.NewDeveloper().Get();
            _developerRepository.Setup(d => d.ExistAsync(developer.Id))
                .ReturnsAsync(expectedStatus != Status.NotFund);
            _developerRepository.Setup(d => d.GetByIdAsync(developer.Id))
                .ReturnsAsync(developer);

            var service = new DeveloperService(_developerRepository.Object, _workRepository.Object, _mockyRepository.Object);
            var result = await service.DeleteDeveloperAsync(developer.Id);

            Assert.Equal(expectedStatus, result.Status);
            if (expectedStatus == Status.Success)
            {
                _developerRepository.Verify(d => d.DeleteAsync(developer), Times.Once);
            }
            else
            {
                _developerRepository.Verify(d => d.DeleteAsync(developer), Times.Never);
            }
        }
    }
}
