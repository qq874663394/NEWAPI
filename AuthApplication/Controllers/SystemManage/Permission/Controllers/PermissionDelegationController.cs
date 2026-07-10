using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AuthApplication.Common;
using Infrastructure.Authentication.CurrentUser;
using Application.DTO.PermissionDelegation;
using Application.Interfaces.PermissionDelegation;

namespace AuthApplication.Controllers.SystemManage.Permission.Controllers
{
    /// <summary>
    /// 权限委托控制器
    /// </summary>
    [ApiController]
    [Route("api/permission-delegations")]
    [Authorize]
    public class PermissionDelegationController : BaseController
    {
        private readonly IPermissionDelegationService _delegationService;

        /// <summary>
        /// 构造函数
        /// </summary>
        public PermissionDelegationController(IPermissionDelegationService delegationService, ICurrentUser currentUser)
            : base(currentUser)
        {
            _delegationService = delegationService;
        }

        /// <summary>
        /// 分页查询权限委托
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetList([FromQuery] DelegationQueryRequest request)
        {
            var result = await _delegationService.GetPagedListAsync(request);
            return Success(result);
        }

        /// <summary>
        /// 获取权限委托详情
        /// </summary>
        [HttpGet("{code}")]
        public async Task<IActionResult> GetDetail(Guid code)
        {
            var result = await _delegationService.GetByIdAsync(code);
            if (result == null) return Fail("权限委托不存在");
            return Success(result);
        }

        /// <summary>
        /// 创建权限委托
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateDelegationRequest request)
        {
            try
            {
                var result = await _delegationService.CreateAsync(request);
                return Success(result);
            }
            catch (InvalidOperationException ex) { return Fail(ex.Message); }
        }

        /// <summary>
        /// 删除权限委托
        /// </summary>
        [HttpDelete("{code}")]
        public async Task<IActionResult> Delete(Guid code)
        {
            try
            {
                await _delegationService.DeleteAsync(code);
                return Success();
            }
            catch (InvalidOperationException ex) { return Fail(ex.Message); }
        }

        /// <summary>
        /// 设置启用/禁用
        /// </summary>
        [HttpPatch("{code}/active")]
        public async Task<IActionResult> SetActive(Guid code, [FromBody] bool isActive)
        {
            try
            {
                await _delegationService.SetActiveAsync(code, isActive);
                return Success();
            }
            catch (InvalidOperationException ex) { return Fail(ex.Message); }
        }
    }
}
