using System.ComponentModel.DataAnnotations;

namespace Application.DTO.Role;

public class CreateRoleRequest
{
    [Required(ErrorMessage = "角色名称不能为空")]
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Level { get; set; }
    public Guid? SuperiorRoleCode { get; set; }
}
