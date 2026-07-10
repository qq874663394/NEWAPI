using System.ComponentModel.DataAnnotations;

namespace Application.DTO.Button;

/// <summary>
/// 创建按钮请求
/// </summary>
public class CreateButtonRequest
{
    [Required(ErrorMessage = "按钮Key不能为空")]
    public string ButtonKey { get; set; } = string.Empty;

    [Required(ErrorMessage = "按钮名称不能为空")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "所属菜单不能为空")]
    public Guid RouteCode { get; set; }

    [Required(ErrorMessage = "前端事件不能为空")]
    public string Event { get; set; } = string.Empty;

    public string? StyleType { get; set; }
    public int Type { get; set; }
    public string? Icon { get; set; }
    public int? Sort { get; set; }
    public bool IsSystemButton { get; set; }
}
