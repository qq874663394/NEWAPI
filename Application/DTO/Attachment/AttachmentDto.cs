namespace Application.DTO.Attachment;

/// <summary>
/// 附件 DTO
/// </summary>
public class AttachmentDto
{
    public Guid Code { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string FileExtension { get; set; } = string.Empty;
    public string FileClientName { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;
    public string FileType { get; set; } = string.Empty;
    public long? FileSize { get; set; }
    public string? Md5hash { get; set; }
    public int? DownloadCount { get; set; }
    public string? Description { get; set; }
    public string EntityType { get; set; } = string.Empty;
    public Guid EntityCode { get; set; }
    public bool IsEnable { get; set; }
    public DateTime CreateTime { get; set; }
}
