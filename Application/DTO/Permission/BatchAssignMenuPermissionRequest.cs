using System.ComponentModel.DataAnnotations;

namespace Application.DTO.Permission;

/// <summary>
/// 批量分配菜单权限请求
/// </summary>
public class BatchAssignMenuPermissionRequest
{
    [Required]
    public string SubjectType { get; set; } = string.Empty;

    [Required]
    public Guid SubjectCode { get; set; }

    /// <summary>授权的菜单Code列表</summary>
    public List<Guid> GrantRouteCodes { get; set; } = new();

    /// <summary>取消授权的菜单Code列表</summary>
    public List<Guid> RevokeRouteCodes { get; set; } = new();
}
