namespace Application.DTO.User;

/// <summary>
/// 用户详情响应 DTO
/// </summary>
public class UserDto
{
    /// <summary>用户 Code（GUID 主键）</summary>
    public Guid Code { get; set; }

    /// <summary>登录账号</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>用户全名</summary>
    public string FullName { get; set; } = string.Empty;

    /// <summary>APO 账号</summary>
    public string? Apo { get; set; }

    /// <summary>邮箱</summary>
    public string? Email { get; set; }

    /// <summary>手机号</summary>
    public string? Phone { get; set; }

    /// <summary>座机</summary>
    public string? Tel { get; set; }

    /// <summary>生日</summary>
    public DateTime? DateOfBirth { get; set; }

    /// <summary>性别（0=未知, 1=男, 2=女）</summary>
    public int? Sex { get; set; }

    /// <summary>证件类型</summary>
    public int? DocumentType { get; set; }

    /// <summary>证件号码</summary>
    public string? DocumentNumber { get; set; }

    /// <summary>是否活跃</summary>
    public bool IsActive { get; set; }

    /// <summary>是否已锁定</summary>
    public bool IsLocked { get; set; }

    /// <summary>锁定结束时间</summary>
    public DateTime? LockEndTime { get; set; }

    /// <summary>最后登录时间</summary>
    public DateTime? LastLoginTime { get; set; }

    /// <summary>登录失败次数</summary>
    public int? FailedLoginCount { get; set; }

    /// <summary>最后修改密码时间</summary>
    public DateTime? PasswordLastSetTime { get; set; }

    /// <summary>创建时间</summary>
    public DateTime? CreateTime { get; set; }

    /// <summary>修改时间</summary>
    public DateTime? ModifyTime { get; set; }

    /// <summary>所属组织与角色列表</summary>
    public List<UserOrgRoleDto> OrgRoles { get; set; } = new();
}
