using System;
using System.Threading.Tasks;
using Tasks.Domain._Common.Results;
using Tasks.Domain.Works.Dtos;

namespace Tasks.Domain.Works.Services
{
    public interface IWorkService
    {
        Task<Result> CreateWorkAsync(WorkCreateDto workDto);
        Task<Result> UpdateWorkAsync(WorkUpdateDto workDto);
        Task<Result> DeleteWorkAsync(Guid id);
    }
}
