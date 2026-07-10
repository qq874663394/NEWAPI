using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AuthApplication.Common;
using Infrastructure.Authentication.CurrentUser;
using Application.DTO.Permission;
using Application.Interfaces.Permission;

namespace AuthApplication.Controllers.SystemManage.Permission.Controllers
{
    /// <summary>
    /// 按钮权限控制器，提供按钮权限的分配与查询接口
    /// </summary>
    [ApiController]
    [Route("api/permissions/buttons")]
    [Authorize]
    public class ButtonPermissionController : BaseController
    {
        private readonly IButtonPermissionService _buttonPermissionService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="buttonPermissionService">按钮权限服务</param>
        /// <param name="currentUser">当前用户信息</param>
        public ButtonPermissionController(IButtonPermissionService buttonPermissionService, ICurrentUser currentUser)
            : base(currentUser)
        {
            _buttonPermissionService = buttonPermissionService;
        }

        /// <summary>查询按钮权限列表</summary>
        [HttpGet]
        public async Task<IActionResult> GetList([FromQuery] Guid? buttonCode, [FromQuery] string? subjectType, [FromQuery] Guid? subjectCode)
        {
            var list = await _buttonPermissionService.GetListAsync(buttonCode, subjectType, subjectCode);
            return Success(list);
        }

        /// <summary>获取某主体的已授权按钮Code列表</summary>
        [HttpGet("granted/{subjectType}/{subjectCode}")]
        public async Task<IActionResult> GetGrantedButtonCodes(string subjectType, Guid subjectCode)
        {
            var codes = await _buttonPermissionService.GetGrantedButtonCodesAsync(subjectType, subjectCode);
            return Success(codes);
        }

        /// <summary>分配/取消单个按钮权限</summary>
        [HttpPost]
        public async Task<IActionResult> Assign([FromBody] AssignButtonPermissionRequest request)
        {
            try
            {
                var result = await _buttonPermissionService.AssignAsync(request);
                return Success(result);
            }
            catch (InvalidOperationException ex)
            {
                return Fail(ex.Message);
            }
        }

        /// <summary>批量分配按钮权限</summary>
        [HttpPut("batch")]
        public async Task<IActionResult> BatchAssign([FromBody] BatchAssignButtonPermissionRequest request)
        {
            try
            {
                await _buttonPermissionService.BatchAssignAsync(request);
                return Success();
            }
            catch (InvalidOperationException ex)
            {
                return Fail(ex.Message);
            }
        }
    }
}
