using Application.DTO.Org;
using Application.Interfaces.Org;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.UnitOfWorks;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Org;

/// <summary>
/// 组织架构服务实现，提供组织机构的完整 CRUD、树形查询、移动及级联更新等业务功能
/// </summary>
public class OrgService : IOrgService
{
    private readonly IRepository<SysOrg> _orgRepo;
    private readonly IUnitOfWork _unitOfWork;

    public OrgService(IRepository<SysOrg> orgRepo, IUnitOfWork unitOfWork)
    {
        _orgRepo = orgRepo;
        _unitOfWork = unitOfWork;
    }

    /// <inheritdoc />
    public async Task<OrgDto?> GetByIdAsync(Guid code)
    {
        var org = await _orgRepo.Query()
            .FirstOrDefaultAsync(o => o.Code == code && !o.IsDelete);

        return org == null ? null : await MapToDtoAsync(org);
    }

    /// <inheritdoc />
    public async Task<List<OrgDto>> GetListAsync(string? keyword = null)
    {
        List<SysOrg> orgs;
        if (!string.IsNullOrWhiteSpace(keyword))
            orgs = await _orgRepo.ToListAsync(o => !o.IsDelete && o.Name.Contains(keyword));
        else
            orgs = await _orgRepo.ToListAsync(o => !o.IsDelete);

        var dtos = new List<OrgDto>();
        foreach (var org in orgs.OrderBy(o => o.Sort).ThenBy(o => o.Path))
            dtos.Add(await MapToDtoAsync(org));

        return dtos;
    }

    /// <inheritdoc />
    public async Task<List<OrgDto>> GetTreeAsync(Guid? parentCode = null)
    {
        var allOrgs = await _orgRepo.ToListAsync(o => !o.IsDelete);
        if (allOrgs.Count == 0)
            return new List<OrgDto>();

        // 构建平铺 DTO 字典，按 Sort 和 Level 排序
        var dtoDict = new Dictionary<Guid, OrgDto>();
        foreach (var org in allOrgs.OrderBy(o => o.Sort).ThenBy(o => o.Level))
        {
            dtoDict[org.Code] = new OrgDto
            {
                Code = org.Code,
                Name = org.Name,
                ParentCode = org.ParentCode,
                Path = org.Path,
                Level = org.Level,
                OrgType = org.OrgType,
                Sort = org.Sort,
                IsVirtual = org.IsVirtual == 1,
                IsEnable = org.IsEnable,
                CreateTime = org.CreateTime ?? DateTime.MinValue,
                Children = new List<OrgDto>()
            };
        }

        // 填充父级名称，并组装树形 Children 结构
        foreach (var dto in dtoDict.Values)
        {
            if (dto.ParentCode.HasValue && dtoDict.TryGetValue(dto.ParentCode.Value, out var parent))
            {
                dto.ParentName = parent.Name;
                parent.Children!.Add(dto);
            }
        }

        // 如果指定了 parentCode，返回以该节点为根的子树
        if (parentCode.HasValue && dtoDict.TryGetValue(parentCode.Value, out var root))
            return new List<OrgDto> { root };

        // 返回所有顶级根节点
        return dtoDict.Values.Where(d => d.ParentCode == null).ToList();
    }

    /// <inheritdoc />
    public async Task<List<OrgDto>> GetByParentAsync(Guid parentCode)
    {
        var orgs = await _orgRepo.ToListAsync(o => o.ParentCode == parentCode && !o.IsDelete);

        var dtos = new List<OrgDto>();
        foreach (var org in orgs.OrderBy(o => o.Sort))
            dtos.Add(await MapToDtoAsync(org));

        return dtos;
    }

