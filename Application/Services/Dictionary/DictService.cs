using Application.DTO.Dictionary;
using Application.Interfaces.Dictionary;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.UnitOfWorks;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Dictionary;

/// <summary>
/// 字典管理服务实现，提供字典项 CRUD、树形查询及类别管理
/// </summary>
public class DictService : IDictService
{
    private readonly IRepository<SysDictionary> _dictRepo;
    private readonly IUnitOfWork _unitOfWork;

    public DictService(IRepository<SysDictionary> dictRepo, IUnitOfWork unitOfWork)
    {
        _dictRepo = dictRepo;
        _unitOfWork = unitOfWork;
    }

    /// <inheritdoc />
    public async Task<List<string>> GetTypesAsync()
    {
        return await _dictRepo.Query()
            .Where(d => !d.IsDelete && d.IsEnable)
            .Select(d => d.Type)
            .Distinct()
            .OrderBy(t => t)
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<List<DictDto>> GetByTypeAsync(string type)
    {
        var items = await _dictRepo.ToListAsync(d => d.Type == type && !d.IsDelete);

        var dtos = new List<DictDto>();
        foreach (var item in items.OrderBy(d => d.Sort))
            dtos.Add(await MapToDtoAsync(item));

        return dtos;
    }

    /// <inheritdoc />
    public async Task<List<DictDto>> GetTreeAsync(string type, Guid? parentCode = null)
    {
        var allItems = await _dictRepo.ToListAsync(d => d.Type == type && !d.IsDelete);
        if (allItems.Count == 0)
            return new List<DictDto>();

        var dtoDict = new Dictionary<Guid, DictDto>();
        foreach (var item in allItems.OrderBy(d => d.Sort))
        {
            dtoDict[item.Code] = new DictDto
            {
                Code = item.Code,
                Type = item.Type,
                KeyText = item.KeyText,
                ValueText = item.ValueText,
                Description = item.Description,
                ParentCode = item.ParentCode,
                Sort = item.Sort,
                IsEnable = item.IsEnable,
                CreateTime = item.CreateTime ?? DateTime.MinValue,
                Children = new List<DictDto>()
            };
        }

        foreach (var dto in dtoDict.Values)
        {
            if (dto.ParentCode.HasValue && dtoDict.TryGetValue(dto.ParentCode.Value, out var parent))
            {
                dto.ParentName = parent.ValueText;
                parent.Children!.Add(dto);
            }
        }

        if (parentCode.HasValue && dtoDict.TryGetValue(parentCode.Value, out var root))
            return new List<DictDto> { root };

        return dtoDict.Values.Where(d => d.ParentCode == null).ToList();
    }

    /// <inheritdoc />
    public async Task<DictDto?> GetByIdAsync(Guid code)
    {
        var item = await _dictRepo.Query()
            .FirstOrDefaultAsync(d => d.Code == code && !d.IsDelete);

        return item == null ? null : await MapToDtoAsync(item);
    }

    /// <inheritdoc />
    public async Task<DictDto> CreateAsync(CreateDictRequest request)
    {
        // 同类别下 KeyText 唯一性校验
        await ValidateKeyUniquenessAsync(request.Type, request.KeyText.Trim(), request.ParentCode, null);

        var item = new SysDictionary
        {
            Code = Guid.NewGuid(),
            Type = request.Type.Trim(),
            KeyText = request.KeyText.Trim(),
            ValueText = request.ValueText.Trim(),
            Description = request.Description,
            ParentCode = request.ParentCode,
            Sort = request.Sort,
            IsEnable = true,
            IsDelete = false,
            CreateTime = DateTime.Now
        };

        await _dictRepo.AddAsync(item);
        await _unitOfWork.CommitAsync();

        return await MapToDtoAsync(item);
    }

    /// <inheritdoc />
    public async Task<DictDto> UpdateAsync(UpdateDictRequest request)
    {
        var item = await _dictRepo.Query()
            .FirstOrDefaultAsync(d => d.Code == request.Code && !d.IsDelete)
            ?? throw new InvalidOperationException("字典项不存在");

        var keyChanged = item.KeyText != request.KeyText.Trim() || item.Type != request.Type;
        if (keyChanged)
            await ValidateKeyUniquenessAsync(request.Type, request.KeyText.Trim(), request.ParentCode, request.Code);

        item.Type = request.Type.Trim();
        item.KeyText = request.KeyText.Trim();
        item.ValueText = request.ValueText.Trim();
        item.Description = request.Description;
        item.ParentCode = request.ParentCode;
        item.Sort = request.Sort;
        item.IsEnable = request.IsEnable;
        item.ModifyTime = DateTime.Now;

        _dictRepo.Update(item);
        await _unitOfWork.CommitAsync();

        return await MapToDtoAsync(item);
    }

    /// <inheritdoc />
    public async Task DeleteAsync(Guid code)
    {
        var item = await _dictRepo.Query()
            .FirstOrDefaultAsync(d => d.Code == code && !d.IsDelete)
            ?? throw new InvalidOperationException("字典项不存在");

        var hasChildren = await _dictRepo.AnyAsync(d => d.ParentCode == code && !d.IsDelete);
        if (hasChildren)
            throw new InvalidOperationException("该字典项存在子项，无法删除");

        item.IsDelete = true;
        item.ModifyTime = DateTime.Now;
        _dictRepo.Update(item);
        await _unitOfWork.CommitAsync();
    }

    /// <summary>
    /// 将 SysDictionary 实体映射为 DictDto
    /// </summary>
    private async Task<DictDto> MapToDtoAsync(SysDictionary item)
    {
        string? parentName = null;
        if (item.ParentCode.HasValue)
        {
            var parent = await _dictRepo.FirstOrDefaultAsync(d => d.Code == item.ParentCode.Value);
            parentName = parent?.ValueText;
        }

        return new DictDto
        {
            Code = item.Code,
            Type = item.Type,
            KeyText = item.KeyText,
            ValueText = item.ValueText,
            Description = item.Description,
            ParentCode = item.ParentCode,
            ParentName = parentName,
            Sort = item.Sort,
            IsEnable = item.IsEnable,
            CreateTime = item.CreateTime ?? DateTime.MinValue,
            Children = null
        };
    }

    /// <summary>
    /// 验证同一类别下 KeyText 唯一性
    /// </summary>
    private async Task ValidateKeyUniquenessAsync(string type, string keyText, Guid? parentCode, Guid? excludeCode)
    {
        var exists = await _dictRepo.Query()
            .AnyAsync(d => d.Type == type && d.KeyText == keyText && d.ParentCode == parentCode && !d.IsDelete
                        && (!excludeCode.HasValue || d.Code != excludeCode.Value));

        if (exists)
            throw new InvalidOperationException($"字典类别 '{type}' 下已存在键为 '{keyText}' 的字典项");
    }
}
