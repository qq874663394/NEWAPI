namespace Application.DTO.Org;

public class OrgDto
{
    public Guid Code { get; set; }
    public string Name { get; set; } = string.Empty;
    public Guid? ParentCode { get; set; }
    public string? ParentName { get; set; }
    public string Path { get; set; } = string.Empty;
    public int Level { get; set; }
    public string OrgType { get; set; } = string.Empty;
    public int Sort { get; set; }
    public bool IsVirtual { get; set; }
    public bool IsEnable { get; set; }
    public DateTime CreateTime { get; set; }
    public List<OrgDto>? Children { get; set; }
}