    /// <inheritdoc />
    public async Task<OrgDto> CreateAsync(CreateOrgRequest request)
    {
        // 同级名称唯一性校验
        await ValidateNameUniquenessAsync(request.Name.Trim(), request.ParentCode, null);

        // 根据父级计算 Path 和 Level
        string path;
        int level;

        if (request.ParentCode.HasValue)
        {
            var parent = await _orgRepo.FirstOrDefaultAsync(o => o.Code == request.ParentCode.Value && !o.IsDelete)
                ?? throw new InvalidOperationException("父级组织不存在");

            path = $"{parent.Path}/{request.Name.Trim()}";
            level = parent.Level + 1;
        }
        else
        {
            path = "/" + request.Name.Trim();
            level = 0;
        }

        var org = new SysOrg
        {
            Code = Guid.NewGuid(),
            Name = request.Name.Trim(),
            OrgType = request.OrgType,
            ParentCode = request.ParentCode,
            Path = path,
            Level = level,
            Sort = request.Sort,
            IsVirtual = request.IsVirtual ? (ulong)1 : 0,
            IsEnable = true,
            IsDelete = false,
            CreateTime = DateTime.Now
        };

        await _orgRepo.AddAsync(org);
        await _unitOfWork.CommitAsync();

        return await MapToDtoAsync(org);
    }

    /// <inheritdoc />
    public async Task<OrgDto> UpdateAsync(UpdateOrgRequest request)
    {
        var org = await _orgRepo.Query()
            .FirstOrDefaultAsync(o => o.Code == request.Code && !o.IsDelete)
            ?? throw new InvalidOperationException("组织不存在");

        var nameChanged = org.Name != request.Name.Trim();
        if (nameChanged)
            await ValidateNameUniquenessAsync(request.Name.Trim(), org.ParentCode, request.Code);

        // 保存变更前的 Path/Level，用于后续级联更新子级
        var oldPath = org.Path;
        var oldLevel = org.Level;

        org.Name = request.Name.Trim();
        org.OrgType = request.OrgType;
        org.Sort = request.Sort;
        org.IsVirtual = request.IsVirtual ? (ulong)1 : 0;
        org.IsEnable = request.IsEnable;
        org.ModifyTime = DateTime.Now;

        // 名称变更时同步更新 Path（保持路径与名称一致）
        if (nameChanged)
        {
            var lastSlashIndex = org.Path.LastIndexOf('/');
            org.Path = lastSlashIndex > 0
                ? org.Path[..lastSlashIndex] + "/" + org.Name
                : "/" + org.Name;
        }

        _orgRepo.Update(org);

        // 名称变更时级联更新所有子级的 Path 和 Level
        if (nameChanged)
            await CascadeUpdateChildrenPathAsync(oldPath, org.Path, oldLevel, org.Level);

        await _unitOfWork.CommitAsync();

        return await MapToDtoAsync(org);
    }

    /// <inheritdoc />
    public async Task DeleteAsync(Guid code)
    {
        var org = await _orgRepo.Query()
            .FirstOrDefaultAsync(o => o.Code == code && !o.IsDelete)
            ?? throw new InvalidOperationException("组织不存在");

        // 检查是否存在下级组织
        var hasChildren = await _orgRepo.AnyAsync(o => o.ParentCode == code && !o.IsDelete);
        if (hasChildren)
            throw new InvalidOperationException("该组织存在下级组织，无法删除");

        org.IsDelete = true;
        org.ModifyTime = DateTime.Now;
        _orgRepo.Update(org);
        await _unitOfWork.CommitAsync();
    }

    /// <inheritdoc />
    public async Task MoveAsync(Guid code, Guid? newParentCode)
    {
        var org = await _orgRepo.Query()
            .FirstOrDefaultAsync(o => o.Code == code && !o.IsDelete)
            ?? throw new InvalidOperationException("组织不存在");

        // 父级未变化，无需处理
        if (org.ParentCode == newParentCode)
            return;

        var oldPath = org.Path;
        var oldLevel = org.Level;

        if (newParentCode.HasValue)
        {
            // 循环引用检测：新父级不能是自身或其子级
            await ValidateCircularReferenceAsync(code, newParentCode.Value);

            var newParent = await _orgRepo.FirstOrDefaultAsync(o => o.Code == newParentCode.Value && !o.IsDelete)
                ?? throw new InvalidOperationException("目标父级组织不存在");

            org.ParentCode = newParentCode;
            org.Path = $"{newParent.Path}/{org.Name}";
            org.Level = newParent.Level + 1;
        }
        else
        {
            // 移动到根级
            org.ParentCode = null;
            org.Path = "/" + org.Name;
            org.Level = 0;
        }

        org.ModifyTime = DateTime.Now;
        _orgRepo.Update(org);

        // 级联更新所有子级的 Path 和 Level
        await CascadeUpdateChildrenPathAsync(oldPath, org.Path, oldLevel, org.Level);
        await _unitOfWork.CommitAsync();
    }

