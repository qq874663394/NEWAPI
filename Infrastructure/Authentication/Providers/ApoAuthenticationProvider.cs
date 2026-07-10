using Application.DTO.Auth;
using Application.Interfaces.Auth;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Authentication.Providers;

/// <summary>
/// APO 认证提供者（轻量级方案 A：按 Apo 字段查询数据库）
/// </summary>
public class ApoAuthenticationProvider : IAuthenticationProvider
{
    private readonly IRepository<SysUser> _repository;
    private readonly ILogger<ApoAuthenticationProvider> _logger;

    /// <summary>
    /// 认证提供者唯一标识
    /// </summary>
    public string Key => "Apo";

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="repository">用户仓储</param>
    /// <param name="logger">日志记录器</param>
    public ApoAuthenticationProvider(
        IRepository<SysUser> repository,
        ILogger<ApoAuthenticationProvider> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    /// <summary>
    /// APO 认证：按 Apo 账号字段查询数据库
    /// </summary>
    /// <param name="request">认证请求，UserName 字段对应 Apo 账号</param>
    /// <returns>匹配的用户实体，未找到返回 null</returns>
    public async Task<SysUser?> AuthenticateAsync(AuthenticationRequest request)
    {
        _logger.LogInformation("APO 认证尝试：UserName={UserName}", request.UserName);

        var user = await _repository.Query()
            .Include(x => x.UserRoleOrgs)
            .FirstOrDefaultAsync(x => x.Apo == request.UserName);

        if (user == null)
        {
            _logger.LogWarning("APO 认证失败，未找到匹配用户：Apo={Apo}", request.UserName);
            return null;
        }

        _logger.LogInformation(
            "APO 认证成功：UserCode={UserCode}, UserName={UserName}",
            user.Code, user.Name);

        return user;
    }
}
