using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

public static class IncludeExtensions
{
    public static IQueryable<T> IncludeProperties<T>(this IQueryable<T> query, bool asNoTracking,
        params Expression<Func<T, object>>[] includes) where T : class
    {
        if (asNoTracking) query = query.AsNoTracking();

        if (includes != null && includes.Any())
            foreach (var include in includes)
                query = IncludeProperty(query, include);

        return query;
    }

    private static IQueryable<T> IncludeProperty<T>(IQueryable<T> query, Expression<Func<T, object>> include)
        where T : class
    {
        var visitor = new IncludeVisitor<T>(query);
        visitor.Visit(include);
        return visitor.Query;
    }

    private class IncludeVisitor<T> : ExpressionVisitor where T : class
    {
        public IncludeVisitor(IQueryable<T> query)
        {
            Query = query;
        }

        public IQueryable<T> Query { get; private set; }

        protected override Expression VisitMember(MemberExpression node)
        {
            if (node.Expression is MemberExpression)
            {
                var path = GetPropertyPath(node);
                Query = Query.Include(path);
            }

            return base.VisitMember(node);
        }

        private string GetPropertyPath(MemberExpression node)
        {
            var path = node.Member.Name;
            while (node.Expression is MemberExpression parent)
            {
                path = parent.Member.Name + "." + path;
                node = parent;
            }

            return path;
        }
    }
}