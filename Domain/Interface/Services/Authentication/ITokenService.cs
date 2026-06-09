using Domain.Model.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interface.Services.Authentication
{
    /// <summary>
    /// Token服务
    /// </summary>
    public interface ITokenService
    {
        /// <summary>
        /// 创建Token
        /// </summary>
        string CreateToken(Guid userId);

        /// <summary>
        /// 验证Token
        /// </summary>
        JwtToken? ValidateToken(string token);

        /// <summary>
        /// 刷新Token
        /// </summary>
        string RefreshToken(string token);
    }
}
