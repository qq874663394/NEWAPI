using Application.DTO.Route;
using Application.Interfaces.Route;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.UnitOfWorks;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Route;

/// <summary>
/// 菜单管理服务实现，提供菜单 CRUD 及树形查询
/// </summary>
public class RouteService : IRouteService
{
    private readonly IRepository<SysRoute> _routeRepo;
    private readonly IUnitOfWork _unitOfWork;

    public RouteService(IRepository<SysRoute> routeRepo, IUnitOfWork unitOfWork)
    {
        _routeRepo = routeRepo;
        _unitOfWork = unitOfWork;
    }

    /// <inheritdoc />
    public async Task<RouteDto?> GetByIdAsync(Guid code)
    {
        var route = await _routeRepo.Query()
            .FirstOrDefaultAsync(r => r.Code == code && !r.IsDelete);

        return route == null ? null : await MapToDtoAsync(route);
    }

    /// <inheritdoc />
    public async Task<List<RouteDto>> GetListAsync(string? keyword = null)
    {
        List<SysRoute> routes;
        if (!string.IsNullOrWhiteSpace(keyword))
            routes = await _routeRepo.ToListAsync(r => !r.IsDelete && r.Name.Contains(keyword));
        else
            routes = await _routeRepo.ToListAsync(r => !r.IsDelete);

        var dtos = new List<RouteDto>();
        foreach (var route in routes.OrderBy(r => r.Sort))
            dtos.Add(await MapToDtoAsync(route));

        return dtos;
    }

    /// <inheritdoc />
    public async Task<List<RouteDto>> GetTreeAsync(Guid? parentCode = null)
    {
        var allRoutes = await _routeRepo.ToListAsync(r => !r.IsDelete);
        if (allRoutes.Count == 0)
            return new List<RouteDto>();

        var dtoDict = new Dictionary<Guid, RouteDto>();
        foreach (var route in allRoutes.OrderBy(r => r.Sort))
        {
            dtoDict[route.Code] = new RouteDto
            {
                Code = route.Code,
                Name = route.Name,
                Path = route.Path,
                Component = route.Component,
                Redirect = route.Redirect,
                ParentCode = route.ParentCode,
                MetaTitle = route.MetaTitle,
                MetaIcon = route.MetaIcon,
                Hidden = route.Hidden == 1,
                AlwaysShow = route.AlwaysShow == 1,
                MetaNoCache = route.MetaNoCache == 1,
                MetaAffix = route.MetaAffix == 1,
                MetaActiveMenu = route.MetaActiveMenu,
                Sort = route.Sort,
                IsEnable = route.IsEnable,
                CreateTime = route.CreateTime ?? DateTime.MinValue,
                Children = new List<RouteDto>()
            };
        }

        foreach (var dto in dtoDict.Values)
        {
            if (dto.ParentCode.HasValue && dtoDict.TryGetValue(dto.ParentCode.Value, out var parent))
            {
                dto.ParentName = parent.Name;
                parent.Children!.Add(dto);
            }
        }

        if (parentCode.HasValue && dtoDict.TryGetValue(parentCode.Value, out var root))
            return new List<RouteDto> { root };

        return dtoDict.Values.Where(d => d.ParentCode == null).ToList();
    }

    /// <inheritdoc />
    public async Task<RouteDto> CreateAsync(CreateRouteRequest request)
    {
        await ValidateNameUniquenessAsync(request.Name.Trim(), request.ParentCode, null);

        var route = new SysRoute
        {
            Code = Guid.NewGuid(),
            Name = request.Name.Trim(),
            Path = request.Path.Trim(),
            Component = request.Component,
            Redirect = request.Redirect,
            ParentCode = request.ParentCode,
            MetaTitle = request.MetaTitle,
            MetaIcon = request.MetaIcon,
            Hidden = request.Hidden ? (ulong)1 : 0,
            AlwaysShow = request.AlwaysShow ? (ulong)1 : 0,
            MetaNoCache = request.MetaNoCache ? (ulong)1 : 0,
            MetaAffix = request.MetaAffix ? (ulong)1 : 0,
            MetaActiveMenu = request.MetaActiveMenu,
            Sort = request.Sort,
            IsEnable = true,
            IsDelete = false,
            CreateTime = DateTime.Now
        };

        await _routeRepo.AddAsync(route);
        await _unitOfWork.CommitAsync();

        return await MapToDtoAsync(route);
    }

