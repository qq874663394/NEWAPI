using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AuthApplication.Common;
using Infrastructure.Authentication.CurrentUser;
using Application.DTO.ReportLine;
using Application.Interfaces.ReportLine;

namespace AuthApplication.Controllers.SystemManage.Org.Controllers
{
    /// <summary>
    /// 汇报关系控制器
    /// </summary>
    [ApiController]
    [Route("api/report-lines")]
    [Authorize]
    public class ReportLineController : BaseController
    {
        private readonly IReportLineService _reportLineService;

        /// <summary>
        /// 构造函数
        /// </summary>
        public ReportLineController(IReportLineService reportLineService, ICurrentUser currentUser)
            : base(currentUser)
        {
            _reportLineService = reportLineService;
        }

        /// <summary>
        /// 分页查询汇报关系
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetList([FromQuery] ReportLineQueryRequest request)
        {
            var result = await _reportLineService.GetPagedListAsync(request);
            return Success(result);
        }

        /// <summary>
        /// 获取汇报关系详情
        /// </summary>
        [HttpGet("{code}")]
        public async Task<IActionResult> GetDetail(Guid code)
        {
            var result = await _reportLineService.GetByIdAsync(code);
            if (result == null)
                return Fail("汇报关系不存在");
            return Success(result);
        }

        /// <summary>
        /// 创建汇报关系
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateReportLineRequest request)
        {
            try
            {
                var result = await _reportLineService.CreateAsync(request);
                return Success(result);
            }
            catch (InvalidOperationException ex)
            {
                return Fail(ex.Message);
            }
        }

        /// <summary>
        /// 删除汇报关系
        /// </summary>
        [HttpDelete("{code}")]
        public async Task<IActionResult> Delete(Guid code)
        {
            try
            {
                await _reportLineService.DeleteAsync(code);
                return Success();
            }
            catch (InvalidOperationException ex)
            {
                return Fail(ex.Message);
            }
        }

        /// <summary>
        /// 设置启用/禁用
        /// </summary>
        [HttpPatch("{code}/active")]
        public async Task<IActionResult> SetActive(Guid code, [FromBody] bool isActive)
        {
            try
            {
                await _reportLineService.SetActiveAsync(code, isActive);
                return Success();
            }
            catch (InvalidOperationException ex)
            {
                return Fail(ex.Message);
            }
        }
    }
}
