using Application.DTO.ReportLine;
using Application.Interfaces.ReportLine;
using Application.Options;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.UnitOfWorks;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.ReportLine;

/// <summary>
/// 汇报关系服务实现
/// </summary>
public class ReportLineService : IReportLineService
{
    private readonly IRepository<SysReportline> _repo;
    private readonly IRepository<SysUser> _userRepo;
    private readonly IRepository<SysOrg> _orgRepo;
    private readonly IRepository<SysRole> _roleRepo;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// 构造函数
    /// </summary>
    public ReportLineService(
        IRepository<SysReportline> repo,
        IRepository<SysUser> userRepo,
        IRepository<SysOrg> orgRepo,
        IRepository<SysRole> roleRepo,
        IUnitOfWork unitOfWork)
    {
        _repo = repo;
        _userRepo = userRepo;
        _orgRepo = orgRepo;
        _roleRepo = roleRepo;
        _unitOfWork = unitOfWork;
    }

    /// <inheritdoc />
    public async Task<PagedResult<ReportLineDto>> GetPagedListAsync(ReportLineQueryRequest request)
    {
        var query = from r in _repo.Query().Where(x => !x.IsDelete)
                    join u in _userRepo.Query() on r.UserCode equals u.Code into uJoin
                    from u in uJoin.DefaultIfEmpty()
                    join s in _userRepo.Query() on r.SupervisorUserCode equals s.Code into sJoin
                    from s in sJoin.DefaultIfEmpty()
                    join o in _orgRepo.Query() on r.OrgCode equals o.Code into oJoin
                    from o in oJoin.DefaultIfEmpty()
                    join role in _roleRepo.Query() on r.RoleCode equals role.Code into roleJoin
                    from role in roleJoin.DefaultIfEmpty()
                    select new { r, u, s, o, role };

        if (request.UserCode.HasValue)
            query = query.Where(x => x.r.UserCode == request.UserCode.Value);
        if (request.SupervisorUserCode.HasValue)
            query = query.Where(x => x.r.SupervisorUserCode == request.SupervisorUserCode.Value);
        if (request.OrgCode.HasValue)
            query = query.Where(x => x.r.OrgCode == request.OrgCode.Value);
        if (request.IsActive.HasValue)
            query = query.Where(x => x.r.IsActive == (request.IsActive.Value ? (ulong)1 : 0));

        var total = await query.CountAsync();
        var items = await query
            .OrderByDescending(x => x.r.CreateTime)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(x => new ReportLineDto
            {
                Code = x.r.Code,
                UserCode = x.r.UserCode,
                UserName = x.u != null ? x.u.FullName : "",
                SupervisorUserCode = x.r.SupervisorUserCode,
                SupervisorUserName = x.s != null ? x.s.FullName : "",
                OrgCode = x.r.OrgCode,
                OrgName = x.o != null ? x.o.Name : "",
                RoleCode = x.r.RoleCode,
                RoleName = x.role != null ? x.role.Name : null,
                IsActive = x.r.IsActive == 1,
                EffectiveDate = x.r.EffectiveDate,
                ExpiryDate = x.r.ExpiryDate,
                CreateTime = x.r.CreateTime ?? DateTime.MinValue
            })
            .ToListAsync();

        return new PagedResult<ReportLineDto>
        {
            Items = items,
            TotalCount = total,
            PageIndex = request.Page,
            PageSize = request.PageSize
        };
    }

    /// <inheritdoc />
    public async Task<ReportLineDto?> GetByIdAsync(Guid code)
    {
        var result = await (from r in _repo.Query().Where(x => x.Code == code && !x.IsDelete)
                           join u in _userRepo.Query() on r.UserCode equals u.Code into uJoin
                           from u in uJoin.DefaultIfEmpty()
                           join s in _userRepo.Query() on r.SupervisorUserCode equals s.Code into sJoin
                           from s in sJoin.DefaultIfEmpty()
                           join o in _orgRepo.Query() on r.OrgCode equals o.Code into oJoin
                           from o in oJoin.DefaultIfEmpty()
                           join role in _roleRepo.Query() on r.RoleCode equals role.Code into roleJoin
                           from role in roleJoin.DefaultIfEmpty()
                           select new ReportLineDto
                           {
                               Code = r.Code,
                               UserCode = r.UserCode,
                               UserName = u != null ? u.UserName : "",
                               SupervisorUserCode = r.SupervisorUserCode,
                               SupervisorUserName = s != null ? s.UserName : "",
                               OrgCode = r.OrgCode,
                               OrgName = o != null ? o.Name : "",
                               RoleCode = r.RoleCode,
                               RoleName = role != null ? role.Name : null,
                               IsActive = r.IsActive == 1,
                               EffectiveDate = r.EffectiveDate,
                               ExpiryDate = r.ExpiryDate,
                               CreateTime = r.CreateTime ?? DateTime.MinValue
                           }).FirstOrDefaultAsync();

        return result;
    }

    /// <inheritdoc />
    public async Task<ReportLineDto> CreateAsync(CreateReportLineRequest request)
    {
        if (request.UserCode == request.SupervisorUserCode)
            throw new InvalidOperationException("员工和直接上级不能是同一人");

        var exists = await _repo.Query()
            .AnyAsync(r => r.UserCode == request.UserCode
                        && r.SupervisorUserCode == request.SupervisorUserCode
                        && r.OrgCode == request.OrgCode
                        && r.IsActive == 1
                        && !r.IsDelete);
        if (exists)
            throw new InvalidOperationException("该汇报关系已存在");

        var entity = new SysReportline
        {
            Code = Guid.NewGuid(),
            UserCode = request.UserCode,
            SupervisorUserCode = request.SupervisorUserCode,
            OrgCode = request.OrgCode,
            RoleCode = request.RoleCode,
            IsActive = request.IsActive ? (ulong)1 : 0,
            EffectiveDate = request.EffectiveDate,
            ExpiryDate = request.ExpiryDate,
            IsEnable = true,
            IsDelete = false,
            CreateTime = DateTime.Now
        };

        await _repo.AddAsync(entity);
        await _unitOfWork.CommitAsync();

        return (await GetByIdAsync(entity.Code))!;
    }

    /// <inheritdoc />
    public async Task DeleteAsync(Guid code)
    {
        var entity = await _repo.Query()
            .FirstOrDefaultAsync(x => x.Code == code && !x.IsDelete)
            ?? throw new InvalidOperationException("汇报关系不存在");

        entity.IsDelete = true;
        entity.ModifyTime = DateTime.Now;
        _repo.Update(entity);
        await _unitOfWork.CommitAsync();
    }

    /// <inheritdoc />
    public async Task SetActiveAsync(Guid code, bool isActive)
    {
        var entity = await _repo.Query()
            .FirstOrDefaultAsync(x => x.Code == code && !x.IsDelete)
            ?? throw new InvalidOperationException("汇报关系不存在");

        entity.IsActive = isActive ? (ulong)1 : 0;
        entity.ModifyTime = DateTime.Now;
        _repo.Update(entity);
        await _unitOfWork.CommitAsync();
    }
}
