using System.Threading.Tasks;
using Tasks.Domain._Common.Results;

namespace Tasks.Domain.External.Services
{
    public interface IMockyService
    {
        Task<Result<bool>> ValidateCPFAsync(string cpf);
        Task<Result<bool>> SendNotificationAsync(string title, string message);
    }
}
