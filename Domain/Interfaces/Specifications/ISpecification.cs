using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Specifications
{
    /// <summary>
    /// 表示一个规范接口，用于定义实体的条件检查和获取规范的 Lambda 表达式。
    /// </summary>
    /// <typeparam name="T">实体类型</typeparam>
    public interface ISpecification<T>
    {
        /// <summary>
        /// 实体的条件检查
        /// </summary>
        /// <param name="candidate">待检查的实体</param>
        /// <returns>实体是否满足规范条件</returns>
        bool IsSatisfiedBy(T t);

        /// <summary>
        /// 获取规范的 Lambda 表达式
        /// </summary>
        Expression<Func<T, bool>> Expression { get; }
    }
}
