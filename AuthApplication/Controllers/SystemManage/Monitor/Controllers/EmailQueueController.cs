using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AuthApplication.Common;
using Infrastructure.Authentication.CurrentUser;
using Application.DTO.EmailQueue;
using Application.Interfaces.EmailQueue;

namespace AuthApplication.Controllers.SystemManage.Monitor.Controllers
{
    /// <summary>
    /// 邮件队列控制器，提供邮件发送、查询和管理接口
    /// </summary>
    [ApiController]
    [Route("api/email-queue")]
    [Authorize]
    public class EmailQueueController : BaseController
    {
        private readonly IEmailQueueService _emailQueueService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="emailQueueService">邮件队列服务</param>
        /// <param name="currentUser">当前用户信息</param>
        public EmailQueueController(IEmailQueueService emailQueueService, ICurrentUser currentUser)
            : base(currentUser)
        {
            _emailQueueService = emailQueueService;
        }

        /// <summary>
        /// 分页查询邮件队列
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetEmails([FromQuery] EmailQueueQueryRequest request)
        {
            var result = await _emailQueueService.GetPagedListAsync(request);
            return Success(result);
        }

        /// <summary>
        /// 获取邮件详情
        /// </summary>
        [HttpGet("{code}")]
        public async Task<IActionResult> GetEmail(Guid code)
        {
            var email = await _emailQueueService.GetByIdAsync(code);
            if (email == null)
                return Fail("邮件记录不存在");

            return Success(email);
        }

        /// <summary>
        /// 创建并发送邮件（加入发送队列）
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Send([FromBody] SendEmailRequest request)
        {
            try
            {
                var result = await _emailQueueService.SendAsync(request);
                return Success(result);
            }
            catch (InvalidOperationException ex)
            {
                return Fail(ex.Message);
            }
        }

        /// <summary>
        /// 重新发送失败邮件
        /// </summary>
        [HttpPost("{code}/resend")]
        public async Task<IActionResult> Resend(Guid code)
        {
            try
            {
                var result = await _emailQueueService.ResendAsync(code);
                return Success(result);
            }
            catch (InvalidOperationException ex)
            {
                return Fail(ex.Message);
            }
        }

        /// <summary>
        /// 删除邮件记录
        /// </summary>
        [HttpDelete("{code}")]
        public async Task<IActionResult> DeleteEmail(Guid code)
        {
            try
            {
                await _emailQueueService.DeleteAsync(code);
                return Success();
            }
            catch (InvalidOperationException ex)
            {
                return Fail(ex.Message);
            }
        }
    }
}
