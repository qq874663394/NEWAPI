using Application.DTO.Auth;
using Application.Interfaces.Auth;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.Authentication.Security;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Authentication.Providers;

public class PasswordAuthenticationProvider : IAuthenticationProvider
{
    private readonly IRepository<SysUser> _repository;

    public string Key => "Password";

    public PasswordAuthenticationProvider(IRepository<SysUser> repository)
    {
        _repository = repository;
    }

    public async Task<SysUser?> AuthenticateAsync(AuthenticationRequest request)
    {
         var user = await _repository.Query()
             .Include(x => x.UserRoleOrgs)
             .FirstOrDefaultAsync(x => x.Name == request.UserName);


        if (user == null)
            return null;
        if (!CryptHelper.VerifyPassword(request.Password, user.Password))
            return null;

        user.LastLoginTime = DateTime.Now;

        return user;
    }
}