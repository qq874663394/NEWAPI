using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Specifications
{
    /// <summary>
    /// 表示一个永远为真的规范，即匹配任何实体
    /// </summary>
    /// <typeparam name="T">实体类型</typeparam>
    public sealed class AnySpecification<T> : Specification<T>
    {
        /// <summary>
        /// 获取规范的表达式，始终返回true
        /// </summary>
        public override Expression<Func<T, bool>> Expression
        {
            // 返回一个始终为 true 的 lambda 表达式
            get { return o => true; }
        }
    }
}
