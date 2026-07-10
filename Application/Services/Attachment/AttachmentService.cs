using Application.DTO.Attachment;
using Application.Interfaces.Attachment;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.UnitOfWorks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Attachment;

/// <summary>
/// 附件管理服务实现，提供附件上传、下载、查询和删除
/// </summary>
public class AttachmentService : IAttachmentService
{
    private readonly IRepository<SysAttachment> _attachmentRepo;
    private readonly IUnitOfWork _unitOfWork;
    private readonly string _uploadRootPath;

    public AttachmentService(
        IRepository<SysAttachment> attachmentRepo,
        IUnitOfWork unitOfWork)
    {
        _attachmentRepo = attachmentRepo;
        _unitOfWork = unitOfWork;
        // 默认上传目录，可根据配置调整
        _uploadRootPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
        if (!Directory.Exists(_uploadRootPath))
            Directory.CreateDirectory(_uploadRootPath);
    }

    /// <inheritdoc />
    public async Task<List<AttachmentDto>> GetListAsync(AttachmentQueryRequest request)
    {
        var query = _attachmentRepo.Query().Where(a => !a.IsDelete);

        if (!string.IsNullOrWhiteSpace(request.EntityType))
            query = query.Where(a => a.EntityType == request.EntityType);
        if (request.EntityCode.HasValue)
            query = query.Where(a => a.EntityCode == request.EntityCode.Value);

        var attachments = await query.OrderByDescending(a => a.CreateTime).ToListAsync();

        return attachments.Select(a => MapToDto(a)).ToList();
    }

    /// <inheritdoc />
    public async Task<AttachmentDto?> GetByIdAsync(Guid code)
    {
        var attachment = await _attachmentRepo.Query()
            .FirstOrDefaultAsync(a => a.Code == code && !a.IsDelete);

        return attachment == null ? null : MapToDto(attachment);
    }

    /// <inheritdoc />
    public async Task<AttachmentDto> UploadAsync(IFormFile file, UploadAttachmentRequest request)
    {
        if (file == null || file.Length == 0)
            throw new InvalidOperationException("上传文件不能为空");

        // 生成唯一文件名
        var fileExtension = Path.GetExtension(file.FileName);
        var newFileName = $"{Guid.NewGuid()}{fileExtension}";
        var dateDir = DateTime.Now.ToString("yyyyMMdd");
        var dirPath = Path.Combine(_uploadRootPath, dateDir);

        if (!Directory.Exists(dirPath))
            Directory.CreateDirectory(dirPath);

        var fullPath = Path.Combine(dirPath, newFileName);

        // 保存文件
        await using (var stream = new FileStream(fullPath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        // 计算 MD5
        string md5Hash;
        await using (var stream = File.OpenRead(fullPath))
        {
            using var md5 = System.Security.Cryptography.MD5.Create();
            var hashBytes = await md5.ComputeHashAsync(stream);
            md5Hash = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }

        var relativePath = Path.Combine("Uploads", dateDir, newFileName).Replace("\\", "/");

        var attachment = new SysAttachment
        {
            Code = Guid.NewGuid(),
            FileName = newFileName,
            FileExtension = fileExtension?.TrimStart('.') ?? "",
            FileClientName = file.FileName,
            FilePath = relativePath,
            FileType = file.ContentType,
            FileSize = file.Length,
            Md5hash = md5Hash,
            DownloadCount = 0,
            Description = request.Description,
            EntityType = request.EntityType,
            EntityCode = request.EntityCode,
            IsEnable = true,
            IsDelete = false,
            CreateTime = DateTime.Now
        };

        await _attachmentRepo.AddAsync(attachment);
        await _unitOfWork.CommitAsync();

        return MapToDto(attachment);
    }

    /// <inheritdoc />
    public async Task<(byte[] FileBytes, string FileName, string ContentType)> DownloadAsync(Guid code)
    {
        var attachment = await _attachmentRepo.Query()
            .FirstOrDefaultAsync(a => a.Code == code && !a.IsDelete)
            ?? throw new InvalidOperationException("附件不存在");

        var fullPath = Path.Combine(Directory.GetCurrentDirectory(), attachment.FilePath.Replace("/", "\\"));
        if (!File.Exists(fullPath))
            throw new InvalidOperationException("附件文件已丢失");

        // 增加下载计数
        attachment.DownloadCount = (attachment.DownloadCount ?? 0) + 1;
        attachment.ModifyTime = DateTime.Now;
        _attachmentRepo.Update(attachment);
        await _unitOfWork.CommitAsync();

        var fileBytes = await File.ReadAllBytesAsync(fullPath);
        return (fileBytes, attachment.FileClientName, attachment.FileType);
    }

    /// <inheritdoc />
    public async Task DeleteAsync(Guid code)
    {
        var attachment = await _attachmentRepo.Query()
            .FirstOrDefaultAsync(a => a.Code == code && !a.IsDelete)
            ?? throw new InvalidOperationException("附件不存在");

        // 删除物理文件
        var fullPath = Path.Combine(Directory.GetCurrentDirectory(), attachment.FilePath.Replace("/", "\\"));
        if (File.Exists(fullPath))
            File.Delete(fullPath);

        attachment.IsDelete = true;
        attachment.ModifyTime = DateTime.Now;
        _attachmentRepo.Update(attachment);
        await _unitOfWork.CommitAsync();
    }

    private static AttachmentDto MapToDto(SysAttachment attachment)
    {
        return new AttachmentDto
        {
            Code = attachment.Code,
            FileName = attachment.FileName,
            FileExtension = attachment.FileExtension,
            FileClientName = attachment.FileClientName,
            FilePath = attachment.FilePath,
            FileType = attachment.FileType,
            FileSize = attachment.FileSize,
            Md5hash = attachment.Md5hash,
            DownloadCount = attachment.DownloadCount,
            Description = attachment.Description,
            EntityType = attachment.EntityType,
            EntityCode = attachment.EntityCode,
            IsEnable = attachment.IsEnable,
            CreateTime = attachment.CreateTime ?? DateTime.MinValue
        };
    }
}
