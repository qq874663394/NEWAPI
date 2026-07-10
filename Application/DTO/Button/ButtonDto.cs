namespace Application.DTO.Button;

/// <summary>
/// 按钮 DTO
/// </summary>
public class ButtonDto
{
    public Guid Code { get; set; }
    public string ButtonKey { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public Guid RouteCode { get; set; }
    public string? RouteName { get; set; }
    public string Event { get; set; } = string.Empty;
    public string? StyleType { get; set; }
    public int Type { get; set; }
    public string? Icon { get; set; }
    public int? Sort { get; set; }
    public bool IsSystemButton { get; set; }
    public bool IsEnable { get; set; }
    public DateTime CreateTime { get; set; }
}
