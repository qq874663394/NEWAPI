using Application.DTO.User;
using Application.Options;

namespace Application.Interfaces.User;

/// <summary>
/// 用户管理服务接口
/// </summary>
public interface IUserService
{
    /// <summary>
    /// 根据 Code 获取用户详情（含组织 + 角色列表）
    /// </summary>
    Task<UserDto?> GetByIdAsync(Guid code);

    /// <summary>
    /// 分页查询用户列表，支持关键字搜索和组织筛选
    /// </summary>
    Task<PagedResult<UserDto>> GetListAsync(UserQueryRequest query);

    /// <summary>
    /// 创建用户（含初始岗位分配）
    /// </summary>
    Task<UserDto> CreateAsync(CreateUserRequest request);

    /// <summary>
    /// 更新用户基本信息
    /// </summary>
    Task<UserDto> UpdateAsync(UpdateUserRequest request);

    /// <summary>
    /// 软删除用户
    /// </summary>
    Task DeleteAsync(Guid code);

    /// <summary>
    /// 修改密码（需验证旧密码）
    /// </summary>
    Task ChangePasswordAsync(ChangePasswordRequest request);

    /// <summary>
    /// 管理员强制重置密码
    /// </summary>
    Task ResetPasswordAsync(Guid code, string newPassword);

    /// <summary>
    /// 锁定用户
    /// </summary>
    Task LockAsync(Guid code);

    /// <summary>
    /// 解锁用户
    /// </summary>
    Task UnlockAsync(Guid code);

    /// <summary>
    /// 调整用户的角色 + 组织分配（先删后增）
    /// </summary>
    Task AssignRolesAsync(Guid userCode, List<AssignRoleOrgRequest> assignments);
}
