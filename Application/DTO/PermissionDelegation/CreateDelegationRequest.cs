using System.ComponentModel.DataAnnotations;

namespace Application.DTO.PermissionDelegation;

/// <summary>
/// 创建权限委托请求
/// </summary>
public class CreateDelegationRequest
{
    [Required(ErrorMessage = "授权人不能为空")]
    public Guid FromUserCode { get; set; }

    [Required(ErrorMessage = "被授权人不能为空")]
    public Guid ToUserCode { get; set; }

    [Required(ErrorMessage = "按钮不能为空")]
    public Guid ButtonCode { get; set; }

    public Guid? RouteCode { get; set; }
    public string? Condition { get; set; }

    [Required(ErrorMessage = "生效开始时间不能为空")]
    public DateTime EffectiveStartDate { get; set; }

    [Required(ErrorMessage = "生效结束时间不能为空")]
    public DateTime EffectiveEndDate { get; set; }

    public bool IsActive { get; set; } = true;
}
