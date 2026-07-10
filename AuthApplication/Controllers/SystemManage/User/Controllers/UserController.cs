using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AuthApplication.Common;
using Infrastructure.Authentication.CurrentUser;
using Application.DTO.User;
using Application.Interfaces.User;

namespace AuthApplication.Controllers.SystemManage.User.Controllers
{
    /// <summary>
    /// 用户管理控制器，提供用户 CRUD、密码管理、锁定/解锁、岗位分配等接口
    /// </summary>
    [ApiController]
    [Route("api/users")]
    [Authorize]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="userService">用户管理服务</param>
        /// <param name="currentUser">当前用户信息</param>
        public UserController(IUserService userService, ICurrentUser currentUser)
            : base(currentUser)
        {
            _userService = userService;
        }

        /// <summary>
        /// 分页查询用户（支持按组织、关键字、活跃状态筛选）
        /// </summary>
        /// <param name="request">查询参数</param>
        /// <returns>分页结果</returns>
        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery] UserQueryRequest request)
        {
            var result = await _userService.GetListAsync(request);
            return Success(result);
        }

        /// <summary>
        /// 获取用户详情（含完整组织+角色列表）
        /// </summary>
        /// <param name="code">用户 Code</param>
        /// <returns>用户详情</returns>
        [HttpGet("{code}")]
        public async Task<IActionResult> GetUser(Guid code)
        {
            var user = await _userService.GetByIdAsync(code);
            if (user == null)
                return Fail("用户不存在");

            return Success(user);
        }

        /// <summary>
        /// 创建用户（含初始角色+组织分配）
        /// </summary>
        /// <param name="request">创建请求</param>
        /// <returns>创建的用户信息</returns>
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
        {
            try
            {
                var user = await _userService.CreateAsync(request);
                return Success(user);
            }
            catch (InvalidOperationException ex)
            {
                return Fail(ex.Message);
            }
        }

        /// <summary>
        /// 更新用户基本信息
        /// </summary>
        /// <param name="code">用户 Code</param>
        /// <param name="request">更新请求</param>
        /// <returns>更新后的用户信息</returns>
        [HttpPut("{code}")]
        public async Task<IActionResult> UpdateUser(Guid code, [FromBody] UpdateUserRequest request)
        {
            if (code != request.Code)
                return Fail("URL 中的 Code 与请求体不一致");

            try
            {
                var updated = await _userService.UpdateAsync(request);
                return Success(updated);
            }
            catch (InvalidOperationException ex)
            {
                return Fail(ex.Message);
            }
        }

        /// <summary>
        /// 软删除用户
        /// </summary>
        /// <param name="code">用户 Code</param>
        /// <returns>无内容</returns>
        [HttpDelete("{code}")]
        public async Task<IActionResult> DeleteUser(Guid code)
        {
            try
            {
                await _userService.DeleteAsync(code);
                return Success();
            }
            catch (InvalidOperationException ex)
            {
                return Fail(ex.Message);
            }
        }

        /// <summary>
        /// 修改密码（需提供旧密码）
        /// </summary>
        /// <param name="code">用户 Code</param>
        /// <param name="request">密码修改请求</param>
        /// <returns>无内容</returns>
        [HttpPut("{code}/password")]
        public async Task<IActionResult> ChangePassword(Guid code, [FromBody] ChangePasswordRequest request)
        {
            if (code != request.UserCode)
                return Fail("URL 中的 Code 与请求体不一致");

            try
            {
                await _userService.ChangePasswordAsync(request);
                return Success();
            }
            catch (InvalidOperationException ex)
            {
                return Fail(ex.Message);
            }
        }

        /// <summary>
        /// 管理员重置密码（无需旧密码）
        /// </summary>
        /// <param name="code">用户 Code</param>
        /// <param name="request">新密码</param>
        /// <returns>无内容</returns>
        [HttpPut("{code}/reset-password")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ResetPassword(Guid code, [FromBody] ResetPasswordRequest request)
        {
            try
            {
                await _userService.ResetPasswordAsync(code, request.NewPassword);
                return Success();
            }
            catch (InvalidOperationException ex)
            {
                return Fail(ex.Message);
            }
        }

        /// <summary>
        /// 锁定用户
        /// </summary>
        /// <param name="code">用户 Code</param>
        /// <returns>无内容</returns>
        [HttpPut("{code}/lock")]
        public async Task<IActionResult> LockUser(Guid code)
        {
            try
            {
                await _userService.LockAsync(code);
                return Success();
            }
            catch (InvalidOperationException ex)
            {
                return Fail(ex.Message);
            }
        }

        /// <summary>
        /// 解锁用户
        /// </summary>
        /// <param name="code">用户 Code</param>
        /// <returns>无内容</returns>
        [HttpPut("{code}/unlock")]
        public async Task<IActionResult> UnlockUser(Guid code)
        {
            try
            {
                await _userService.UnlockAsync(code);
                return Success();
            }
            catch (InvalidOperationException ex)
            {
                return Fail(ex.Message);
            }
        }

        /// <summary>
        /// 分配岗位（调整角色+组织关联）
        /// </summary>
        /// <param name="code">用户 Code</param>
        /// <param name="assignments">岗位列表</param>
        /// <returns>无内容</returns>
        [HttpPut("{code}/roles")]
        public async Task<IActionResult> AssignRoles(Guid code, [FromBody] List<AssignRoleOrgRequest> assignments)
        {
            try
            {
                await _userService.AssignRolesAsync(code, assignments);
                return Success();
            }
            catch (InvalidOperationException ex)
            {
                return Fail(ex.Message);
            }
        }
    }

}