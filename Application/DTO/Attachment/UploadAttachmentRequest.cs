using System.ComponentModel.DataAnnotations;

namespace Application.DTO.Attachment;

/// <summary>
/// 上传附件请求
/// </summary>
public class UploadAttachmentRequest
{
    [Required(ErrorMessage = "业务实体类型不能为空")]
    public string EntityType { get; set; } = string.Empty;

    [Required]
    public Guid EntityCode { get; set; }

    public string? Description { get; set; }
}
