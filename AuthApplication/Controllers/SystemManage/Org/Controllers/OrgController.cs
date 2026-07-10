using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AuthApplication.Common;
using Infrastructure.Authentication.CurrentUser;
using Application.DTO.Org;
using Application.Interfaces.Org;

namespace AuthApplication.Controllers.SystemManage.Org.Controllers
{
    /// <summary>
    /// 组织架构控制器，提供组织 CRUD、树形查询及移动等接口
    /// </summary>
    [ApiController]
    [Route("api/orgs")]
    [Authorize]
    public class OrgController : BaseController
    {
        private readonly IOrgService _orgService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="orgService">组织架构服务</param>
        /// <param name="currentUser">当前用户信息</param>
        public OrgController(IOrgService orgService, ICurrentUser currentUser)
            : base(currentUser)
        {
            _orgService = orgService;
        }

        /// <summary>
        /// 获取组织列表（支持关键字搜索）
        /// </summary>
        /// <param name="keyword">搜索关键字（可选）</param>
        /// <returns>组织列表</returns>
        [HttpGet]
        public async Task<IActionResult> GetOrgs([FromQuery] string? keyword = null)
        {
            var orgs = await _orgService.GetListAsync(keyword);
            return Success(orgs);
        }

        /// <summary>
        /// 获取组织树（顶级节点列表，下级挂在 Children 下）
        /// </summary>
        /// <param name="parentCode">可选，指定父级 Code 时返回以该节点为根的子树</param>
        /// <returns>组织树</returns>
        [HttpGet("tree")]
        public async Task<IActionResult> GetTree([FromQuery] Guid? parentCode = null)
        {
            var tree = await _orgService.GetTreeAsync(parentCode);
            return Success(tree);
        }

        /// <summary>
        /// 获取指定父级下的直接子组织列表
        /// </summary>
        /// <param name="parentCode">父级组织 Code</param>
        /// <returns>子组织列表</returns>
        [HttpGet("by-parent/{parentCode}")]
        public async Task<IActionResult> GetByParent(Guid parentCode)
        {
            var children = await _orgService.GetByParentAsync(parentCode);
            return Success(children);
        }

        /// <summary>
        /// 获取组织详情
        /// </summary>
        /// <param name="code">组织 Code</param>
        /// <returns>组织详情</returns>
        [HttpGet("{code}")]
        public async Task<IActionResult> GetOrg(Guid code)
        {
            var org = await _orgService.GetByIdAsync(code);
            if (org == null)
                return Fail("组织不存在");

            return Success(org);
        }

        /// <summary>
        /// 创建组织
        /// </summary>
        /// <param name="request">创建请求</param>
        /// <returns>创建的组织信息</returns>
        [HttpPost]
        public async Task<IActionResult> CreateOrg([FromBody] CreateOrgRequest request)
        {
            try
            {
                var org = await _orgService.CreateAsync(request);
                return Success(org);
            }
            catch (InvalidOperationException ex)
            {
                return Fail(ex.Message);
            }
        }

        /// <summary>
        /// 更新组织
        /// </summary>
        /// <param name="code">组织 Code</param>
        /// <param name="request">更新请求</param>
        /// <returns>更新后的组织信息</returns>
        [HttpPut("{code}")]
        public async Task<IActionResult> UpdateOrg(Guid code, [FromBody] UpdateOrgRequest request)
        {
            if (code != request.Code)
                return Fail("URL 中的 Code 与请求体不一致");

            try
            {
                var org = await _orgService.UpdateAsync(request);
                return Success(org);
            }
            catch (InvalidOperationException ex)
            {
                return Fail(ex.Message);
            }
        }

        /// <summary>
        /// 软删除组织（仅允许删除没有下级组织的节点）
        /// </summary>
        /// <param name="code">组织 Code</param>
        /// <returns>无内容</returns>
        [HttpDelete("{code}")]
        public async Task<IActionResult> DeleteOrg(Guid code)
        {
            try
            {
                await _orgService.DeleteAsync(code);
                return Success();
            }
            catch (InvalidOperationException ex)
            {
                return Fail(ex.Message);
            }
        }

        /// <summary>
        /// 移动组织到新的父级下
        /// </summary>
        /// <param name="code">要移动的组织 Code</param>
        /// <param name="newParentCode">新的父级组织 Code（null 表示移动到根级）</param>
        /// <returns>无内容</returns>
        [HttpPut("{code}/move")]
        public async Task<IActionResult> MoveOrg(Guid code, [FromBody] Guid? newParentCode)
        {
            try
            {
                await _orgService.MoveAsync(code, newParentCode);
                return Success();
            }
            catch (InvalidOperationException ex)
            {
                return Fail(ex.Message);
            }
        }
    }
}
