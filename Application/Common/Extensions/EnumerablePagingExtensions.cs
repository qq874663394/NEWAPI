using Application.Options;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Extensions
{
    /// <summary>
    /// 内存分页扩展（适用于已加载到内存的集合）
    /// </summary>
    public static class QueryableExtensions
    {
        /// <summary>
        /// 数据库分页 + 自定义投影（一次性查询，含关联数据）
        /// </summary>
        public static async Task<PagedResult<TResult>> ToPagedResultAsync<TEntity, TResult>(
            this IQueryable<TEntity> source,
            int pageIndex,
            int pageSize,
            Expression<Func<TEntity, TResult>> selector,
            CancellationToken cancellationToken = default)
        {
            var totalCount = await source.CountAsync(cancellationToken);
            pageIndex = Math.Max(1, pageIndex);
            pageSize = Math.Clamp(pageSize, 1, 100);

            var items = await source
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .Select(selector)
                .ToListAsync(cancellationToken);

            return new PagedResult<TResult>(items, totalCount, pageIndex, pageSize);
        }

        // 如果不需要投影，直接返回实体本身
        public static async Task<PagedResult<T>> ToPagedResultAsync<T>(
            this IQueryable<T> source,
            int pageIndex,
            int pageSize,
            CancellationToken cancellationToken = default)
        {
            var totalCount = await source.CountAsync(cancellationToken);
            pageIndex = Math.Max(1, pageIndex);
            pageSize = Math.Clamp(pageSize, 1, 100);

            var items = await source
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return new PagedResult<T>(items, totalCount, pageIndex, pageSize);
        }
    }
}
