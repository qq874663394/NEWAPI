using Application.DTO.Role;
using Application.Interfaces.Role;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.UnitOfWorks;

namespace Application.Services.Role;

public class RoleService : IRoleService
{
    private readonly IRepository<SysRole> _roleRepo;
    private readonly IUnitOfWork _unitOfWork;

    public RoleService(IRepository<SysRole> roleRepo, IUnitOfWork unitOfWork)
    {
        _roleRepo = roleRepo;
        _unitOfWork = unitOfWork;
    }

    public async Task<RoleDto?> GetByIdAsync(Guid code)
    {
        var role = await _roleRepo.FirstOrDefaultAsync(r => r.Code == code && !r.IsDelete);
        return role == null ? null : await MapToDtoAsync(role);
    }

    public async Task<List<RoleDto>> GetListAsync(string? keyword = null)
    {
        List<SysRole> roles;
        if (!string.IsNullOrWhiteSpace(keyword))
            roles = await _roleRepo.ToListAsync(r => !r.IsDelete && r.Name.Contains(keyword));
        else
            roles = await _roleRepo.ToListAsync(r => !r.IsDelete);

        var result = new List<RoleDto>();
        foreach (var r in roles)
            result.Add(await MapToDtoAsync(r));
        return result;
    }

    public async Task<List<RoleDto>> GetTreeAsync()
    {
        var roles = await _roleRepo.ToListAsync(r => !r.IsDelete);
        var dtos = new List<RoleDto>();
        foreach (var r in roles)
            dtos.Add(await MapToDtoAsync(r));
        return dtos.Where(r => r.SuperiorRoleCode == null).ToList();
    }

    public async Task<RoleDto> CreateAsync(CreateRoleRequest request)
    {
        var exists = await _roleRepo.AnyAsync(r => r.Name == request.Name && !r.IsDelete);
        if (exists) throw new InvalidOperationException($"角色名 '{request.Name}' 已存在");

        var role = new SysRole
        {
            Code = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description ?? string.Empty,
            Level = request.Level,
            SuperiorRoleCode = request.SuperiorRoleCode,
            IsEnable = true,
            IsDelete = false,
            CreateTime = DateTime.Now
        };
        await _roleRepo.AddAsync(role);
        await _unitOfWork.CommitAsync();
        return await MapToDtoAsync(role);
    }

    public async Task<RoleDto> UpdateAsync(UpdateRoleRequest request)
    {
        var role = await _roleRepo.FirstOrDefaultAsync(r => r.Code == request.Code && !r.IsDelete)
            ?? throw new InvalidOperationException("角色不存在");

        var duplicate = await _roleRepo.AnyAsync(r => r.Name == request.Name && r.Code != request.Code && !r.IsDelete);
        if (duplicate) throw new InvalidOperationException($"角色名 '{request.Name}' 已被使用");

        role.Name = request.Name;
        role.Description = request.Description ?? string.Empty;
        role.Level = request.Level;
        role.SuperiorRoleCode = request.SuperiorRoleCode;
        role.ModifyTime = DateTime.Now;
        _roleRepo.Update(role);
        await _unitOfWork.CommitAsync();
        return await MapToDtoAsync(role);
    }

    public async Task DeleteAsync(Guid code)
    {
        var role = await _roleRepo.FirstOrDefaultAsync(r => r.Code == code && !r.IsDelete)
            ?? throw new InvalidOperationException("角色不存在");

        var hasInferior = await _roleRepo.AnyAsync(r => r.SuperiorRoleCode == code && !r.IsDelete);
        if (hasInferior) throw new InvalidOperationException("该角色存在下级角色，无法删除");

        role.IsDelete = true;
        role.ModifyTime = DateTime.Now;
        _roleRepo.Update(role);
        await _unitOfWork.CommitAsync();
    }

    private async Task<RoleDto> MapToDtoAsync(SysRole role)
    {
        string? superiorName = null;
        if (role.SuperiorRoleCode.HasValue)
        {
            var superior = await _roleRepo.FirstOrDefaultAsync(r => r.Code == role.SuperiorRoleCode.Value);
            superiorName = superior?.Name;
        }
        return new RoleDto
        {
            Code = role.Code,
            Name = role.Name,
            Description = role.Description,
            Level = role.Level,
            SuperiorRoleCode = role.SuperiorRoleCode,
            SuperiorRoleName = superiorName,
            IsEnable = role.IsEnable,
            CreateTime = role.CreateTime
        };
    }
}
