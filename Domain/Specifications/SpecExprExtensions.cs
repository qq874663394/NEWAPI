using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Specifications
{
    //表达式增强类，实现Not、And、Or方法
    public static class SpecExprExtensions
    {
        // 实现逻辑非（NOT）操作的方法
        public static Expression<Func<T, bool>> Not<T>(this Expression<Func<T, bool>> one)
        {
            // 获取参数表达式
            var candidateExpr = one.Parameters[0];
            // 对主体表达式进行逻辑非操作
            var body = Expression.Not(one.Body);
            // 创建新的 lambda 表达式，并返回
            return Expression.Lambda<Func<T, bool>>(body, candidateExpr);
        }

        // 实现逻辑与（AND）操作的方法
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
        {
            if (expr1 == null) return expr2;
            if (expr2 == null) return expr1;

            var parameter = Expression.Parameter(typeof(T), "x");

            var leftVisitor = new ReplaceExpressionVisitor(expr1.Parameters[0], parameter);
            var left = leftVisitor.Visit(expr1.Body);

            var rightVisitor = new ReplaceExpressionVisitor(expr2.Parameters[0], parameter);
            var right = rightVisitor.Visit(expr2.Body);

            return Expression.Lambda<Func<T, bool>>(Expression.AndAlso(left, right), parameter);
        }

        // 实现逻辑或（OR）操作的方法
        public static Expression<Func<T, bool>> Or<T>(
            this Expression<Func<T, bool>> one,
            Expression<Func<T, bool>> another)
        {
            // 首先定义好一个 ParameterExpression
            var candidateExpr = Expression.Parameter(typeof(T), "candidate");
            // 创建一个 ParameterReplacer 实例，用于替换表达式中的参数
            var parameterReplacer = new ParameterReplacer(candidateExpr);

            // 将表达式树的参数统一替换成我们定义好的 candidateExpr
            var left = parameterReplacer.Replace(one.Body);
            var right = parameterReplacer.Replace(another.Body);
            // 构建新的表达式树，表示逻辑或操作
            var body = Expression.Or(left, right);

            // 创建新的 lambda 表达式，并返回
            return Expression.Lambda<Func<T, bool>>(body, candidateExpr);
        }
    }
    internal class ReplaceExpressionVisitor : ExpressionVisitor
    {
        private readonly Expression _oldValue;
        private readonly Expression _newValue;

        public ReplaceExpressionVisitor(Expression oldValue, Expression newValue)
        {
            _oldValue = oldValue;
            _newValue = newValue;
        }

        public override Expression Visit(Expression node) => node == _oldValue ? _newValue : base.Visit(node);
    }
}
