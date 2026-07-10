using Application.DTO.PermissionDelegation;
using Application.Options;

namespace Application.Interfaces.PermissionDelegation;

/// <summary>
/// 权限委托服务接口
/// </summary>
public interface IPermissionDelegationService
{
    /// <summary>分页查询权限委托</summary>
    Task<PagedResult<PermissionDelegationDto>> GetPagedListAsync(DelegationQueryRequest request);

    /// <summary>获取权限委托详情</summary>
    Task<PermissionDelegationDto?> GetByIdAsync(Guid code);

    /// <summary>创建权限委托</summary>
    Task<PermissionDelegationDto> CreateAsync(CreateDelegationRequest request);

    /// <summary>删除权限委托</summary>
    Task DeleteAsync(Guid code);

    /// <summary>设置启用/禁用</summary>
    Task SetActiveAsync(Guid code, bool isActive);
}
