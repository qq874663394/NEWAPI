using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AuthApplication.Common;
using Infrastructure.Authentication.CurrentUser;
using Application.DTO.Auth;
using Application.Interfaces.Auth;

namespace AuthApplication.Controllers.SystemManage.Auth.Controllers
{
    /// <summary>
    /// 认证控制器，提供登录、Token 刷新、用户信息等接口
    /// </summary>
    [ApiController]
    [Route("api/auth")]
    public class AuthController : BaseController
    {
        private readonly IAuthService _authService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="authService">认证服务</param>
        /// <param name="currentUser">当前用户信息</param>
        public AuthController(IAuthService authService, ICurrentUser currentUser)
            : base(currentUser)
        {
            _authService = authService;
        }

        /// <summary>
        /// 用户登录（支持密码登录和 APO 登录）
        /// </summary>
        /// <param name="request">认证请求，包含用户名、密码及认证类型</param>
        /// <returns>登录结果，含 AccessToken 和 RefreshToken</returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] AuthenticationRequest request)
        {
            var result = await _authService.LoginAsync(request);

            if (result == null)
                return Fail("用户名或密码错误");

            return Success(result);
        }

        /// <summary>
        /// 刷新 Access Token
        /// </summary>
        /// <param name="request">刷新请求，包含 RefreshToken</param>
        /// <returns>新的 AccessToken</returns>
        [HttpPost("refresh")]
        [AllowAnonymous]
        public IActionResult Refresh([FromBody] RefreshRequest request)
        {
            try
            {
                var newAccessToken = _authService.RefreshToken(request.RefreshToken);
                return Success(new { AccessToken = newAccessToken });
            }
            catch (Exception ex)
            {
                return Fail(ex.Message);
            }
        }

        /// <summary>
        /// 获取当前登录用户信息（需登录）
        /// </summary>
        /// <returns>当前用户的基本信息</returns>
        [HttpGet("me")]
        public IActionResult GetCurrentUser()
        {
            var userInfo = new
            {
                Code = CurrentUser.Code,
                Name = CurrentUser.Name,
                FullName = CurrentUser.FullName,
                IsAuthenticated = CurrentUser.IsAuthenticated
            };
            return Success(userInfo);
        }
    }
}
