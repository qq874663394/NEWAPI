using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AuthApplication.Common;
using Infrastructure.Authentication.CurrentUser;
using Application.DTO.Route;
using Application.Interfaces.Route;

namespace AuthApplication.Controllers.SystemManage.Menu.Controllers
{
    /// <summary>
    /// 菜单管理控制器，提供菜单 CRUD 及树形查询接口
    /// </summary>
    [ApiController]
    [Route("api/routes")]
    [Authorize]
    public class RouteController : BaseController
    {
        private readonly IRouteService _routeService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="routeService">菜单管理服务</param>
        /// <param name="currentUser">当前用户信息</param>
        public RouteController(IRouteService routeService, ICurrentUser currentUser)
            : base(currentUser)
        {
            _routeService = routeService;
        }

        /// <summary>
        /// 获取菜单列表（支持关键字搜索）
        /// </summary>
        /// <param name="keyword">搜索关键字（可选）</param>
        /// <returns>菜单列表</returns>
        [HttpGet]
        public async Task<IActionResult> GetRoutes([FromQuery] string? keyword = null)
        {
            var routes = await _routeService.GetListAsync(keyword);
            return Success(routes);
        }

        /// <summary>
        /// 获取菜单树
        /// </summary>
        /// <param name="parentCode">可选，指定父级 Code 时返回以该节点为根的子树</param>
        /// <returns>菜单树</returns>
        [HttpGet("tree")]
        public async Task<IActionResult> GetTree([FromQuery] Guid? parentCode = null)
        {
            var tree = await _routeService.GetTreeAsync(parentCode);
            return Success(tree);
        }

        /// <summary>
        /// 获取菜单详情
        /// </summary>
        /// <param name="code">菜单 Code</param>
        /// <returns>菜单详情</returns>
        [HttpGet("{code}")]
        public async Task<IActionResult> GetRoute(Guid code)
        {
            var route = await _routeService.GetByIdAsync(code);
            if (route == null)
                return Fail("菜单不存在");

            return Success(route);
        }

        /// <summary>
        /// 创建菜单
        /// </summary>
        /// <param name="request">创建请求</param>
        /// <returns>创建的菜单信息</returns>
        [HttpPost]
        public async Task<IActionResult> CreateRoute([FromBody] CreateRouteRequest request)
        {
            try
            {
                var route = await _routeService.CreateAsync(request);
                return Success(route);
            }
            catch (InvalidOperationException ex)
            {
                return Fail(ex.Message);
            }
        }

        /// <summary>
        /// 更新菜单
        /// </summary>
        /// <param name="code">菜单 Code</param>
        /// <param name="request">更新请求</param>
        /// <returns>更新后的菜单信息</returns>
        [HttpPut("{code}")]
        public async Task<IActionResult> UpdateRoute(Guid code, [FromBody] UpdateRouteRequest request)
        {
            if (code != request.Code)
                return Fail("URL 中的 Code 与请求体不一致");

            try
            {
                var route = await _routeService.UpdateAsync(request);
                return Success(route);
            }
            catch (InvalidOperationException ex)
            {
                return Fail(ex.Message);
            }
        }

        /// <summary>
        /// 软删除菜单（仅允许删除没有下级菜单的节点）
        /// </summary>
        /// <param name="code">菜单 Code</param>
        /// <returns>无内容</returns>
        [HttpDelete("{code}")]
        public async Task<IActionResult> DeleteRoute(Guid code)
        {
            try
            {
                await _routeService.DeleteAsync(code);
                return Success();
            }
            catch (InvalidOperationException ex)
            {
                return Fail(ex.Message);
            }
        }
    }
}
