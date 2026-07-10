using Application.DTO.Dictionary;

namespace Application.Interfaces.Dictionary;

/// <summary>
/// 字典管理服务接口
/// </summary>
public interface IDictService
{
    /// <summary>获取所有字典类别列表</summary>
    Task<List<string>> GetTypesAsync();

    /// <summary>按类别获取字典项平铺列表</summary>
    Task<List<DictDto>> GetByTypeAsync(string type);

    /// <summary>按类别获取字典树</summary>
    Task<List<DictDto>> GetTreeAsync(string type, Guid? parentCode = null);

    /// <summary>获取字典项详情</summary>
    Task<DictDto?> GetByIdAsync(Guid code);

    /// <summary>创建字典项</summary>
    Task<DictDto> CreateAsync(CreateDictRequest request);

    /// <summary>更新字典项</summary>
    Task<DictDto> UpdateAsync(UpdateDictRequest request);

    /// <summary>删除字典项（需无子级）</summary>
    Task DeleteAsync(Guid code);
}
