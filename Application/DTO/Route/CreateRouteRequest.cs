using System.ComponentModel.DataAnnotations;

namespace Application.DTO.Route;

/// <summary>
/// 创建菜单请求
/// </summary>
public class CreateRouteRequest
{
    [Required(ErrorMessage = "菜单名称不能为空")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "路由路径不能为空")]
    public string Path { get; set; } = string.Empty;

    public string? Component { get; set; }
    public string? Redirect { get; set; }
    public Guid? ParentCode { get; set; }
    public string? MetaTitle { get; set; }
    public string? MetaIcon { get; set; }
    public bool Hidden { get; set; }
    public bool AlwaysShow { get; set; }
    public bool MetaNoCache { get; set; }
    public bool MetaAffix { get; set; }
    public string? MetaActiveMenu { get; set; }
    public int Sort { get; set; }
}
