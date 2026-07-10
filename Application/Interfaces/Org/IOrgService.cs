using Application.DTO.Org;

namespace Application.Interfaces.Org;

public interface IOrgService
{
    Task<OrgDto?> GetByIdAsync(Guid code);
    Task<List<OrgDto>> GetListAsync(string? keyword = null);
    Task<List<OrgDto>> GetTreeAsync(Guid? parentCode = null);
    Task<List<OrgDto>> GetByParentAsync(Guid parentCode);
    Task<OrgDto> CreateAsync(CreateOrgRequest request);
    Task<OrgDto> UpdateAsync(UpdateOrgRequest request);
    Task DeleteAsync(Guid code);
    Task MoveAsync(Guid code, Guid? newParentCode);
}
