using System.ComponentModel.DataAnnotations;

namespace Application.DTO.Dictionary;

/// <summary>
/// 更新字典项请求
/// </summary>
public class UpdateDictRequest
{
    [Required]
    public Guid Code { get; set; }

    [Required(ErrorMessage = "字典类别不能为空")]
    public string Type { get; set; } = string.Empty;

    [Required(ErrorMessage = "字典键不能为空")]
    public string KeyText { get; set; } = string.Empty;

    [Required(ErrorMessage = "字典值不能为空")]
    public string ValueText { get; set; } = string.Empty;

    public string? Description { get; set; }
    public Guid? ParentCode { get; set; }
    public int Sort { get; set; }
    public bool IsEnable { get; set; }
}
