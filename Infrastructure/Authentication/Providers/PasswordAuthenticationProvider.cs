using Domain.Entities;
using Domain.Interface.Repositories;
using Domain.Interface.Services.Authentication;
using Domain.Model.Authentication;
using Domain.Specifications;
using Infrastructure.Authentication.Security;
using Microsoft.EntityFrameworkCore;

namespace Repositories.Authentication.Providers
{
    public class PasswordAuthenticationProvider
           : IAuthenticationProvider
    {
        private readonly IRepository<SysUser> _repository;

        public string Key => "Password";

        public PasswordAuthenticationProvider(
            IRepository<SysUser> repository)
        {
            _repository = repository;
        }

        public async Task<SysUser?> AuthenticateAsync(
            AuthenticationRequest request)
        {
            var user = await _repository.Query()
                .Where(p => p.Apo != null && p.Apo == request.Username)
                .Include(p => p.UserRoleOrgs)
                .FirstOrDefaultAsync();

            if (user == null)
                return null;

            if (!CryptHelper.VerifyPassword(
                    request.Password,
                    user.Password))
            {
                return null;
            }

            user.LastLoginTime = DateTime.Now;

            return user;
        }
    }
}
