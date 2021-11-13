using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tasks.Domain._Common.Dtos;
using Tasks.Domain._Common.Enums;
using Tasks.Domain._Common.Results;
using Tasks.Domain.Developers.Dtos;
using Tasks.Domain.Developers.Dtos.Ranking;
using Tasks.Domain.Developers.Dtos.Works;
using Tasks.Domain.Developers.Entities;
using Tasks.Domain.Developers.Repositories;
using Tasks.Domain.Developers.Services;
using Tasks.Domain.External.Services;
using Tasks.Domain.Projects.Dtos;
using Tasks.Domain.Works.Repositories;

namespace Tasks.Service.Developers
{
    public class DeveloperService : IDeveloperService
    {
        private readonly IDeveloperRepository _developerRepository;
        private readonly IWorkRepository _workRepository;
        private readonly IMockyService _mockyService;
        public DeveloperService(
            IDeveloperRepository developerRepository,
            IWorkRepository workRepository,
            IMockyService mockyService
        )
        {
            _developerRepository = developerRepository;
            _workRepository = workRepository;
            _mockyService = mockyService;
        }

        public async Task<Result> CreateDeveloperAsync(DeveloperCreateDto developerDto)
        {
            var existLogin = await _developerRepository.ExistByLoginAsync(developerDto.Login);
            if (existLogin) return new Result(Status.Conflict, $"Developer with {nameof(developerDto.Login)} already exist");
            var validCpf = await _mockyService.ValidateCPFAsync(developerDto.CPF);
            if (!validCpf.Success) return new Result(validCpf.Status, validCpf.ErrorMessages);
            if (!validCpf.Data) return new Result(Status.Invalid, $"Parameter {nameof(developerDto.CPF)} is not valid");

            var developer = new Developer(
                id: developerDto.Id,
                name: developerDto.Name,
                login: developerDto.Login,
                cpf: developerDto.CPF,
                password: developerDto.Password
            );

            await _developerRepository.CreateAsync(developer);
            return new Result();
        }

        public async Task<Result> DeleteDeveloperAsync(Guid id)
        {
            var existDeveloper = await _developerRepository.ExistAsync(id);
            if (!existDeveloper) return new Result(Status.NotFund, $"Developer with {nameof(id)} does not exist");

            var developer = await _developerRepository.GetByIdAsync(id);
            await _developerRepository.DeleteAsync(developer);
            return new Result();
        }

        public async Task<Result<DeveloperDetailDto>> GetDeveloperByIdAsync(Guid id)
        {
            var existDeveloper = await _developerRepository.ExistAsync(id);
            if (!existDeveloper) return new Result<DeveloperDetailDto>(Status.NotFund, $"Developer with {nameof(id)} does not exist");

            var developer = await _developerRepository.GetByIdAsync(id);
            var developerDetail = new DeveloperDetailDto
            {
                Id = developer.Id,
                CPF = developer.CPF,
                Login = developer.Login,
                Name = developer.Name
            };

            return new Result<DeveloperDetailDto>(developerDetail);
        }

        public async Task<Result<IEnumerable<DeveloperRankingListDto>>> ListDeveloperRankingAsync(DeveloperRankingSearchDto searchDto)
        {
            var rawDeveloperList = await _developerRepository.Query()
                .Select(d => new {
                    Developer = new { d.Id, d.Name },
                    Works = d.DeveloperProjects
                        .SelectMany(dp => dp.Works)
                        .Where(w => w.StartTime >= searchDto.StartTime)
                        .Where(w => searchDto.ProjectId == null || w.DeveloperProject.ProjectId == searchDto.ProjectId)
                })
                .ToArrayAsync();

            var rankingList = rawDeveloperList
                .Select(d => new DeveloperRankingListDto
                {
                    Id = d.Developer.Id,
                    Name = d.Developer.Name,
                    SumHours = d.Works.Sum(w => w.Hours),
                    AvgHours = d.Works.Any() ? d.Works.Average(w => w.Hours) : 0
                })
                .OrderByDescending(d => d.AvgHours)
                .Take(5);

            return new Result<IEnumerable<DeveloperRankingListDto>>(rankingList, rankingList.Count());
        }

        public async Task<Result<IEnumerable<DeveloperListDto>>> ListDevelopersAsync(PaginationDto pagination)
        {
            var query = _developerRepository.Query();
            var developers = await query
                .Skip(pagination.CalculateOffset())
                .Take(pagination.Limit)
                .Select(d => new DeveloperListDto
                {
                    Id = d.Id,
                    Name = d.Name
                })
                .ToArrayAsync();

            return new Result<IEnumerable<DeveloperListDto>>(developers, await query.CountAsync());
        }

        public async Task<Result<IEnumerable<DeveloperWorkListDto>>> ListDeveloperWorksAsync(DeveloperWorkSearchDto searchDto)
        {
            var query = _workRepository.Query()
                .Where(w => w.DeveloperProject.DeveloperId == searchDto.DeveloperId)
                .Where(w => searchDto.ProjectId == null || w.DeveloperProject.ProjectId == searchDto.ProjectId);

            var developerWorks = await query
                .Skip(searchDto.CalculateOffset())
                .Take(searchDto.Limit)
                .Select(w => new DeveloperWorkListDto
                {
                    Id = w.Id,
                    StartTime = w.StartTime,
                    EndTime = w.EndTime,
                    Comment = w.Comment,
                    Hours = w.Hours,
                    Project = new ProjectListDto
                    {
                        Id = w.DeveloperProject.ProjectId,
                        Title = w.DeveloperProject.Project.Title
                    },
                })
                .ToArrayAsync();

            return new Result<IEnumerable<DeveloperWorkListDto>>(developerWorks, await query.CountAsync());
        }

        public async Task<Result> UpdateDeveloperAsync(DeveloperUpdateDto developerDto)
        {
            var existDeveloper = await _developerRepository.ExistAsync(developerDto.Id);
            if (!existDeveloper) return new Result(Status.NotFund, $"Developer with {nameof(developerDto.Id)} does not exist");
            var existLogin = await _developerRepository.ExistByLoginAsync(developerDto.Login, developerDto.Id);
            if (existLogin) return new Result(Status.Conflict, $"Developer with {nameof(developerDto.Login)} already exist");

            var developer = await _developerRepository.GetByIdAsync(developerDto.Id);
            developer.SetData(
                name: developerDto.Name,
                login: developerDto.Login
            );

            await _developerRepository.UpdateAsync(developer);
            return new Result();
        }
    }
}
