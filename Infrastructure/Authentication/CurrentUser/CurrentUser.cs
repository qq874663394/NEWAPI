using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Authentication.CurrentUser
{
    /// <summary>
    /// 当前登录用户
    /// </summary>
    public sealed class CurrentUser : ICurrentUser
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUser(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;

        public bool IsAuthenticated =>
            User?.Identity?.IsAuthenticated ?? false;

        public string? Code =>
            User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        public string? Name =>
            User?.FindFirst(ClaimTypes.Name)?.Value;

        public string? FullName =>
            User?.FindFirst(ClaimTypes.GivenName)?.Value;

        public IEnumerable<Claim> Claims =>
            User?.Claims ?? Enumerable.Empty<Claim>();

        public bool IsInRole(string role)
        {
            return User?.IsInRole(role) ?? false;
        }
    }
}
