using Application.DTO.EmailQueue;
using Application.Interfaces.EmailQueue;
using Application.Options;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.UnitOfWorks;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.EmailQueue;

/// <summary>
/// 邮件队列服务实现，提供邮件创建、重新发送、查询和删除
/// </summary>
public class EmailQueueService : IEmailQueueService
{
    private readonly IRepository<SysEmailqueue> _emailRepo;
    private readonly IUnitOfWork _unitOfWork;

    public EmailQueueService(IRepository<SysEmailqueue> emailRepo, IUnitOfWork unitOfWork)
    {
        _emailRepo = emailRepo;
        _unitOfWork = unitOfWork;
    }

    /// <inheritdoc />
    public async Task<PagedResult<EmailQueueDto>> GetPagedListAsync(EmailQueueQueryRequest request)
    {
        var query = _emailRepo.Query().Where(e => !e.IsDelete);

        if (request.SendStatus.HasValue)
            query = query.Where(e => e.SendStatus == request.SendStatus.Value);

        if (!string.IsNullOrWhiteSpace(request.Keyword))
            query = query.Where(e => e.Subject.Contains(request.Keyword)
                                  || e.ToAddress.Contains(request.Keyword)
                                  || e.EmailCode.Contains(request.Keyword));

        if (request.StartDate.HasValue)
            query = query.Where(e => e.CreateTime >= request.StartDate.Value);
        if (request.EndDate.HasValue)
            query = query.Where(e => e.CreateTime <= request.EndDate.Value);

        var total = await query.CountAsync();
        var items = await query
            .OrderByDescending(e => e.CreateTime)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new EmailQueueDto
            {
                Code = e.Code,
                EmailCode = e.EmailCode,
                Subject = e.Subject,
                Content = e.Content,
                ToAddress = e.ToAddress,
                CcAddress = e.CcAddress,
                BccAddress = e.BccAddress,
                IsBodyHtml = e.IsBodyHtml == 1,
                SendStatus = e.SendStatus,
                RetryCount = e.RetryCount,
                MaxRetryCount = e.MaxRetryCount,
                SentTime = e.SentTime,
                ErrorMessage = e.ErrorMessage,
                BusinessType = e.BusinessType,
                BusinessId = e.BusinessId,
                Remark = e.Remark,
                CreateTime = e.CreateTime ?? DateTime.MinValue
            })
            .ToListAsync();

        return new PagedResult<EmailQueueDto>
        {
            Items = items,
            TotalCount = total,
            PageIndex = request.Page,
            PageSize = request.PageSize
        };
    }

    /// <inheritdoc />
    public async Task<EmailQueueDto?> GetByIdAsync(Guid code)
    {
        var email = await _emailRepo.Query()
            .FirstOrDefaultAsync(e => e.Code == code && !e.IsDelete);

        return email == null ? null : MapToDto(email);
    }

    /// <inheritdoc />
    public async Task<EmailQueueDto> SendAsync(SendEmailRequest request)
    {
        var emailCode = $"EMAIL_{DateTime.Now:yyyyMMddHHmmss}_{Guid.NewGuid():N}".ToUpper();

        var email = new SysEmailqueue
        {
            Code = Guid.NewGuid(),
            EmailCode = emailCode,
            Subject = request.Subject.Trim(),
            Content = request.Content,
            ToAddress = request.ToAddress.Trim(),
            CcAddress = request.CcAddress,
            BccAddress = request.BccAddress,
            IsBodyHtml = request.IsBodyHtml ? (ulong)1 : 0,
            SendStatus = 0,
            RetryCount = 0,
            MaxRetryCount = 3,
            SentTime = null,
            ErrorMessage = null,
            BusinessType = request.BusinessType,
            BusinessId = request.BusinessId,
            Remark = request.Remark,
            IsEnable = true,
            IsDelete = false,
            CreateTime = DateTime.Now
        };

        await _emailRepo.AddAsync(email);
        await _unitOfWork.CommitAsync();

        return MapToDto(email);
    }

    /// <inheritdoc />
    public async Task<EmailQueueDto> ResendAsync(Guid code)
    {
        var email = await _emailRepo.Query()
            .FirstOrDefaultAsync(e => e.Code == code && !e.IsDelete)
            ?? throw new InvalidOperationException("邮件记录不存在");

        if (email.SendStatus == 2)
            throw new InvalidOperationException("该邮件已发送成功，无需重新发送");

        email.SendStatus = 0;
        email.RetryCount = 0;
        email.ErrorMessage = null;
        email.ModifyTime = DateTime.Now;

        _emailRepo.Update(email);
        await _unitOfWork.CommitAsync();

        return MapToDto(email);
    }

    /// <inheritdoc />
    public async Task DeleteAsync(Guid code)
    {
        var email = await _emailRepo.Query()
            .FirstOrDefaultAsync(e => e.Code == code && !e.IsDelete)
            ?? throw new InvalidOperationException("邮件记录不存在");

        email.IsDelete = true;
        email.ModifyTime = DateTime.Now;
        _emailRepo.Update(email);
        await _unitOfWork.CommitAsync();
    }

    private static EmailQueueDto MapToDto(SysEmailqueue email)
    {
        return new EmailQueueDto
        {
            Code = email.Code,
            EmailCode = email.EmailCode,
            Subject = email.Subject,
            Content = email.Content,
            ToAddress = email.ToAddress,
            CcAddress = email.CcAddress,
            BccAddress = email.BccAddress,
            IsBodyHtml = email.IsBodyHtml == 1,
            SendStatus = email.SendStatus,
            RetryCount = email.RetryCount,
            MaxRetryCount = email.MaxRetryCount,
            SentTime = email.SentTime,
            ErrorMessage = email.ErrorMessage,
            BusinessType = email.BusinessType,
            BusinessId = email.BusinessId,
            Remark = email.Remark,
            CreateTime = email.CreateTime ?? DateTime.MinValue
        };
    }
}
