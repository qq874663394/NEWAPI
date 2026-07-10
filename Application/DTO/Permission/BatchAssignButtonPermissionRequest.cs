using System.ComponentModel.DataAnnotations;

namespace Application.DTO.Permission;

/// <summary>
/// 批量分配按钮权限请求
/// </summary>
public class BatchAssignButtonPermissionRequest
{
    [Required]
    public string SubjectType { get; set; } = string.Empty;

    [Required]
    public Guid SubjectCode { get; set; }

    /// <summary>授权的按钮Code列表</summary>
    public List<Guid> GrantButtonCodes { get; set; } = new();

    /// <summary>取消授权的按钮Code列表</summary>
    public List<Guid> RevokeButtonCodes { get; set; } = new();
}
