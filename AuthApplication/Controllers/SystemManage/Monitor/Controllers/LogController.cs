using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AuthApplication.Common;
using Infrastructure.Authentication.CurrentUser;
using Application.DTO.Log;
using Application.Interfaces.Log;

namespace AuthApplication.Controllers.SystemManage.Monitor.Controllers
{
    /// <summary>
    /// 日志管理控制器，提供日志查询和清理接口（只读）
    /// </summary>
    [ApiController]
    [Route("api/logs")]
    [Authorize]
    public class LogController : BaseController
    {
        private readonly ILogService _logService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logService">日志管理服务</param>
        /// <param name="currentUser">当前用户信息</param>
        public LogController(ILogService logService, ICurrentUser currentUser)
            : base(currentUser)
        {
            _logService = logService;
        }

        /// <summary>
        /// 分页查询日志
        /// </summary>
        /// <param name="request">查询参数</param>
        /// <returns>分页日志列表</returns>
        [HttpGet]
        public async Task<IActionResult> GetLogs([FromQuery] LogQueryRequest request)
        {
            var result = await _logService.GetPagedListAsync(request);
            return Success(result);
        }

        /// <summary>
        /// 获取日志详情
        /// </summary>
        /// <param name="code">日志 Code</param>
        /// <returns>日志详情</returns>
        [HttpGet("{code}")]
        public async Task<IActionResult> GetLog(Guid code)
        {
            var log = await _logService.GetByIdAsync(code);
            if (log == null)
                return Fail("日志不存在");

            return Success(log);
        }

        /// <summary>
        /// 清理指定日期之前的日志（软删除）
        /// </summary>
        /// <param name="beforeDate">清理该日期之前的日志</param>
        /// <returns>无内容</returns>
        [HttpDelete("clean")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CleanLogs([FromQuery] DateTime beforeDate)
        {
            try
            {
                await _logService.CleanAsync(beforeDate);
                return Success();
            }
            catch (InvalidOperationException ex)
            {
                return Fail(ex.Message);
            }
        }
    }
}
