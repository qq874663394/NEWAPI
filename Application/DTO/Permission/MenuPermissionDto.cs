namespace Application.DTO.Permission;

/// <summary>
/// 菜单权限 DTO
/// </summary>
public class MenuPermissionDto
{
    public Guid Code { get; set; }
    public Guid RouteCode { get; set; }
    public string? RouteName { get; set; }
    public string SubjectType { get; set; } = string.Empty;
    public Guid SubjectCode { get; set; }
    public bool IsGranted { get; set; }
    public DateTime CreateTime { get; set; }
}
