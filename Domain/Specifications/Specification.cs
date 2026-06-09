using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Domain.Interface.Specifications;

namespace Domain.Specifications
{
    /// <summary>
    /// 表示一个规范，用于定义实体的条件检查
    /// </summary>
    /// <typeparam name="T">实体类型</typeparam>
    public abstract class Specification<T> : ISpecification<T>
    {
        /// <summary>
        /// 通过 Lambda 表达式创建 Specification 类型的实例
        /// </summary>
        /// <param name="expression">表示规范条件的 Lambda 表达式</param>
        /// <returns>创建的 Specification 实例</returns>
        public static Specification<T> Eval(Expression<Func<T, bool>> expression)
        {
            return new ExpressionSpecification<T>(expression);
        }

        #region ISpecification<T> Members

        /// <summary>
        /// 实体的条件检查
        /// </summary>
        /// <param name="candidate">待检查的实体</param>
        /// <returns>实体是否满足规范条件</returns>
        public bool IsSatisfiedBy(T t)
        {
            // 编译并执行表达式，检查实体是否满足规范条件
            return this.Expression.Compile()(t);
        }

        /// <summary>
        /// 获取规范的 Lambda 表达式
        /// </summary>
        public abstract Expression<Func<T, bool>> Expression { get; }

        #endregion

        /// <summary>
        /// 组合当前规范与新的条件表达式
        /// </summary>
        /// <param name="expression">新的条件表达式</param>
        /// <returns>组合后的新规范</returns>
        public Specification<T> And(Expression<Func<T, bool>> expression)
        {
            // 使用 SpecExprExtensions.And 方法组合表达式
            var combinedExpression = this.Expression.And(expression);
            return new ExpressionSpecification<T>(combinedExpression);
        }

        /// <summary>
        /// 组合当前规范与另一个规范
        /// </summary>
        /// <param name="specification">另一个规范</param>
        /// <returns>组合后的新规范</returns>
        public Specification<T> And(Specification<T> specification)
        {
            // 使用 SpecExprExtensions.And 方法组合表达式
            var combinedExpression = this.Expression.And(specification.Expression);
            return new ExpressionSpecification<T>(combinedExpression);
        }
    }
}
