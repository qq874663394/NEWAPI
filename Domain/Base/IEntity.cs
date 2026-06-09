
namespace Domain.Interface.IAggregateRoots
{
    /// <summary>
    /// 普通实体类基类
    /// </summary>
    public interface IEntity
    {
        #region 通用属性
        /// <summary>
        /// 标识列
        /// </summary>
        Guid Code { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        DateTime? CreateTime { get; set; }
        /// <summary>
        /// 创建用户Code
        /// </summary>
        Guid? CreateUserCode { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        DateTime? ModifyTime { get; set; }
        /// <summary>
        /// 修改用户Code
        /// </summary>
        Guid? ModifyUserCode { get; set; }
        /// <summary>
        /// 是否启用，false否，true是，默认true
        /// </summary>
        bool IsEnable { get; set; }
        /// <summary>
        /// 是否删除，false否，true是，默认false
        /// </summary>
        bool IsDelete { get; set; }
        #endregion
    }
}
