using Application.DTO.EmailQueue;
using Application.Options;

namespace Application.Interfaces.EmailQueue;

/// <summary>
/// 邮件队列服务接口
/// </summary>
public interface IEmailQueueService
{
    /// <summary>分页查询邮件队列</summary>
    Task<PagedResult<EmailQueueDto>> GetPagedListAsync(EmailQueueQueryRequest request);

    /// <summary>获取邮件详情</summary>
    Task<EmailQueueDto?> GetByIdAsync(Guid code);

    /// <summary>创建并发送邮件（加入队列）</summary>
    Task<EmailQueueDto> SendAsync(SendEmailRequest request);

    /// <summary>重新发送失败邮件</summary>
    Task<EmailQueueDto> ResendAsync(Guid code);

    /// <summary>删除邮件记录</summary>
    Task DeleteAsync(Guid code);
}
