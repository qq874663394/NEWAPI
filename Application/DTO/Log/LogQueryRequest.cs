namespace Application.DTO.Log;

/// <summary>
/// 日志查询请求
/// </summary>
public class LogQueryRequest
{
    public string? Type { get; set; }
    public string? Keyword { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}
