namespace Application.DTO.User;

/// <summary>
/// 分配角色 + 组织请求（一个岗位）
/// </summary>
public class AssignRoleOrgRequest
{
    /// <summary>角色 Code</summary>
    public Guid RoleCode { get; set; }

    /// <summary>组织 Code</summary>
    public Guid OrgCode { get; set; }

    /// <summary>是否主岗</summary>
    public bool IsPrimary { get; set; }
}
