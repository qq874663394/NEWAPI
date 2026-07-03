using Application.Auth;
using Application.Auth.DTO;
using Domain.Entities;
using Domain.Interface.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Authentication.Providers;

public class ApoAuthenticationProvider : IAuthenticationProvider
{
    private readonly IRepository<SysUser> _repository;

    public string Key => "Apo";

    public ApoAuthenticationProvider(IRepository<SysUser> repository)
    {
        _repository = repository;
    }

    public async Task<SysUser?> AuthenticateAsync(AuthenticationRequest request)
    {
        return await _repository.Query()
            .Include(x => x.UserRoleOrgs)
            .FirstOrDefaultAsync(x => x.Apo == request.UserName);
    }
}