using Application.DTO.ReportLine;
using Application.Options;

namespace Application.Interfaces.ReportLine;

/// <summary>
/// 汇报关系服务接口
/// </summary>
public interface IReportLineService
{
    /// <summary>分页查询汇报关系</summary>
    Task<PagedResult<ReportLineDto>> GetPagedListAsync(ReportLineQueryRequest request);

    /// <summary>获取汇报关系详情</summary>
    Task<ReportLineDto?> GetByIdAsync(Guid code);

    /// <summary>创建汇报关系</summary>
    Task<ReportLineDto> CreateAsync(CreateReportLineRequest request);

    /// <summary>删除汇报关系</summary>
    Task DeleteAsync(Guid code);

    /// <summary>设置启用/禁用</summary>
    Task SetActiveAsync(Guid code, bool isActive);
}
