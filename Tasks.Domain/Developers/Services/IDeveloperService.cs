using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tasks.Domain._Common.Dtos;
using Tasks.Domain._Common.Results;
using Tasks.Domain.Developers.Dtos;
using Tasks.Domain.Developers.Dtos.Ranking;
using Tasks.Domain.Developers.Dtos.Works;

namespace Tasks.Domain.Developers.Services
{
    public interface IDeveloperService
    {
        Task<Result<DeveloperDetailDto>> GetDeveloperByIdAsync(Guid id);
        Task<Result<IEnumerable<DeveloperListDto>>> ListDevelopersAsync(PaginationDto pagination);
        Task<Result<IEnumerable<DeveloperWorkListDto>>> ListDeveloperWorksAsync(DeveloperWorkSearchDto searchDto);
        Task<Result<IEnumerable<DeveloperRankingListDto>>> ListDeveloperRankingAsync(DeveloperRankingSearchDto searchDto);
        Task<Result> CreateDeveloperAsync(DeveloperCreateDto developerDto);
        Task<Result> UpdateDeveloperAsync(DeveloperUpdateDto developerDto);
        Task<Result> DeleteDeveloperAsync(Guid id);
    }
}
