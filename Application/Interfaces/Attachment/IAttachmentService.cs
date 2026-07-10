using Application.DTO.Attachment;
using Microsoft.AspNetCore.Http;

namespace Application.Interfaces.Attachment;

/// <summary>
/// 附件管理服务接口
/// </summary>
public interface IAttachmentService
{
    /// <summary>查询附件列表</summary>
    Task<List<AttachmentDto>> GetListAsync(AttachmentQueryRequest request);

    /// <summary>获取附件详情</summary>
    Task<AttachmentDto?> GetByIdAsync(Guid code);

    /// <summary>上传附件</summary>
    Task<AttachmentDto> UploadAsync(IFormFile file, UploadAttachmentRequest request);

    /// <summary>下载附件（同时增加下载计数）</summary>
    Task<(byte[] FileBytes, string FileName, string ContentType)> DownloadAsync(Guid code);

    /// <summary>删除附件</summary>
    Task DeleteAsync(Guid code);
}
