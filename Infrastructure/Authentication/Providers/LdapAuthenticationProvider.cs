using Domain.Entities;
using Domain.Interface.Repositories;
using Domain.Interface.Services.Authentication;
using Domain.Model.Authentication;
using Microsoft.EntityFrameworkCore;

namespace Repositories.Authentication.Providers
{
    public class LdapAuthenticationProvider
            : IAuthenticationProvider
    {
        private readonly IRepository<SysUser> _repository;
        public string Key => "Ldap";
        public LdapAuthenticationProvider(
            IRepository<SysUser> repository)
        {
            _repository = repository;
        }
        public async Task<SysUser?> AuthenticateAsync(
            AuthenticationRequest request)
        {
            return await _repository.Query()
                .Where(p => p.Apo != null && p.Apo == request.Username)
                .Include(p => p.UserRoleOrgs)
                .FirstOrDefaultAsync();
        }
    }
}
