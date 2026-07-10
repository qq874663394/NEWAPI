using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AuthApplication.Common;
using Infrastructure.Authentication.CurrentUser;
using Application.DTO.Attachment;
using Application.Interfaces.Attachment;

namespace AuthApplication.Controllers.SystemManage.File.Controllers
{
    /// <summary>
    /// 附件管理控制器，提供附件上传、下载、查询和删除接口
    /// </summary>
    [ApiController]
    [Route("api/attachments")]
    [Authorize]
    public class AttachmentController : BaseController
    {
        private readonly IAttachmentService _attachmentService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="attachmentService">附件管理服务</param>
        /// <param name="currentUser">当前用户信息</param>
        public AttachmentController(IAttachmentService attachmentService, ICurrentUser currentUser)
            : base(currentUser)
        {
            _attachmentService = attachmentService;
        }

        /// <summary>
        /// 查询附件列表
        /// </summary>
        /// <param name="request">查询参数</param>
        /// <returns>附件列表</returns>
        [HttpGet]
        public async Task<IActionResult> GetList([FromQuery] AttachmentQueryRequest request)
        {
            var list = await _attachmentService.GetListAsync(request);
            return Success(list);
        }

        /// <summary>
        /// 获取附件详情
        /// </summary>
        /// <param name="code">附件 Code</param>
        /// <returns>附件详情</returns>
        [HttpGet("{code}")]
        public async Task<IActionResult> GetAttachment(Guid code)
        {
            var attachment = await _attachmentService.GetByIdAsync(code);
            if (attachment == null)
                return Fail("附件不存在");

            return Success(attachment);
        }

        /// <summary>
        /// 上传附件（multipart/form-data）
        /// </summary>
        /// <param name="file">上传的文件</param>
        /// <param name="request">上传参数</param>
        /// <returns>上传后的附件信息</returns>
        [HttpPost("upload")]
        [RequestSizeLimit(50 * 1024 * 1024)] // 50MB
        public async Task<IActionResult> Upload(IFormFile file, [FromForm] UploadAttachmentRequest request)
        {
            try
            {
                var result = await _attachmentService.UploadAsync(file, request);
                return Success(result);
            }
            catch (InvalidOperationException ex)
            {
                return Fail(ex.Message);
            }
        }

        /// <summary>
        /// 下载附件
        /// </summary>
        /// <param name="code">附件 Code</param>
        /// <returns>文件流</returns>
        [HttpGet("{code}/download")]
        public async Task<IActionResult> Download(Guid code)
        {
            try
            {
                var (fileBytes, fileName, contentType) = await _attachmentService.DownloadAsync(code);
                return File(fileBytes, contentType, fileName);
            }
            catch (InvalidOperationException ex)
            {
                return Fail(ex.Message);
            }
        }

        /// <summary>
        /// 删除附件（同时删除物理文件）
        /// </summary>
        /// <param name="code">附件 Code</param>
        /// <returns>无内容</returns>
        [HttpDelete("{code}")]
        public async Task<IActionResult> DeleteAttachment(Guid code)
        {
            try
            {
                await _attachmentService.DeleteAsync(code);
                return Success();
            }
            catch (InvalidOperationException ex)
            {
                return Fail(ex.Message);
            }
        }
    }
}
