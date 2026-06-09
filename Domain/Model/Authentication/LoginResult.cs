using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model.Authentication
{
    /// <summary>
    /// 登录结果
    /// </summary>
    public class LoginResult
    {
        /// <summary>
        /// Jwt Token
        /// </summary>
        public string Token { get; set; }
            = string.Empty;

        /// <summary>
        /// 用户Code
        /// </summary>
        public Guid UserCode { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
            = string.Empty;

        /// <summary>
        /// 用户全名
        /// </summary>
        public string? FullName { get; set; }

        /// <summary>
        /// APO
        /// </summary>
        public string? Apo { get; set; }
    }
}
