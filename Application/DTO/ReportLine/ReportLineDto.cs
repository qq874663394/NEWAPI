namespace Application.DTO.ReportLine;

/// <summary>
/// 汇报关系 DTO
/// </summary>
public class ReportLineDto
{
    public Guid Code { get; set; }
    public Guid UserCode { get; set; }
    public string UserName { get; set; } = string.Empty;
    public Guid SupervisorUserCode { get; set; }
    public string SupervisorUserName { get; set; } = string.Empty;
    public Guid OrgCode { get; set; }
    public string OrgName { get; set; } = string.Empty;
    public Guid? RoleCode { get; set; }
    public string? RoleName { get; set; }
    public bool IsActive { get; set; }
    public DateOnly? EffectiveDate { get; set; }
    public DateOnly? ExpiryDate { get; set; }
    public DateTime CreateTime { get; set; }
}
