namespace Application.DTO.EmailQueue;

/// <summary>
/// 邮件队列查询请求
/// </summary>
public class EmailQueueQueryRequest
{
    public int? SendStatus { get; set; }
    public string? Keyword { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}
