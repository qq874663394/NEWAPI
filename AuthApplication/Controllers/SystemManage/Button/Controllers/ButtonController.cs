using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AuthApplication.Common;
using Infrastructure.Authentication.CurrentUser;
using Application.DTO.Button;
using Application.Interfaces.Button;

namespace AuthApplication.Controllers.SystemManage.Button.Controllers
{
    /// <summary>
    /// 按钮管理控制器，提供按钮 CRUD 及按菜单查询接口
    /// </summary>
    [ApiController]
    [Route("api/buttons")]
    [Authorize]
    public class ButtonController : BaseController
    {
        private readonly IButtonService _buttonService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="buttonService">按钮管理服务</param>
        /// <param name="currentUser">当前用户信息</param>
        public ButtonController(IButtonService buttonService, ICurrentUser currentUser)
            : base(currentUser)
        {
            _buttonService = buttonService;
        }

        /// <summary>
        /// 获取指定菜单下的按钮列表
        /// </summary>
        /// <param name="routeCode">菜单 Code</param>
        /// <returns>按钮列表</returns>
        [HttpGet("by-route/{routeCode}")]
        public async Task<IActionResult> GetByRoute(Guid routeCode)
        {
            var buttons = await _buttonService.GetByRouteAsync(routeCode);
            return Success(buttons);
        }

        /// <summary>
        /// 获取按钮详情
        /// </summary>
        /// <param name="code">按钮 Code</param>
        /// <returns>按钮详情</returns>
        [HttpGet("{code}")]
        public async Task<IActionResult> GetButton(Guid code)
        {
            var button = await _buttonService.GetByIdAsync(code);
            if (button == null)
                return Fail("按钮不存在");

            return Success(button);
        }

        /// <summary>
        /// 创建按钮
        /// </summary>
        /// <param name="request">创建请求</param>
        /// <returns>创建的按钮信息</returns>
        [HttpPost]
        public async Task<IActionResult> CreateButton([FromBody] CreateButtonRequest request)
        {
            try
            {
                var button = await _buttonService.CreateAsync(request);
                return Success(button);
            }
            catch (InvalidOperationException ex)
            {
                return Fail(ex.Message);
            }
        }

        /// <summary>
        /// 更新按钮
        /// </summary>
        /// <param name="code">按钮 Code</param>
        /// <param name="request">更新请求</param>
        /// <returns>更新后的按钮信息</returns>
        [HttpPut("{code}")]
        public async Task<IActionResult> UpdateButton(Guid code, [FromBody] UpdateButtonRequest request)
        {
            if (code != request.Code)
                return Fail("URL 中的 Code 与请求体不一致");

            try
            {
                var button = await _buttonService.UpdateAsync(request);
                return Success(button);
            }
            catch (InvalidOperationException ex)
            {
                return Fail(ex.Message);
            }
        }

        /// <summary>
        /// 删除按钮（系统内置按钮不可删除）
        /// </summary>
        /// <param name="code">按钮 Code</param>
        /// <returns>无内容</returns>
        [HttpDelete("{code}")]
        public async Task<IActionResult> DeleteButton(Guid code)
        {
            try
            {
                await _buttonService.DeleteAsync(code);
                return Success();
            }
            catch (InvalidOperationException ex)
            {
                return Fail(ex.Message);
            }
        }
    }
}
