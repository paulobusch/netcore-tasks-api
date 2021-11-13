using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Tasks.Domain._Common.Enums;
using Tasks.Domain._Common.Results;
using Tasks.Domain._Common.Security;
using Tasks.Domain.Developers.Dtos.Auth;
using Tasks.Domain.Developers.Entities;
using Tasks.Domain.Developers.Repositories;
using Tasks.Domain.Developers.Services;

namespace Tasks.Service.Developers
{
    public class AuthService : IAuthService
    {
        private readonly IDeveloperRepository _developerRepository;
        private readonly TokenConfiguration _tokenConfiguration;

        public AuthService(
            IDeveloperRepository developerRepository,
            TokenConfiguration tokenConfiguration
        )
        {
            _developerRepository = developerRepository;
            _tokenConfiguration = tokenConfiguration;
        }

        public async Task<Result<TokenDto>> LoginAsync(LoginDto loginDto)
        {
            var defaultReject = new Result<TokenDto>(Status.Unauthorized, "Login or Password is not valid");
            var exists = await _developerRepository.ExistByLoginAsync(loginDto.Login);
            if (!exists) return defaultReject;
            var developer = await _developerRepository.FindByLoginAsync(loginDto.Login);
            if (!developer.ValidatePassword(loginDto.Password)) return defaultReject;
            return new Result<TokenDto>(await GenerateJwtTokenAsync(developer));
        }

        public async Task<TokenDto> GenerateJwtTokenAsync(Developer developer)
        {
            if (developer == null || string.IsNullOrWhiteSpace(developer.Login)) return null;
            var userClaims = new[] {
                new Claim(nameof(developer.Id), developer.Id.ToString()),
                new Claim(nameof(developer.Login), developer.Login)
            };

            var identityClaims = new ClaimsIdentity(userClaims);

            var created = DateTime.UtcNow;
            var expires = created.AddSeconds(_tokenConfiguration.Seconds);
            var handler = new JwtSecurityTokenHandler();
            var symmetricKey = Encoding.ASCII.GetBytes(_tokenConfiguration.Signature);
            var securityToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _tokenConfiguration.Issuer,
                Subject = identityClaims,
                NotBefore = created,
                Expires = expires,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(symmetricKey),
                    SecurityAlgorithms.HmacSha256Signature
                )
            });
            var tokenDto = new TokenDto
            {
                Id = developer.Id,
                Name = developer.Name,
                Login = developer.Login,
                Token = handler.WriteToken(securityToken),
                CreatedAt = created,
                ExpiresAt = expires
            };
            return await Task.FromResult(tokenDto);
        }
    }
}
