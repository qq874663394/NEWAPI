using Application.DTO.Log;
using Application.Interfaces.Log;
using Application.Options;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.UnitOfWorks;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Log;

/// <summary>
/// 日志管理服务实现，提供日志查询和清理（只读）
/// </summary>
public class LogService : ILogService
{
    private readonly IRepository<SysLog> _logRepo;
    private readonly IUnitOfWork _unitOfWork;

    public LogService(IRepository<SysLog> logRepo, IUnitOfWork unitOfWork)
    {
        _logRepo = logRepo;
        _unitOfWork = unitOfWork;
    }

    /// <inheritdoc />
    public async Task<PagedResult<LogDto>> GetPagedListAsync(LogQueryRequest request)
    {
        var query = _logRepo.Query().Where(l => !l.IsDelete);

        if (!string.IsNullOrWhiteSpace(request.Type))
            query = query.Where(l => l.Type == request.Type);

        if (!string.IsNullOrWhiteSpace(request.Keyword))
            query = query.Where(l => l.UserName.Contains(request.Keyword)
                                  || l.Content.Contains(request.Keyword)
                                  || l.Url.Contains(request.Keyword)
                                  || l.Ip.Contains(request.Keyword));

        if (request.StartDate.HasValue)
            query = query.Where(l => l.CreateTime >= request.StartDate.Value);

        if (request.EndDate.HasValue)
            query = query.Where(l => l.CreateTime <= request.EndDate.Value);

        var total = await query.CountAsync();
        var items = await query
            .OrderByDescending(l => l.CreateTime)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(l => new LogDto
            {
                Code = l.Code,
                UserName = l.UserName,
                Type = l.Type,
                MenuName = l.MenuName,
                ModuleName = l.ModuleName,
                ButtonName = l.ButtonName,
                Content = l.Content,
                Result = l.Result,
                Url = l.Url,
                Ip = l.Ip,
                WorkStationName = l.WorkStationName,
                Method = l.Method,
                Params = l.Params,
                CreateTime = l.CreateTime ?? DateTime.MinValue
            })
            .ToListAsync();

        return new PagedResult<LogDto>
        {
            Items = items,
            TotalCount = total,
            PageIndex = request.Page,
            PageSize = request.PageSize
        };
    }

    /// <inheritdoc />
    public async Task<LogDto?> GetByIdAsync(Guid code)
    {
        var log = await _logRepo.Query()
            .FirstOrDefaultAsync(l => l.Code == code && !l.IsDelete);

        if (log == null) return null;

        return new LogDto
        {
            Code = log.Code,
            UserName = log.UserName,
            Type = log.Type,
            MenuName = log.MenuName,
            ModuleName = log.ModuleName,
            ButtonName = log.ButtonName,
            Content = log.Content,
            Result = log.Result,
            Url = log.Url,
            Ip = log.Ip,
            WorkStationName = log.WorkStationName,
            Method = log.Method,
            Params = log.Params,
            CreateTime = log.CreateTime ?? DateTime.MinValue
        };
    }

    /// <inheritdoc />
    public async Task CleanAsync(DateTime beforeDate)
    {
        var logs = await _logRepo.ToListAsync(l => l.CreateTime < beforeDate && !l.IsDelete);

        foreach (var log in logs)
        {
            log.IsDelete = true;
            log.ModifyTime = DateTime.Now;
            _logRepo.Update(log);
        }

        await _unitOfWork.CommitAsync();
    }
}
