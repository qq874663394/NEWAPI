using Application.DTO.Permission;
using Application.Interfaces.Permission;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.UnitOfWorks;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Permission;

/// <summary>
/// 按钮权限服务实现，提供按钮权限的分配与查询
/// </summary>
public class ButtonPermissionService : IButtonPermissionService
{
    private readonly IRepository<SysButtonpermission> _btnPermRepo;
    private readonly IRepository<SysButton> _buttonRepo;
    private readonly IUnitOfWork _unitOfWork;

    public ButtonPermissionService(
        IRepository<SysButtonpermission> btnPermRepo,
        IRepository<SysButton> buttonRepo,
        IUnitOfWork unitOfWork)
    {
        _btnPermRepo = btnPermRepo;
        _buttonRepo = buttonRepo;
        _unitOfWork = unitOfWork;
    }

    /// <inheritdoc />
    public async Task<List<ButtonPermissionDto>> GetListAsync(Guid? buttonCode = null, string? subjectType = null, Guid? subjectCode = null)
    {
        var query = _btnPermRepo.Query().Where(p => !p.IsDelete);

        if (buttonCode.HasValue)
            query = query.Where(p => p.ButtonCode == buttonCode.Value);
        if (!string.IsNullOrWhiteSpace(subjectType))
            query = query.Where(p => p.SubjectType == subjectType);
        if (subjectCode.HasValue)
            query = query.Where(p => p.SubjectCode == subjectCode.Value);

        var permissions = await query.ToListAsync();
        var dtos = new List<ButtonPermissionDto>();

        foreach (var p in permissions)
        {
            string? btnName = null;
            Guid? routeCode = null;
            var button = await _buttonRepo.FirstOrDefaultAsync(b => b.Code == p.ButtonCode);
            if (button != null)
            {
                btnName = button.Name;
                routeCode = button.RouteCode;
            }

            dtos.Add(new ButtonPermissionDto
            {
                Code = p.Code,
                ButtonCode = p.ButtonCode,
                ButtonName = btnName,
                RouteCode = routeCode,
                SubjectType = p.SubjectType,
                SubjectCode = p.SubjectCode,
                IsGranted = p.IsGranted == 1,
                CreateTime = p.CreateTime ?? DateTime.MinValue
            });
        }

        return dtos;
    }

    /// <inheritdoc />
    public async Task<List<Guid>> GetGrantedButtonCodesAsync(string subjectType, Guid subjectCode)
    {
        return await _btnPermRepo.Query()
            .Where(p => p.SubjectType == subjectType && p.SubjectCode == subjectCode && !p.IsDelete && p.IsGranted == 1)
            .Select(p => p.ButtonCode)
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<ButtonPermissionDto> AssignAsync(AssignButtonPermissionRequest request)
    {
        var existing = await _btnPermRepo.Query()
            .FirstOrDefaultAsync(p => p.ButtonCode == request.ButtonCode
                                   && p.SubjectType == request.SubjectType
                                   && p.SubjectCode == request.SubjectCode
                                   && !p.IsDelete);

        if (existing != null)
        {
            existing.IsGranted = request.IsGranted ? (ulong)1 : 0;
            existing.ModifyTime = DateTime.Now;
            _btnPermRepo.Update(existing);
            await _unitOfWork.CommitAsync();
            return await MapToDtoAsync(existing);
        }

        var permission = new SysButtonpermission
        {
            Code = Guid.NewGuid(),
            ButtonCode = request.ButtonCode,
            SubjectType = request.SubjectType,
            SubjectCode = request.SubjectCode,
            IsGranted = request.IsGranted ? (ulong)1 : 0,
            IsEnable = true,
            IsDelete = false,
            CreateTime = DateTime.Now
        };

        await _btnPermRepo.AddAsync(permission);
        await _unitOfWork.CommitAsync();

        return await MapToDtoAsync(permission);
    }

    /// <inheritdoc />
    public async Task BatchAssignAsync(BatchAssignButtonPermissionRequest request)
    {
        foreach (var btnCode in request.GrantButtonCodes)
        {
            var existing = await _btnPermRepo.Query()
                .FirstOrDefaultAsync(p => p.ButtonCode == btnCode
                                       && p.SubjectType == request.SubjectType
                                       && p.SubjectCode == request.SubjectCode
                                       && !p.IsDelete);
            if (existing != null)
            {
                existing.IsGranted = 1;
                existing.ModifyTime = DateTime.Now;
                _btnPermRepo.Update(existing);
            }
            else
            {
                await _btnPermRepo.AddAsync(new SysButtonpermission
                {
                    Code = Guid.NewGuid(),
                    ButtonCode = btnCode,
                    SubjectType = request.SubjectType,
                    SubjectCode = request.SubjectCode,
                    IsGranted = 1,
                    IsEnable = true,
                    IsDelete = false,
                    CreateTime = DateTime.Now
                });
            }
        }

        foreach (var btnCode in request.RevokeButtonCodes)
        {
            var existing = await _btnPermRepo.Query()
                .FirstOrDefaultAsync(p => p.ButtonCode == btnCode
                                       && p.SubjectType == request.SubjectType
                                       && p.SubjectCode == request.SubjectCode
                                       && !p.IsDelete);
            if (existing != null)
            {
                existing.IsGranted = 0;
                existing.ModifyTime = DateTime.Now;
                _btnPermRepo.Update(existing);
            }
        }

        await _unitOfWork.CommitAsync();
    }

    private async Task<ButtonPermissionDto> MapToDtoAsync(SysButtonpermission permission)
    {
        string? btnName = null;
        Guid? routeCode = null;
        var button = await _buttonRepo.FirstOrDefaultAsync(b => b.Code == permission.ButtonCode);
        if (button != null)
        {
            btnName = button.Name;
            routeCode = button.RouteCode;
        }

        return new ButtonPermissionDto
        {
            Code = permission.Code,
            ButtonCode = permission.ButtonCode,
            ButtonName = btnName,
            RouteCode = routeCode,
            SubjectType = permission.SubjectType,
            SubjectCode = permission.SubjectCode,
            IsGranted = permission.IsGranted == 1,
            CreateTime = permission.CreateTime ?? DateTime.MinValue
        };
    }
}
