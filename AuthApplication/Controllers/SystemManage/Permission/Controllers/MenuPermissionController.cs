using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AuthApplication.Common;
using Infrastructure.Authentication.CurrentUser;
using Application.DTO.Permission;
using Application.Interfaces.Permission;

namespace AuthApplication.Controllers.SystemManage.Permission.Controllers
{
    /// <summary>
    /// 菜单权限控制器，提供菜单权限的分配与查询接口
    /// </summary>
    [ApiController]
    [Route("api/permissions/menus")]
    [Authorize]
    public class MenuPermissionController : BaseController
    {
        private readonly IMenuPermissionService _menuPermissionService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="menuPermissionService">菜单权限服务</param>
        /// <param name="currentUser">当前用户信息</param>
        public MenuPermissionController(IMenuPermissionService menuPermissionService, ICurrentUser currentUser)
            : base(currentUser)
        {
            _menuPermissionService = menuPermissionService;
        }

        /// <summary>查询菜单权限列表</summary>
        [HttpGet]
        public async Task<IActionResult> GetList([FromQuery] Guid? routeCode, [FromQuery] string? subjectType, [FromQuery] Guid? subjectCode)
        {
            var list = await _menuPermissionService.GetListAsync(routeCode, subjectType, subjectCode);
            return Success(list);
        }

        /// <summary>获取某主体的已授权菜单</summary>
        [HttpGet("granted/{subjectType}/{subjectCode}")]
        public async Task<IActionResult> GetGrantedMenus(string subjectType, Guid subjectCode)
        {
            var list = await _menuPermissionService.GetGrantedMenusAsync(subjectType, subjectCode);
            return Success(list);
        }

        /// <summary>分配/取消单个菜单权限</summary>
        [HttpPost]
        public async Task<IActionResult> Assign([FromBody] AssignMenuPermissionRequest request)
        {
            try
            {
                var result = await _menuPermissionService.AssignAsync(request);
                return Success(result);
            }
            catch (InvalidOperationException ex)
            {
                return Fail(ex.Message);
            }
        }

        /// <summary>批量分配菜单权限</summary>
        [HttpPut("batch")]
        public async Task<IActionResult> BatchAssign([FromBody] BatchAssignMenuPermissionRequest request)
        {
            try
            {
                await _menuPermissionService.BatchAssignAsync(request);
                return Success();
            }
            catch (InvalidOperationException ex)
            {
                return Fail(ex.Message);
            }
        }
    }
}
