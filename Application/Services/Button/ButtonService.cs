using Application.DTO.Button;
using Application.Interfaces.Button;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.UnitOfWorks;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Button;

/// <summary>
/// 按钮管理服务实现，提供按钮 CRUD
/// </summary>
public class ButtonService : IButtonService
{
    private readonly IRepository<SysButton> _buttonRepo;
    private readonly IRepository<SysRoute> _routeRepo;
    private readonly IUnitOfWork _unitOfWork;

    public ButtonService(
        IRepository<SysButton> buttonRepo,
        IRepository<SysRoute> routeRepo,
        IUnitOfWork unitOfWork)
    {
        _buttonRepo = buttonRepo;
        _routeRepo = routeRepo;
        _unitOfWork = unitOfWork;
    }

    /// <inheritdoc />
    public async Task<ButtonDto?> GetByIdAsync(Guid code)
    {
        var button = await _buttonRepo.Query()
            .FirstOrDefaultAsync(b => b.Code == code && !b.IsDelete);

        return button == null ? null : await MapToDtoAsync(button);
    }

    /// <inheritdoc />
    public async Task<List<ButtonDto>> GetByRouteAsync(Guid routeCode)
    {
        var buttons = await _buttonRepo.ToListAsync(b => b.RouteCode == routeCode && !b.IsDelete);

        var dtos = new List<ButtonDto>();
        foreach (var button in buttons.OrderBy(b => b.Sort))
            dtos.Add(await MapToDtoAsync(button));

        return dtos;
    }

    /// <inheritdoc />
    public async Task<ButtonDto> CreateAsync(CreateButtonRequest request)
    {
        // 校验所属菜单存在
        var routeExists = await _routeRepo.AnyAsync(r => r.Code == request.RouteCode && !r.IsDelete);
        if (!routeExists)
            throw new InvalidOperationException("所属菜单不存在");

        // 同级按钮 ButtonKey 唯一性校验
        var keyExists = await _buttonRepo.AnyAsync(b =>
            b.RouteCode == request.RouteCode && b.ButtonKey == request.ButtonKey && !b.IsDelete);
        if (keyExists)
            throw new InvalidOperationException($"该菜单下已存在按钮Key为 '{request.ButtonKey}' 的按钮");

        var button = new SysButton
        {
            Code = Guid.NewGuid(),
            ButtonKey = request.ButtonKey.Trim(),
            Name = request.Name.Trim(),
            RouteCode = request.RouteCode,
            Event = request.Event.Trim(),
            StyleType = request.StyleType,
            Type = request.Type,
            Icon = request.Icon,
            Sort = request.Sort,
            IsSystemButton = request.IsSystemButton ? (ulong)1 : 0,
            IsEnable = true,
            IsDelete = false,
            CreateTime = DateTime.Now
        };

        await _buttonRepo.AddAsync(button);
        await _unitOfWork.CommitAsync();

        return await MapToDtoAsync(button);
    }

    /// <inheritdoc />
    public async Task<ButtonDto> UpdateAsync(UpdateButtonRequest request)
    {
        var button = await _buttonRepo.Query()
            .FirstOrDefaultAsync(b => b.Code == request.Code && !b.IsDelete)
            ?? throw new InvalidOperationException("按钮不存在");

        // 如果 RouteCode 或 ButtonKey 变更，校验唯一性
        if (button.RouteCode != request.RouteCode || button.ButtonKey != request.ButtonKey.Trim())
        {
            var keyExists = await _buttonRepo.AnyAsync(b =>
                b.RouteCode == request.RouteCode && b.ButtonKey == request.ButtonKey.Trim()
                && b.Code != request.Code && !b.IsDelete);
            if (keyExists)
                throw new InvalidOperationException($"该菜单下已存在按钮Key为 '{request.ButtonKey}' 的按钮");

            // 新菜单存在性校验
            if (button.RouteCode != request.RouteCode)
            {
                var routeExists = await _routeRepo.AnyAsync(r => r.Code == request.RouteCode && !r.IsDelete);
                if (!routeExists)
                    throw new InvalidOperationException("所属菜单不存在");
            }
        }

        button.ButtonKey = request.ButtonKey.Trim();
        button.Name = request.Name.Trim();
        button.RouteCode = request.RouteCode;
        button.Event = request.Event.Trim();
        button.StyleType = request.StyleType;
        button.Type = request.Type;
        button.Icon = request.Icon;
        button.Sort = request.Sort;
        button.IsSystemButton = request.IsSystemButton ? (ulong)1 : 0;
        button.IsEnable = request.IsEnable;
        button.ModifyTime = DateTime.Now;

        _buttonRepo.Update(button);
        await _unitOfWork.CommitAsync();

        return await MapToDtoAsync(button);
    }

    /// <inheritdoc />
    public async Task DeleteAsync(Guid code)
    {
        var button = await _buttonRepo.Query()
            .FirstOrDefaultAsync(b => b.Code == code && !b.IsDelete)
            ?? throw new InvalidOperationException("按钮不存在");

        // 系统内置按钮不允许删除
        if (button.IsSystemButton == 1)
            throw new InvalidOperationException("系统内置按钮不允许删除");

        button.IsDelete = true;
        button.ModifyTime = DateTime.Now;
        _buttonRepo.Update(button);
        await _unitOfWork.CommitAsync();
    }

    /// <summary>
    /// 将 SysButton 实体映射为 ButtonDto
    /// </summary>
    private async Task<ButtonDto> MapToDtoAsync(SysButton button)
    {
        string? routeName = null;
        var route = await _routeRepo.FirstOrDefaultAsync(r => r.Code == button.RouteCode);
        routeName = route?.Name;

        return new ButtonDto
        {
            Code = button.Code,
            ButtonKey = button.ButtonKey,
            Name = button.Name,
            RouteCode = button.RouteCode,
            RouteName = routeName,
            Event = button.Event,
            StyleType = button.StyleType,
            Type = button.Type,
            Icon = button.Icon,
            Sort = button.Sort,
            IsSystemButton = button.IsSystemButton == 1,
            IsEnable = button.IsEnable,
            CreateTime = button.CreateTime ?? DateTime.MinValue
        };
    }
}
