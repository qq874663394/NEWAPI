using System.ComponentModel.DataAnnotations;

namespace Application.DTO.Permission;

/// <summary>
/// 分配菜单权限请求
/// </summary>
public class AssignMenuPermissionRequest
{
    [Required]
    public Guid RouteCode { get; set; }

    [Required(ErrorMessage = "授权主体类型不能为空")]
    public string SubjectType { get; set; } = string.Empty;

    [Required]
    public Guid SubjectCode { get; set; }

    public bool IsGranted { get; set; }
}
