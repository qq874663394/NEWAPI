using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model.Authentication
{
    public class JwtToken
    {
        /// <summary>
        /// 用户Code
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// 是否过期
        /// </summary>
        public bool IsExpired { get; set; }
    }
}
