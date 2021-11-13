using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Tasks.Domain._Common.Results;
using Tasks.Domain.Developers.Dtos.Auth;
using Tasks.Domain.Developers.Services;

namespace Tasks.API.Controllers
{
    public class AuthController : TasksControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost("login", Order = 2)]
        public async Task<Result<TokenDto>> LoginAsync([FromBody] LoginDto loginDto)
        {
            return GetResult(await _authService.LoginAsync(loginDto));
        }

        [HttpGet("logout")]
        public Result Logout() => GetResult(new Result());
    }
}
