using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using Tasks.Domain._Common.Session;

namespace Tasks.Services._Common.Session
{
    public class AutenticationContext : IAutenticationContext
    {
        public bool IsAuthenticated => _context.HttpContext.User.Identity.IsAuthenticated;
        public Guid Id => new Guid(GetClaimValue<string>(nameof(Id)));
        public string Login => GetClaimValue<string>(nameof(Login));

        private readonly IHttpContextAccessor _context;

        public AutenticationContext(IHttpContextAccessor context)
        {
            _context = context;
        }

        private T GetClaimValue<T>(string key)
        {
            if (!_context.HttpContext.User.Identity.IsAuthenticated) return default;
            var claim = _context.HttpContext.User.Claims.FirstOrDefault(c => c.Type == key);
            if (claim == null) return default;
            return (T)Convert.ChangeType(claim.Value, typeof(T));
        }
    }
}
