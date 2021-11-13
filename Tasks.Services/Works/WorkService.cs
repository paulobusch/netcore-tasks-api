using System;
using System.Threading.Tasks;
using Tasks.Domain._Common.Enums;
using Tasks.Domain._Common.Results;
using Tasks.Domain.Developers.Repositories;
using Tasks.Domain.External.Services;
using Tasks.Domain.Projects.Repositories;
using Tasks.Domain.Works.Dtos;
using Tasks.Domain.Works.Entities;
using Tasks.Domain.Works.Repositories;
using Tasks.Domain.Works.Services;

namespace Tasks.Services.Works
{
    public class WorkService : IWorkService
    {
        private readonly IWorkRepository _workRepository;
        private readonly IDeveloperRepository _developerRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IMockyService _mockyService;

        public WorkService(
            IWorkRepository workRepository,
            IDeveloperRepository developerRepository,
            IProjectRepository projectRepository,
            IMockyService mockyService
        ) {
            _workRepository = workRepository;
            _developerRepository = developerRepository;
            _projectRepository = projectRepository;
            _mockyService = mockyService;
        }

        public async Task<Result> CreateWorkAsync(WorkCreateDto workDto)
        {
            var resultValidation = ValidateRanteDateTime(workDto.StartTime, workDto.EndTime);
            if (!resultValidation.Success) return resultValidation;
            var existProject = await _projectRepository.ExistAsync(workDto.ProjectId);
            if (!existProject) return new Result(Status.NotFund, $"Project with {nameof(workDto.ProjectId)} does not exist");
            var existDeveloper = await _developerRepository.ExistAsync(workDto.DeveloperId);
            if (!existDeveloper) return new Result(Status.NotFund, $"Developer with {nameof(workDto.DeveloperId)} does not exist");
            var developerVinculatedProject = await _projectRepository.ExistDeveloperVinculatedAsync(workDto.ProjectId, workDto.DeveloperId);
            if (!developerVinculatedProject) return new Result(Status.NotAllowed, $"Developer is not vinculated in Project");

            var developerProjectId = await _projectRepository.GetDeveloperProjectIdAsync(workDto.ProjectId, workDto.DeveloperId);
            var work = new Work(
                id: workDto.Id,
                developerProjectId: developerProjectId,
                startTime: workDto.StartTime,
                endTime: workDto.EndTime,
                comment: workDto.Comment,
                hours: workDto.Hours
            );

            await _workRepository.CreateAsync(work);
            var result = await _mockyService.SendNotificationAsync("Lançamento de horas", "Um novo lançamento de horas foi realizado");
            if (!result.Success || !result.Data) return new Result(Status.Error, result.ErrorMessages);
            return new Result();
        }

        public async Task<Result> DeleteWorkAsync(Guid id)
        {
            var existWork = await _workRepository.ExistAsync(id);
            if (!existWork) return new Result(Status.NotFund, $"Work with {nameof(id)} does not exist");
            
            var work = await _workRepository.GetByIdAsync(id);
            await _workRepository.DeleteAsync(work);
            var result = await _mockyService.SendNotificationAsync("Lançamento de horas", "Um lançamento de horas foi removido");
            if (!result.Success || !result.Data) return new Result(Status.Error, result.ErrorMessages);
            return new Result();
        }

        public async Task<Result> UpdateWorkAsync(WorkUpdateDto workDto)
        {
            var resultValidation = ValidateRanteDateTime(workDto.StartTime, workDto.EndTime);
            if (!resultValidation.Success) return resultValidation;
            var existWork = await _workRepository.ExistAsync(workDto.Id);
            if (!existWork) return new Result(Status.NotFund, $"Work with {nameof(workDto.Id)} does not exist");

            var work = await _workRepository.GetByIdAsync(workDto.Id);
            work.SetData(
                startTime: workDto.StartTime,
                endTime: workDto.EndTime,
                comment: workDto.Comment,
                hours: workDto.Hours
            );

            await _workRepository.UpdateAsync(work);
            var result = await _mockyService.SendNotificationAsync("Lançamento de horas", "Um lançamento de horas foi atualizado");
            if (!result.Success || !result.Data) return new Result(Status.Error, result.ErrorMessages);
            return new Result();
        }

        private Result ValidateRanteDateTime(DateTime startTime, DateTime endTime)
        {
            var now = DateTime.Now;
            if (endTime > now) return new Result(Status.Invalid, $"StartTime cannot be greater than Now");
            if (startTime > endTime) return new Result(Status.Invalid, $"EndTime cannot be greater than EndTime");
            return new Result();
        }
    }
}
