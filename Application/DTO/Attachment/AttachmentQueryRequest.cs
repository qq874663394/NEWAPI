namespace Application.DTO.Attachment;

/// <summary>
/// 附件查询请求
/// </summary>
public class AttachmentQueryRequest
{
    public string? EntityType { get; set; }
    public Guid? EntityCode { get; set; }
}
