using Application.DTO.Route;

namespace Application.Interfaces.Route;

/// <summary>
/// 菜单管理服务接口
/// </summary>
public interface IRouteService
{
    /// <summary>获取菜单详情</summary>
    Task<RouteDto?> GetByIdAsync(Guid code);

    /// <summary>获取菜单列表（支持关键字搜索）</summary>
    Task<List<RouteDto>> GetListAsync(string? keyword = null);

    /// <summary>获取菜单树（可选指定根节点）</summary>
    Task<List<RouteDto>> GetTreeAsync(Guid? parentCode = null);

    /// <summary>创建菜单</summary>
    Task<RouteDto> CreateAsync(CreateRouteRequest request);

    /// <summary>更新菜单</summary>
    Task<RouteDto> UpdateAsync(UpdateRouteRequest request);

    /// <summary>删除菜单（需无子级）</summary>
    Task DeleteAsync(Guid code);
}
