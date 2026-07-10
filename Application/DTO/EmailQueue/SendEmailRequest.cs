using System.ComponentModel.DataAnnotations;

namespace Application.DTO.EmailQueue;

/// <summary>
/// 发送邮件请求
/// </summary>
public class SendEmailRequest
{
    [Required(ErrorMessage = "邮件标题不能为空")]
    public string Subject { get; set; } = string.Empty;

    [Required(ErrorMessage = "邮件内容不能为空")]
    public string Content { get; set; } = string.Empty;

    [Required(ErrorMessage = "收件人不能为空")]
    public string ToAddress { get; set; } = string.Empty;

    public string? CcAddress { get; set; }
    public string? BccAddress { get; set; }
    public bool IsBodyHtml { get; set; } = true;
    public string? BusinessType { get; set; }
    public string? BusinessId { get; set; }
    public string? Remark { get; set; }
}
