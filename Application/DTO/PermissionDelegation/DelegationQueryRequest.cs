namespace Application.DTO.PermissionDelegation;

/// <summary>
/// 权限委托查询请求
/// </summary>
public class DelegationQueryRequest
{
    public Guid? FromUserCode { get; set; }
    public Guid? ToUserCode { get; set; }
    public Guid? ButtonCode { get; set; }
    public Guid? RouteCode { get; set; }
    public bool? IsActive { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}
