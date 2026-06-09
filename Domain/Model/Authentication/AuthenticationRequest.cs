using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model.Authentication
{
    public class AuthenticationRequest
    {
        /// <summary>
        /// 插件Key
        /// </summary>
        public string AuthType { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }
    }
}
