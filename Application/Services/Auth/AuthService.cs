using Application.DTO.Auth;
using Application.Interfaces.Auth;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.UnitOfWorks;

namespace Application.Services.Auth;

public class AuthService : IAuthService
{
    private readonly IAuthenticationProviderRegistry _registry;
    private readonly ITokenService _tokenService;
    private readonly IRepository<SysUser> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public AuthService(
        IRepository<SysUser> repository,
        IAuthenticationProviderRegistry registry,
        ITokenService tokenService,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _registry = registry;
        _tokenService = tokenService;
        _unitOfWork = unitOfWork;
    }

    // =========================
    // 登录入口（唯一入口）
    // =========================
    public async Task<LoginResult?> LoginAsync(AuthenticationRequest request)
    {
        var provider = ResolveProvider(request);

        var user = await provider.AuthenticateAsync(request);

        if (user == null)
            return null;

        user.LastLoginTime = DateTime.Now;

        _repository.Update(user);
        await _unitOfWork.CommitAsync();

        var token = _tokenService.CreateToken(user.Code, user.Name);

        return new LoginResult
        {
            UserCode = user.Code,
            UserName = user.Name,
            AccessToken = token.AccessToken,
            RefreshToken = token.RefreshToken,
            ExpireAt = token.ExpireAt
        };
    }

    // =========================
    // Refresh（简化版）
    // =========================
    public string RefreshToken(string token)
    {
        return _tokenService.RefreshToken(token).AccessToken;
    }

    // =========================
    // Validate
    // =========================
    public Guid? ValidateToken(string token)
    {
        return _tokenService.ValidateToken(token);
    }

    // =========================
    // Provider 解析
    // =========================
    private IAuthenticationProvider ResolveProvider(AuthenticationRequest request)
    {
        // 优先根据 AuthType 选择 Provider
        if (!string.IsNullOrWhiteSpace(request.AuthType))
        {
            return _registry.GetProvider(request.AuthType);
        }

        // 回退：根据用户名特征自动推断
        if (request.UserName.Contains("@"))
            return _registry.GetProvider("Ldap");

        if (request.UserName.StartsWith("wx_"))
            return _registry.GetProvider("WeChat");

        return _registry.GetProvider("Password");
    }
}