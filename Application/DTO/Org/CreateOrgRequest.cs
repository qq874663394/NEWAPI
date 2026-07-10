using System.ComponentModel.DataAnnotations;

namespace Application.DTO.Org;

public class CreateOrgRequest
{
    [Required(ErrorMessage = "组织名称不能为空")]
    public string Name { get; set; } = string.Empty;
    [Required(ErrorMessage = "组织类型不能为空")]
    public string OrgType { get; set; } = string.Empty;
    public Guid? ParentCode { get; set; }
    public int Sort { get; set; }
    public bool IsVirtual { get; set; }
}
