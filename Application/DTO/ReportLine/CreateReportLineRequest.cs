using System.ComponentModel.DataAnnotations;

namespace Application.DTO.ReportLine;

/// <summary>
/// 创建汇报关系请求
/// </summary>
public class CreateReportLineRequest
{
    [Required(ErrorMessage = "员工不能为空")]
    public Guid UserCode { get; set; }

    [Required(ErrorMessage = "直接上级不能为空")]
    public Guid SupervisorUserCode { get; set; }

    [Required(ErrorMessage = "所在组织不能为空")]
    public Guid OrgCode { get; set; }

    public Guid? RoleCode { get; set; }
    public bool IsActive { get; set; } = true;
    public DateOnly? EffectiveDate { get; set; }
    public DateOnly? ExpiryDate { get; set; }
}
