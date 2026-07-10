namespace Application.DTO.Role;

public class RoleDto
{
    public Guid Code { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Level { get; set; }
    public Guid? SuperiorRoleCode { get; set; }
    public string? SuperiorRoleName { get; set; }
    public bool IsEnable { get; set; }
    public DateTime? CreateTime { get; set; }
}
