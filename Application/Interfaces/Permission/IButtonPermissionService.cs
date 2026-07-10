using Application.DTO.Permission;

namespace Application.Interfaces.Permission;

/// <summary>
/// 按钮权限服务接口
/// </summary>
public interface IButtonPermissionService
{
    /// <summary>查询按钮权限列表</summary>
    Task<List<ButtonPermissionDto>> GetListAsync(Guid? buttonCode = null, string? subjectType = null, Guid? subjectCode = null);

    /// <summary>获取某角色/用户的已授权按钮Code列表</summary>
    Task<List<Guid>> GetGrantedButtonCodesAsync(string subjectType, Guid subjectCode);

    /// <summary>分配/取消单个按钮权限</summary>
    Task<ButtonPermissionDto> AssignAsync(AssignButtonPermissionRequest request);

    /// <summary>批量分配按钮权限</summary>
    Task BatchAssignAsync(BatchAssignButtonPermissionRequest request);
}
