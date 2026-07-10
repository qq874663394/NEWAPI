namespace Application.DTO.ReportLine;

/// <summary>
/// 汇报关系查询请求
/// </summary>
public class ReportLineQueryRequest
{
    public Guid? UserCode { get; set; }
    public Guid? SupervisorUserCode { get; set; }
    public Guid? OrgCode { get; set; }
    public bool? IsActive { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}
