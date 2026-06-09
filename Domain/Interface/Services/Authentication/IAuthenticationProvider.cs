using Domain.Entities;
using Domain.Model.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interface.Services.Authentication
{
    /// <summary>
    /// 登录请求
    /// </summary>
    public interface IAuthenticationProvider
    {
        /// <summary>
        /// 插件唯一标识（替代 enum）
        /// </summary>
        string Key { get; }

        /// <summary>
        /// 登录
        /// </summary>
        Task<T_User?> AuthenticateAsync(AuthenticationRequest request);
    }
}
