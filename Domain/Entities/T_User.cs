using Domain.Interface.IAggregateRoots;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    /// <summary>
    /// 用户信息表
    /// </summary>
    public partial class T_User : Entity, IAggregateRoot
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// 用户APO，
        /// </summary>
        public string? Apo { get; set; }
        /// <summary>
        /// 用户全名
        /// </summary>
        public string? FullName { get; set; }
        /// <summary>
        /// 用户密码
        /// </summary>
        public string? Password { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public int? Sex { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string? Email { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string? Phone { get; set; } = string.Empty;
        /// <summary>
        /// 座机号
        /// </summary>
        public string? Tel { get; set; } = string.Empty;
        /// <summary>
        /// 出生日期
        /// </summary>
        public DateTime? DateOfBirth { get; set; }
        /// <summary>
        /// 证件类型
        /// </summary>
        public int? DocumentType { get; set; }
        /// <summary>
        /// 证件号
        /// </summary>
        public string? DocumentNumber { get; set; }
        /// <summary>
        /// 最后登录时间
        /// </summary>
        public DateTime? LastLoginTime { get; set; }
      
        /// <summary>
        /// 是否活动
        /// </summary>
        public bool? IsActive { get; set; } = true;

    }
}
