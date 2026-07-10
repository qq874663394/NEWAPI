using System.ComponentModel.DataAnnotations;

namespace Application.DTO.Permission;

/// <summary>
/// 分配按钮权限请求
/// </summary>
public class AssignButtonPermissionRequest
{
    [Required]
    public Guid ButtonCode { get; set; }

    [Required(ErrorMessage = "授权主体类型不能为空")]
    public string SubjectType { get; set; } = string.Empty;

    [Required]
    public Guid SubjectCode { get; set; }

    public bool IsGranted { get; set; }
}
