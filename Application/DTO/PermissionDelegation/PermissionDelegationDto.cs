namespace Application.DTO.PermissionDelegation;

/// <summary>
/// 权限委托 DTO
/// </summary>
public class PermissionDelegationDto
{
    public Guid Code { get; set; }
    public Guid FromUserCode { get; set; }
    public string FromUserName { get; set; } = string.Empty;
    public Guid ToUserCode { get; set; }
    public string ToUserName { get; set; } = string.Empty;
    public Guid ButtonCode { get; set; }
    public string ButtonName { get; set; } = string.Empty;
    public Guid? RouteCode { get; set; }
    public string? RouteName { get; set; }
    public string? Condition { get; set; }
    public DateTime EffectiveStartDate { get; set; }
    public DateTime EffectiveEndDate { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreateTime { get; set; }
}
