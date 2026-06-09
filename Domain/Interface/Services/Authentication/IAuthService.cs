using Domain.Model.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interface.Services.Authentication
{
    /// <summary>
    /// 认证服务
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// 登录
        /// </summary>
        Task<LoginResult> LoginAsync(
            AuthenticationRequest request);

        /// <summary>
        /// 刷新Token
        /// </summary>
        string RefreshToken(string token);

        /// <summary>
        /// Token校验
        /// </summary>
        JwtToken? ValidateToken(string token);
    }
}
