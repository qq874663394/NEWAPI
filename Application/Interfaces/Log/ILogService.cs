using Application.DTO.Log;
using Application.Options;

namespace Application.Interfaces.Log;

/// <summary>
/// 日志管理服务接口（只读）
/// </summary>
public interface ILogService
{
    /// <summary>分页查询日志</summary>
    Task<PagedResult<LogDto>> GetPagedListAsync(LogQueryRequest request);

    /// <summary>获取日志详情</summary>
    Task<LogDto?> GetByIdAsync(Guid code);

    /// <summary>清理指定日期之前的日志</summary>
    Task CleanAsync(DateTime beforeDate);
}
