using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AuthApplication.Common;
using Infrastructure.Authentication.CurrentUser;
using Application.DTO.Role;
using Application.Interfaces.Role;

namespace AuthApplication.Controllers.SystemManage.Role.Controllers
{
    /// <summary>
    /// 角色管理控制器，提供角色 CRUD 及树形查询接口
    /// </summary>
    [ApiController]
    [Route("api/roles")]
    [Authorize]
    public class RoleController : BaseController
    {
        private readonly IRoleService _roleService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="roleService">角色管理服务</param>
        /// <param name="currentUser">当前用户信息</param>
        public RoleController(IRoleService roleService, ICurrentUser currentUser)
            : base(currentUser)
        {
            _roleService = roleService;
        }

        /// <summary>
        /// 获取角色列表（支持关键字搜索）
        /// </summary>
        /// <param name="keyword">搜索关键字（可选）</param>
        /// <returns>角色列表</returns>
        [HttpGet]
        public async Task<IActionResult> GetRoles([FromQuery] string? keyword = null)
        {
            var roles = await _roleService.GetListAsync(keyword);
            return Success(roles);
        }

        /// <summary>
        /// 获取角色树（顶级节点列表，下级挂在 Children 下）
        /// </summary>
        /// <returns>角色树</returns>
        [HttpGet("tree")]
        public async Task<IActionResult> GetTree()
        {
            var roles = await _roleService.GetTreeAsync();
            return Success(roles);
        }

        /// <summary>
        /// 获取角色详情
        /// </summary>
        /// <param name="code">角色 Code</param>
        /// <returns>角色详情</returns>
        [HttpGet("{code}")]
        public async Task<IActionResult> GetRole(Guid code)
        {
            var role = await _roleService.GetByIdAsync(code);
            if (role == null)
                return Fail("角色不存在");

            return Success(role);
        }

        /// <summary>
        /// 创建角色
        /// </summary>
        /// <param name="request">创建请求</param>
        /// <returns>创建的角色信息</returns>
        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleRequest request)
        {
            try
            {
                var role = await _roleService.CreateAsync(request);
                return Success(role);
            }
            catch (InvalidOperationException ex)
            {
                return Fail(ex.Message);
            }
        }

        /// <summary>
        /// 更新角色
        /// </summary>
        /// <param name="code">角色 Code</param>
        /// <param name="request">更新请求</param>
        /// <returns>更新后的角色信息</returns>
        [HttpPut("{code}")]
        public async Task<IActionResult> UpdateRole(Guid code, [FromBody] UpdateRoleRequest request)
        {
            if (code != request.Code)
                return Fail("URL 中的 Code 与请求体不一致");

            try
            {
                var role = await _roleService.UpdateAsync(request);
                return Success(role);
            }
            catch (InvalidOperationException ex)
            {
                return Fail(ex.Message);
            }
        }

        /// <summary>
        /// 软删除角色
        /// </summary>
        /// <param name="code">角色 Code</param>
        /// <returns>无内容</returns>
        [HttpDelete("{code}")]
        public async Task<IActionResult> DeleteRole(Guid code)
        {
            try
            {
                await _roleService.DeleteAsync(code);
                return Success();
            }
            catch (InvalidOperationException ex)
            {
                return Fail(ex.Message);
            }
        }
    }
}
