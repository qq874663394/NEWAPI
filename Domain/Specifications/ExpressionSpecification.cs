using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Specifications
{
    /// <summary>
    /// 表达式规范，表示一个基于表达式树的规范
    /// </summary>
    /// <typeparam name="T">实体类型</typeparam>
    public sealed class ExpressionSpecification<T> : Specification<T>
    {
        private readonly Expression<Func<T, bool>> _expression;
        /// <summary>
        /// 构造函数，接受一个表示规范条件的表达式
        /// </summary>
        /// <param name="expression">表示规范条件的表达式</param>
        public ExpressionSpecification(Expression<Func<T, bool>> expression)
        {
            this._expression = expression ?? throw new ArgumentNullException(nameof(expression));
        }

        /// <summary>
        /// 获取规范的表达式
        /// </summary>
        public override Expression<Func<T, bool>> Expression
        {
            get { return _expression; }
        }
    }
}