    #region 私有辅助方法

    /// <summary>
    /// 将 SysOrg 实体映射为 OrgDto
    /// </summary>
    /// <param name="org">组织实体</param>
    /// <returns>组织 DTO</returns>
    private async Task<OrgDto> MapToDtoAsync(SysOrg org)
    {
        string? parentName = null;
        if (org.ParentCode.HasValue)
        {
            var parent = await _orgRepo.FirstOrDefaultAsync(o => o.Code == org.ParentCode.Value);
            parentName = parent?.Name;
        }

        return new OrgDto
        {
            Code = org.Code,
            Name = org.Name,
            ParentCode = org.ParentCode,
            ParentName = parentName,
            Path = org.Path,
            Level = org.Level,
            OrgType = org.OrgType,
            Sort = org.Sort,
            IsVirtual = org.IsVirtual == 1,
            IsEnable = org.IsEnable,
            CreateTime = org.CreateTime ?? DateTime.MinValue,
            Children = null
        };
    }

    /// <summary>
    /// 验证同级组织名称唯一性
    /// </summary>
    /// <param name="name">组织名称</param>
    /// <param name="parentCode">父级Code</param>
    /// <param name="excludeCode">排除的组织Code（更新时排除自身）</param>
    /// <exception cref="InvalidOperationException">名称已存在时抛出</exception>
    private async Task ValidateNameUniquenessAsync(string name, Guid? parentCode, Guid? excludeCode)
    {
        var exists = await _orgRepo.Query()
            .AnyAsync(o => o.ParentCode == parentCode && o.Name == name && !o.IsDelete
                        && (!excludeCode.HasValue || o.Code != excludeCode.Value));

        if (exists)
            throw new InvalidOperationException($"同级组织下已存在名称为 '{name}' 的组织");
    }

    /// <summary>
    /// 验证循环引用：目标父级不能是当前组织自身或其任意后代
    /// </summary>
    /// <param name="orgCode">当前组织Code</param>
    /// <param name="newParentCode">目标父级Code</param>
    /// <exception cref="InvalidOperationException">发生循环引用时抛出</exception>
    private async Task ValidateCircularReferenceAsync(Guid orgCode, Guid newParentCode)
    {
        if (orgCode == newParentCode)
            throw new InvalidOperationException("不能将组织自身设为父级");

        var allOrgs = await _orgRepo.ToListAsync(o => !o.IsDelete);
        var descendantCodes = GetAllDescendantCodes(allOrgs, orgCode);

        if (descendantCodes.Contains(newParentCode))
            throw new InvalidOperationException("不能将组织的下级设为父级，这将导致循环引用");
    }

    /// <summary>
    /// 递归获取指定组织的所有后代 Code 集合
    /// </summary>
    /// <param name="allOrgs">全量组织列表</param>
    /// <param name="parentCode">父级组织Code</param>
    /// <returns>所有后代 Code 的 HashSet</returns>
    private static HashSet<Guid> GetAllDescendantCodes(List<SysOrg> allOrgs, Guid parentCode)
    {
        var result = new HashSet<Guid>();
        var children = allOrgs.Where(o => o.ParentCode == parentCode).ToList();

        foreach (var child in children)
        {
            result.Add(child.Code);
            result.UnionWith(GetAllDescendantCodes(allOrgs, child.Code));
        }

        return result;
    }

    /// <summary>
    /// 级联更新所有子级的 Path 和 Level
    /// </summary>
    /// <param name="oldPath">旧的路径前缀</param>
    /// <param name="newPath">新的路径前缀</param>
    /// <param name="oldLevel">旧的层级</param>
    /// <param name="newLevel">新的层级</param>
    private async Task CascadeUpdateChildrenPathAsync(string oldPath, string newPath, int oldLevel, int newLevel)
    {
        var children = await _orgRepo.ToListAsync(o => o.Path.StartsWith(oldPath + "/") && !o.IsDelete);

        foreach (var child in children)
        {
            var levelDiff = child.Level - oldLevel;
            child.Path = newPath + child.Path[oldPath.Length..];
            child.Level = newLevel + levelDiff;
            child.ModifyTime = DateTime.Now;
            _orgRepo.Update(child);
        }
    }

    #endregion
}
