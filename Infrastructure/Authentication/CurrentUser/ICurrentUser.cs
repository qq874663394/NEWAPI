using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Authentication.CurrentUser
{
    /// <summary>
    /// 当前登录用户
    /// </summary>
    public interface ICurrentUser
    {
        /// <summary>
        /// 是否已登录
        /// </summary>
        bool IsAuthenticated { get; }

        /// <summary>
        /// 用户Code
        /// </summary>
        string? Code { get; }

        /// <summary>
        /// 登录账号
        /// </summary>
        string? Name { get; }

        /// <summary>
        /// 全名
        /// </summary>
        string? FullName { get; }

        /// <summary>
        /// 当前Claims
        /// </summary>
        IEnumerable<Claim> Claims { get; }

        /// <summary>
        /// 判断是否拥有指定角色
        /// </summary>
        bool IsInRole(string role);
    }
}
