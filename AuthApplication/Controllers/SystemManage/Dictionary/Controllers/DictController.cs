using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AuthApplication.Common;
using Infrastructure.Authentication.CurrentUser;
using Application.DTO.Dictionary;
using Application.Interfaces.Dictionary;

namespace AuthApplication.Controllers.SystemManage.Dictionary.Controllers
{
    /// <summary>
    /// 字典管理控制器，提供字典项 CRUD、树形查询及类别管理接口
    /// </summary>
    [ApiController]
    [Route("api/dict")]
    [Authorize]
    public class DictController : BaseController
    {
        private readonly IDictService _dictService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dictService">字典管理服务</param>
        /// <param name="currentUser">当前用户信息</param>
        public DictController(IDictService dictService, ICurrentUser currentUser)
            : base(currentUser)
        {
            _dictService = dictService;
        }

        /// <summary>
        /// 获取所有字典类别列表
        /// </summary>
        /// <returns>字典类别列表</returns>
        [HttpGet("types")]
        public async Task<IActionResult> GetTypes()
        {
            var types = await _dictService.GetTypesAsync();
            return Success(types);
        }

        /// <summary>
        /// 按类别获取字典树
        /// </summary>
        /// <param name="type">字典类别</param>
        /// <param name="parentCode">可选，指定父级 Code 时返回以该节点为根的子树</param>
        /// <returns>字典树</returns>
        [HttpGet("{type}/tree")]
        public async Task<IActionResult> GetTree(string type, [FromQuery] Guid? parentCode = null)
        {
            var tree = await _dictService.GetTreeAsync(type, parentCode);
            return Success(tree);
        }

        /// <summary>
        /// 按类别获取字典项平铺列表
        /// </summary>
        /// <param name="type">字典类别</param>
        /// <returns>字典项列表</returns>
        [HttpGet("{type}")]
        public async Task<IActionResult> GetByType(string type)
        {
            var items = await _dictService.GetByTypeAsync(type);
            return Success(items);
        }

        /// <summary>
        /// 获取字典项详情
        /// </summary>
        /// <param name="code">字典项 Code</param>
        /// <returns>字典项详情</returns>
        [HttpGet("item/{code}")]
        public async Task<IActionResult> GetDict(Guid code)
        {
            var item = await _dictService.GetByIdAsync(code);
            if (item == null)
                return Fail("字典项不存在");

            return Success(item);
        }

        /// <summary>
        /// 创建字典项
        /// </summary>
        /// <param name="request">创建请求</param>
        /// <returns>创建的字典项信息</returns>
        [HttpPost]
        public async Task<IActionResult> CreateDict([FromBody] CreateDictRequest request)
        {
            try
            {
                var item = await _dictService.CreateAsync(request);
                return Success(item);
            }
            catch (InvalidOperationException ex)
            {
                return Fail(ex.Message);
            }
        }

        /// <summary>
        /// 更新字典项
        /// </summary>
        /// <param name="code">字典项 Code</param>
        /// <param name="request">更新请求</param>
        /// <returns>更新后的字典项信息</returns>
        [HttpPut("item/{code}")]
        public async Task<IActionResult> UpdateDict(Guid code, [FromBody] UpdateDictRequest request)
        {
            if (code != request.Code)
                return Fail("URL 中的 Code 与请求体不一致");

            try
            {
                var item = await _dictService.UpdateAsync(request);
                return Success(item);
            }
            catch (InvalidOperationException ex)
            {
                return Fail(ex.Message);
            }
        }

        /// <summary>
        /// 删除字典项（仅允许删除没有子项的节点）
        /// </summary>
        /// <param name="code">字典项 Code</param>
        /// <returns>无内容</returns>
        [HttpDelete("item/{code}")]
        public async Task<IActionResult> DeleteDict(Guid code)
        {
            try
            {
                await _dictService.DeleteAsync(code);
                return Success();
            }
            catch (InvalidOperationException ex)
            {
                return Fail(ex.Message);
            }
        }
    }
}
