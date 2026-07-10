using Application.DTO.Role;

namespace Application.Interfaces.Role;

public interface IRoleService
{
    Task<RoleDto?> GetByIdAsync(Guid code);
    Task<List<RoleDto>> GetListAsync(string? keyword = null);
    Task<RoleDto> CreateAsync(CreateRoleRequest request);
    Task<RoleDto> UpdateAsync(UpdateRoleRequest request);
    Task DeleteAsync(Guid code);
    Task<List<RoleDto>> GetTreeAsync();
}
