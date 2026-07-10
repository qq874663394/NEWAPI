using Application.DTO.Permission;

namespace Application.Interfaces.Permission;

/// <summary>
/// 菜单权限服务接口
/// </summary>
public interface IMenuPermissionService
{
    /// <summary>查询菜单权限列表</summary>
    Task<List<MenuPermissionDto>> GetListAsync(Guid? routeCode = null, string? subjectType = null, Guid? subjectCode = null);

    /// <summary>获取某角色/用户的已授权菜单列表</summary>
    Task<List<MenuPermissionDto>> GetGrantedMenusAsync(string subjectType, Guid subjectCode);

    /// <summary>分配/取消单个菜单权限（存在则更新，不存在则创建）</summary>
    Task<MenuPermissionDto> AssignAsync(AssignMenuPermissionRequest request);

    /// <summary>批量分配菜单权限</summary>
    Task BatchAssignAsync(BatchAssignMenuPermissionRequest request);

    /// <summary>移除某主体的所有菜单权限</summary>
    Task RemoveBySubjectAsync(string subjectType, Guid subjectCode);
}
