namespace AuthApplication.Common;

/// <summary>
/// 分页返回
/// </summary>
/// <typeparam name="T"></typeparam>
public class PageResult<T>
{
    /// <summary>
    /// 数据
    /// </summary>
    public IEnumerable<T> Items { get; set; } = Enumerable.Empty<T>();

    /// <summary>
    /// 总数量
    /// </summary>
    public long TotalCount { get; set; }

    /// <summary>
    /// 当前页
    /// </summary>
    public int PageIndex { get; set; }

    /// <summary>
    /// 每页数量
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// 总页数
    /// </summary>
    public int TotalPages =>
        PageSize == 0
            ? 0
            : (int)Math.Ceiling((double)TotalCount / PageSize);

    public PageResult()
    {
    }

    public PageResult(
        IEnumerable<T> items,
        long totalCount,
        int pageIndex,
        int pageSize)
    {
        Items = items;
        TotalCount = totalCount;
        PageIndex = pageIndex;
        PageSize = pageSize;
    }
}