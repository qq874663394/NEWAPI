namespace Application.DTO.User;

/// <summary>
/// 用户的一个岗位（角色 + 组织对）
/// </summary>
public class UserOrgRoleDto
{
    /// <summary>组织 Code</summary>
    public Guid OrgCode { get; set; }

    /// <summary>组织名称</summary>
    public string OrgName { get; set; } = string.Empty;

    /// <summary>组织路径（如 /公司/本部/部/课）</summary>
    public string OrgPath { get; set; } = string.Empty;

    /// <summary>组织类型</summary>
    public string OrgType { get; set; } = string.Empty;

    /// <summary>角色 Code</summary>
    public Guid RoleCode { get; set; }

    /// <summary>角色名称</summary>
    public string RoleName { get; set; } = string.Empty;

    /// <summary>角色层级</summary>
    public int RoleLevel { get; set; }

    /// <summary>是否主岗</summary>
    public bool IsPrimary { get; set; }
}
