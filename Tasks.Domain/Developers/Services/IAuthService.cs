using System.Threading.Tasks;
using Tasks.Domain._Common.Results;
using Tasks.Domain.Developers.Dtos.Auth;
using Tasks.Domain.Developers.Entities;

namespace Tasks.Domain.Developers.Services
{
    public interface IAuthService
    {
        Task<Result<TokenDto>> LoginAsync(LoginDto loginDto);
        Task<TokenDto> GenerateJwtTokenAsync(Developer developer);
    }
}
