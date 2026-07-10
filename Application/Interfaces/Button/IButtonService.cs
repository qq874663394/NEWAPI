using Application.DTO.Button;

namespace Application.Interfaces.Button;

/// <summary>
/// 按钮管理服务接口
/// </summary>
public interface IButtonService
{
    /// <summary>获取按钮详情</summary>
    Task<ButtonDto?> GetByIdAsync(Guid code);

    /// <summary>获取指定菜单下的按钮列表</summary>
    Task<List<ButtonDto>> GetByRouteAsync(Guid routeCode);

    /// <summary>创建按钮</summary>
    Task<ButtonDto> CreateAsync(CreateButtonRequest request);

    /// <summary>更新按钮</summary>
    Task<ButtonDto> UpdateAsync(UpdateButtonRequest request);

    /// <summary>删除按钮</summary>
    Task DeleteAsync(Guid code);
}
