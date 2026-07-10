namespace Application.Options;

/// <summary>
/// 分页返回结果（Application 层内部使用，避免跨层引用 AuthApplication）
/// </summary>
/// <typeparam name="T">数据项类型</typeparam>
public class PagedResult<T>
{
    /// <summary>数据列表</summary>
    public IEnumerable<T> Items { get; set; } = Enumerable.Empty<T>();

    /// <summary>总记录数</summary>
    public long TotalCount { get; set; }

    /// <summary>当前页码</summary>
    public int PageIndex { get; set; }

    /// <summary>每页数量</summary>
    public int PageSize { get; set; }

    /// <summary>总页数</summary>
    public int TotalPages =>
        PageSize == 0
            ? 0
            : (int)Math.Ceiling((double)TotalCount / PageSize);

    public PagedResult() { }

    public PagedResult(IEnumerable<T> items, long totalCount, int pageIndex, int pageSize)
    {
        Items = items;
        TotalCount = totalCount;
        PageIndex = pageIndex;
        PageSize = pageSize;
    }
}