    /// <inheritdoc />
    public async Task<RouteDto> UpdateAsync(UpdateRouteRequest request)
    {
        var route = await _routeRepo.Query()
            .FirstOrDefaultAsync(r => r.Code == request.Code && !r.IsDelete)
            ?? throw new InvalidOperationException("菜单不存在");

        var nameChanged = route.Name != request.Name.Trim();
        if (nameChanged)
            await ValidateNameUniquenessAsync(request.Name.Trim(), route.ParentCode, request.Code);

        route.Name = request.Name.Trim();
        route.Path = request.Path.Trim();
        route.Component = request.Component;
        route.Redirect = request.Redirect;
        route.ParentCode = request.ParentCode;
        route.MetaTitle = request.MetaTitle;
        route.MetaIcon = request.MetaIcon;
        route.Hidden = request.Hidden ? (ulong)1 : 0;
        route.AlwaysShow = request.AlwaysShow ? (ulong)1 : 0;
        route.MetaNoCache = request.MetaNoCache ? (ulong)1 : 0;
        route.MetaAffix = request.MetaAffix ? (ulong)1 : 0;
        route.MetaActiveMenu = request.MetaActiveMenu;
        route.Sort = request.Sort;
        route.IsEnable = request.IsEnable;
        route.ModifyTime = DateTime.Now;

        _routeRepo.Update(route);
        await _unitOfWork.CommitAsync();

        return await MapToDtoAsync(route);
    }

    /// <inheritdoc />
    public async Task DeleteAsync(Guid code)
    {
        var route = await _routeRepo.Query()
            .FirstOrDefaultAsync(r => r.Code == code && !r.IsDelete)
            ?? throw new InvalidOperationException("菜单不存在");

        var hasChildren = await _routeRepo.AnyAsync(r => r.ParentCode == code && !r.IsDelete);
        if (hasChildren)
            throw new InvalidOperationException("该菜单存在下级菜单，无法删除");

        route.IsDelete = true;
        route.ModifyTime = DateTime.Now;
        _routeRepo.Update(route);
        await _unitOfWork.CommitAsync();
    }

    /// <summary>
    /// 将 SysRoute 实体映射为 RouteDto
    /// </summary>
    private async Task<RouteDto> MapToDtoAsync(SysRoute route)
    {
        string? parentName = null;
        if (route.ParentCode.HasValue)
        {
            var parent = await _routeRepo.FirstOrDefaultAsync(r => r.Code == route.ParentCode.Value);
            parentName = parent?.Name;
        }

        return new RouteDto
        {
            Code = route.Code,
            Name = route.Name,
            Path = route.Path,
            Component = route.Component,
            Redirect = route.Redirect,
            ParentCode = route.ParentCode,
            ParentName = parentName,
            MetaTitle = route.MetaTitle,
            MetaIcon = route.MetaIcon,
            Hidden = route.Hidden == 1,
            AlwaysShow = route.AlwaysShow == 1,
            MetaNoCache = route.MetaNoCache == 1,
            MetaAffix = route.MetaAffix == 1,
            MetaActiveMenu = route.MetaActiveMenu,
            Sort = route.Sort,
            IsEnable = route.IsEnable,
            CreateTime = route.CreateTime ?? DateTime.MinValue,
            Children = null
        };
    }

    /// <summary>
    /// 验证同级菜单名称唯一性
    /// </summary>
    private async Task ValidateNameUniquenessAsync(string name, Guid? parentCode, Guid? excludeCode)
    {
        var exists = await _routeRepo.Query()
            .AnyAsync(r => r.ParentCode == parentCode && r.Name == name && !r.IsDelete
                        && (!excludeCode.HasValue || r.Code != excludeCode.Value));

        if (exists)
            throw new InvalidOperationException($"同级菜单下已存在名称为 '{name}' 的菜单");
    }
}
