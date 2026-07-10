namespace Application.DTO.Dictionary;

/// <summary>
/// 字典项 DTO
/// </summary>
public class DictDto
{
    public Guid Code { get; set; }
    public string Type { get; set; } = string.Empty;
    public string KeyText { get; set; } = string.Empty;
    public string ValueText { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Guid? ParentCode { get; set; }
    public string? ParentName { get; set; }
    public int Sort { get; set; }
    public bool IsEnable { get; set; }
    public DateTime CreateTime { get; set; }
    public List<DictDto>? Children { get; set; }
}
