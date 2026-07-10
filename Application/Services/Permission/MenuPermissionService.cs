using Application.DTO.Permission;
using Application.Interfaces.Permission;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.UnitOfWorks;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Permission;

/// <summary>
/// 菜单权限服务实现，提供菜单权限的分配与查询
/// </summary>
public class MenuPermissionService : IMenuPermissionService
{
    private readonly IRepository<SysMenupermission> _menuPermRepo;
    private readonly IRepository<SysRoute> _routeRepo;
    private readonly IUnitOfWork _unitOfWork;

    public MenuPermissionService(
        IRepository<SysMenupermission> menuPermRepo,
        IRepository<SysRoute> routeRepo,
        IUnitOfWork unitOfWork)
    {
        _menuPermRepo = menuPermRepo;
        _routeRepo = routeRepo;
        _unitOfWork = unitOfWork;
    }

    /// <inheritdoc />
    public async Task<List<MenuPermissionDto>> GetListAsync(Guid? routeCode = null, string? subjectType = null, Guid? subjectCode = null)
    {
        var query = _menuPermRepo.Query().Where(p => !p.IsDelete);

        if (routeCode.HasValue)
            query = query.Where(p => p.RouteCode == routeCode.Value);
        if (!string.IsNullOrWhiteSpace(subjectType))
            query = query.Where(p => p.SubjectType == subjectType);
        if (subjectCode.HasValue)
            query = query.Where(p => p.SubjectCode == subjectCode.Value);

        var permissions = await query.ToListAsync();
        var dtos = new List<MenuPermissionDto>();

        foreach (var p in permissions)
        {
            string? routeName = null;
            var route = await _routeRepo.FirstOrDefaultAsync(r => r.Code == p.RouteCode);
            routeName = route?.Name;

            dtos.Add(new MenuPermissionDto
            {
                Code = p.Code,
                RouteCode = p.RouteCode,
                RouteName = routeName,
                SubjectType = p.SubjectType,
                SubjectCode = p.SubjectCode,
                IsGranted = p.IsGranted == 1,
                CreateTime = p.CreateTime ?? DateTime.MinValue
            });
        }

        return dtos;
    }

    /// <inheritdoc />
    public async Task<List<MenuPermissionDto>> GetGrantedMenusAsync(string subjectType, Guid subjectCode)
    {
        var permissions = await _menuPermRepo.ToListAsync(p =>
            p.SubjectType == subjectType && p.SubjectCode == subjectCode && !p.IsDelete && p.IsGranted == 1);

        var dtos = new List<MenuPermissionDto>();
        foreach (var p in permissions)
        {
            string? routeName = null;
            var route = await _routeRepo.FirstOrDefaultAsync(r => r.Code == p.RouteCode);
            routeName = route?.Name;

            dtos.Add(new MenuPermissionDto
            {
                Code = p.Code,
                RouteCode = p.RouteCode,
                RouteName = routeName,
                SubjectType = p.SubjectType,
                SubjectCode = p.SubjectCode,
                IsGranted = true,
                CreateTime = p.CreateTime ?? DateTime.MinValue
            });
        }

        return dtos;
    }

    /// <inheritdoc />
    public async Task<MenuPermissionDto> AssignAsync(AssignMenuPermissionRequest request)
    {
        var existing = await _menuPermRepo.Query()
            .FirstOrDefaultAsync(p => p.RouteCode == request.RouteCode
                                   && p.SubjectType == request.SubjectType
                                   && p.SubjectCode == request.SubjectCode
                                   && !p.IsDelete);

        if (existing != null)
        {
            existing.IsGranted = request.IsGranted ? (ulong)1 : 0;
            existing.ModifyTime = DateTime.Now;
            _menuPermRepo.Update(existing);
            await _unitOfWork.CommitAsync();
            return await MapToDtoAsync(existing);
        }

        var permission = new SysMenupermission
        {
            Code = Guid.NewGuid(),
            RouteCode = request.RouteCode,
            SubjectType = request.SubjectType,
            SubjectCode = request.SubjectCode,
            IsGranted = request.IsGranted ? (ulong)1 : 0,
            IsEnable = true,
            IsDelete = false,
            CreateTime = DateTime.Now
        };

        await _menuPermRepo.AddAsync(permission);
        await _unitOfWork.CommitAsync();

        return await MapToDtoAsync(permission);
    }

    /// <inheritdoc />
    public async Task BatchAssignAsync(BatchAssignMenuPermissionRequest request)
    {
        // 逐个授权
        foreach (var routeCode in request.GrantRouteCodes)
        {
            var existing = await _menuPermRepo.Query()
                .FirstOrDefaultAsync(p => p.RouteCode == routeCode
                                       && p.SubjectType == request.SubjectType
                                       && p.SubjectCode == request.SubjectCode
                                       && !p.IsDelete);
            if (existing != null)
            {
                existing.IsGranted = 1;
                existing.ModifyTime = DateTime.Now;
                _menuPermRepo.Update(existing);
            }
            else
            {
                await _menuPermRepo.AddAsync(new SysMenupermission
                {
                    Code = Guid.NewGuid(),
                    RouteCode = routeCode,
                    SubjectType = request.SubjectType,
                    SubjectCode = request.SubjectCode,
                    IsGranted = 1,
                    IsEnable = true,
                    IsDelete = false,
                    CreateTime = DateTime.Now
                });
            }
        }

        // 逐个取消授权
        foreach (var routeCode in request.RevokeRouteCodes)
        {
            var existing = await _menuPermRepo.Query()
                .FirstOrDefaultAsync(p => p.RouteCode == routeCode
                                       && p.SubjectType == request.SubjectType
                                       && p.SubjectCode == request.SubjectCode
                                       && !p.IsDelete);
            if (existing != null)
            {
                existing.IsGranted = 0;
                existing.ModifyTime = DateTime.Now;
                _menuPermRepo.Update(existing);
            }
        }

        await _unitOfWork.CommitAsync();
    }

    /// <inheritdoc />
    public async Task RemoveBySubjectAsync(string subjectType, Guid subjectCode)
    {
        var permissions = await _menuPermRepo.ToListAsync(p =>
            p.SubjectType == subjectType && p.SubjectCode == subjectCode && !p.IsDelete);

        foreach (var p in permissions)
        {
            p.IsDelete = true;
            p.ModifyTime = DateTime.Now;
            _menuPermRepo.Update(p);
        }

        await _unitOfWork.CommitAsync();
    }

    private async Task<MenuPermissionDto> MapToDtoAsync(SysMenupermission permission)
    {
        string? routeName = null;
        var route = await _routeRepo.FirstOrDefaultAsync(r => r.Code == permission.RouteCode);
        routeName = route?.Name;

        return new MenuPermissionDto
        {
            Code = permission.Code,
            RouteCode = permission.RouteCode,
            RouteName = routeName,
            SubjectType = permission.SubjectType,
            SubjectCode = permission.SubjectCode,
            IsGranted = permission.IsGranted == 1,
            CreateTime = permission.CreateTime ?? DateTime.MinValue
        };
    }
}
