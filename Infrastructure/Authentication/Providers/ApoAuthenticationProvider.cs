using Domain.Entities;
using Domain.Interface.Repositories;
using Domain.Interface.Services.Authentication;
using Domain.Model.Authentication;
using Domain.Specifications;
using Microsoft.EntityFrameworkCore;

namespace Repositories.Authentication.Providers
{
    public class ApoAuthenticationProvider
           : IAuthenticationProvider
    {
        private readonly IRepository<SysUser> _repository;

        public string Key => "Apo";

        public ApoAuthenticationProvider(
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
