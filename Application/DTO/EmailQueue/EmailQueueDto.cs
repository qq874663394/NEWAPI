namespace Application.DTO.EmailQueue;

/// <summary>
/// 邮件队列 DTO
/// </summary>
public class EmailQueueDto
{
    public Guid Code { get; set; }
    public string EmailCode { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string ToAddress { get; set; } = string.Empty;
    public string? CcAddress { get; set; }
    public string? BccAddress { get; set; }
    public bool IsBodyHtml { get; set; }
    public int SendStatus { get; set; }
    public int RetryCount { get; set; }
    public int MaxRetryCount { get; set; }
    public DateTime? SentTime { get; set; }
    public string? ErrorMessage { get; set; }
    public string? BusinessType { get; set; }
    public string? BusinessId { get; set; }
    public string? Remark { get; set; }
    public DateTime CreateTime { get; set; }
}
