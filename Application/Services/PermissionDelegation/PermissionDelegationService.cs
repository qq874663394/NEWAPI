using Application.DTO.PermissionDelegation;
using Application.Interfaces.PermissionDelegation;
using Application.Options;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.UnitOfWorks;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.PermissionDelegation;

/// <summary>
/// 权限委托服务实现
/// </summary>
public class PermissionDelegationService : IPermissionDelegationService
{
    private readonly IRepository<SysPermissiondelegation> _repo;
    private readonly IRepository<SysUser> _userRepo;
    private readonly IRepository<SysButton> _buttonRepo;
    private readonly IRepository<SysRoute> _routeRepo;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// 构造函数
    /// </summary>
    public PermissionDelegationService(
        IRepository<SysPermissiondelegation> repo,
        IRepository<SysUser> userRepo,
        IRepository<SysButton> buttonRepo,
        IRepository<SysRoute> routeRepo,
        IUnitOfWork unitOfWork)
    {
        _repo = repo;
        _userRepo = userRepo;
        _buttonRepo = buttonRepo;
        _routeRepo = routeRepo;
        _unitOfWork = unitOfWork;
    }

    /// <inheritdoc />
    public async Task<PagedResult<PermissionDelegationDto>> GetPagedListAsync(DelegationQueryRequest request)
    {
        var query = from d in _repo.Query().Where(x => !x.IsDelete)
                    join fromUser in _userRepo.Query() on d.FromUserCode equals fromUser.Code into fJoin
                    from fromUser in fJoin.DefaultIfEmpty()
                    join toUser in _userRepo.Query() on d.ToUserCode equals toUser.Code into tJoin
                    from toUser in tJoin.DefaultIfEmpty()
                    join btn in _buttonRepo.Query() on d.ButtonCode equals btn.Code into bJoin
                    from btn in bJoin.DefaultIfEmpty()
                    join route in _routeRepo.Query() on d.RouteCode equals route.Code into rJoin
                    from route in rJoin.DefaultIfEmpty()
                    select new { d, fromUser, toUser, btn, route };

        if (request.FromUserCode.HasValue)
            query = query.Where(x => x.d.FromUserCode == request.FromUserCode.Value);
        if (request.ToUserCode.HasValue)
            query = query.Where(x => x.d.ToUserCode == request.ToUserCode.Value);
        if (request.ButtonCode.HasValue)
            query = query.Where(x => x.d.ButtonCode == request.ButtonCode.Value);
        if (request.RouteCode.HasValue)
            query = query.Where(x => x.d.RouteCode == request.RouteCode.Value);
        if (request.IsActive.HasValue)
            query = query.Where(x => x.d.IsActive == (request.IsActive.Value ? (ulong)1 : 0));

        var total = await query.CountAsync();
        var items = await query
            .OrderByDescending(x => x.d.CreateTime)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(x => new PermissionDelegationDto
            {
                Code = x.d.Code,
                FromUserCode = x.d.FromUserCode,
                FromUserName = x.fromUser != null ? x.fromUser.FullName : "",
                ToUserCode = x.d.ToUserCode,
                ToUserName = x.toUser != null ? x.toUser.FullName : "",
                ButtonCode = x.d.ButtonCode,
                ButtonName = x.btn != null ? x.btn.Name : "",
                RouteCode = x.d.RouteCode,
                RouteName = x.route != null ? x.route.Name : null,
                Condition = x.d.Condition,
                EffectiveStartDate = x.d.EffectiveStartDate,
                EffectiveEndDate = x.d.EffectiveEndDate,
                IsActive = x.d.IsActive == 1,
                CreateTime = x.d.CreateTime ?? DateTime.MinValue
            })
            .ToListAsync();

        return new PagedResult<PermissionDelegationDto>
        {
            Items = items,
            TotalCount = total,
            PageIndex = request.Page,
            PageSize = request.PageSize
        };
    }

    /// <inheritdoc />
    public async Task<PermissionDelegationDto?> GetByIdAsync(Guid code)
    {
        var result = await (from d in _repo.Query().Where(x => x.Code == code && !x.IsDelete)
                           join fromUser in _userRepo.Query() on d.FromUserCode equals fromUser.Code into fJoin
                           from fromUser in fJoin.DefaultIfEmpty()
                           join toUser in _userRepo.Query() on d.ToUserCode equals toUser.Code into tJoin
                           from toUser in tJoin.DefaultIfEmpty()
                           join btn in _buttonRepo.Query() on d.ButtonCode equals btn.Code into bJoin
                           from btn in bJoin.DefaultIfEmpty()
                           join route in _routeRepo.Query() on d.RouteCode equals route.Code into rJoin
                           from route in rJoin.DefaultIfEmpty()
                           select new PermissionDelegationDto
                           {
                               Code = d.Code,
                               FromUserCode = d.FromUserCode,
                               FromUserName = fromUser != null ? fromUser.FullName : "",
                               ToUserCode = d.ToUserCode,
                               ToUserName = toUser != null ? toUser.FullName : "",
                               ButtonCode = d.ButtonCode,
                               ButtonName = btn != null ? btn.Name : "",
                               RouteCode = d.RouteCode,
                               RouteName = route != null ? route.Name : null,
                               Condition = d.Condition,
                               EffectiveStartDate = d.EffectiveStartDate,
                               EffectiveEndDate = d.EffectiveEndDate,
                               IsActive = d.IsActive == 1,
                               CreateTime = d.CreateTime ?? DateTime.MinValue
                           }).FirstOrDefaultAsync();

        return result;
    }

    /// <inheritdoc />
    public async Task<PermissionDelegationDto> CreateAsync(CreateDelegationRequest request)
    {
        if (request.FromUserCode == request.ToUserCode)
            throw new InvalidOperationException("授权人和被授权人不能是同一用户");

        if (request.EffectiveStartDate >= request.EffectiveEndDate)
            throw new InvalidOperationException("生效结束时间必须晚于生效开始时间");

        var entity = new SysPermissiondelegation
        {
            Code = Guid.NewGuid(),
            FromUserCode = request.FromUserCode,
            ToUserCode = request.ToUserCode,
            ButtonCode = request.ButtonCode,
            RouteCode = request.RouteCode,
            Condition = request.Condition ?? "{}",
            EffectiveStartDate = request.EffectiveStartDate,
            EffectiveEndDate = request.EffectiveEndDate,
            IsActive = request.IsActive ? (ulong)1 : 0,
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
            ?? throw new InvalidOperationException("权限委托不存在");

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
            ?? throw new InvalidOperationException("权限委托不存在");

        entity.IsActive = isActive ? (ulong)1 : 0;
        entity.ModifyTime = DateTime.Now;
        _repo.Update(entity);
        await _unitOfWork.CommitAsync();
    }
}
