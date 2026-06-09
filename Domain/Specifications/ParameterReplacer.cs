using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Specifications
{
    // 辅助类，用于替换表达式中的参数
    public class ParameterReplacer : ExpressionVisitor
    {
        // 构造函数，接受要替换的参数表达式
        public ParameterReplacer(ParameterExpression paramExpr)
        {
            this.ParameterExpression = paramExpr;
        }

        // 要替换的参数表达式
        public ParameterExpression ParameterExpression { get; private set; }

        // 替换表达式中的参数
        public Expression Replace(Expression expr)
        {
            return this.Visit(expr);
        }

        // 当访问到参数表达式时，将其替换为指定的参数表达式
        protected override Expression VisitParameter(ParameterExpression p)
        {
            return this.ParameterExpression;
        }
    }
}
