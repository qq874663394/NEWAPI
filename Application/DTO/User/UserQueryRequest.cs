namespace Application.DTO.User;

/// <summary>
/// 用户分页查询请求
/// </summary>
public class UserQueryRequest
{
    /// <summary>关键字（模糊匹配 Name / FullName / Email）</summary>
    public string? Keyword { get; set; }

    /// <summary>
    /// 按组织 Code 筛选。
    /// 通过 SysOrg.Path 前缀匹配，会同时查出该组织及其所有子组织的用户
    /// </summary>
    public Guid? OrgCode { get; set; }

    /// <summary>是否仅查询活跃用户</summary>
    public bool? IsActive { get; set; }

    /// <summary>当前页，默认 1</summary>
    public int PageIndex { get; set; } = 1;

    /// <summary>每页数量，默认 20</summary>
    public int PageSize { get; set; } = 20;
}
