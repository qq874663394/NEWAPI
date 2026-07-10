namespace Application.DTO.Route;

/// <summary>
/// 菜单 DTO
/// </summary>
public class RouteDto
{
    public Guid Code { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Path { get; set; } = string.Empty;
    public string? Component { get; set; }
    public string? Redirect { get; set; }
    public Guid? ParentCode { get; set; }
    public string? ParentName { get; set; }
    public string? MetaTitle { get; set; }
    public string? MetaIcon { get; set; }
    public bool Hidden { get; set; }
    public bool AlwaysShow { get; set; }
    public bool MetaNoCache { get; set; }
    public bool MetaAffix { get; set; }
    public string? MetaActiveMenu { get; set; }
    public int Sort { get; set; }
    public bool IsEnable { get; set; }
    public DateTime CreateTime { get; set; }
    public List<RouteDto>? Children { get; set; }
}
