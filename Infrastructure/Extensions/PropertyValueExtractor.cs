using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using dotnetcoreproject.Application.Interfaces;

namespace dotnetcoreproject.Infrastructure.Extensions;

public class PropertyValueExtractor : ExpressionVisitor, IPropertyValueExtractor
{
    private readonly List<object> _values = new();
    private object? _currentConstant;

    public List<object> ExtractValues<T>(Expression<Func<T, bool>> expression)
    {
        Visit(expression);
        return _values;
    }

    protected override Expression VisitBinary(BinaryExpression node)
    {
        Visit(node.Left);
        Visit(node.Right);
        return node;
    }

    protected override Expression VisitMember(MemberExpression node)
    {
        if (node.Expression != null && node.Expression.NodeType == ExpressionType.Constant)
        {
            var container = ((ConstantExpression)node.Expression).Value;
            if (container != null)
            {
                var member = node.Member;

                if (member is FieldInfo field)
                {
                    var value = field.GetValue(container);
                    if (value != null)
                        _values.Add(value);
                }
                else if (member is PropertyInfo property)
                {
                    var value = property.GetValue(container);
                    if (value != null)
                        _values.Add(value);
                }
            }
        }
        else if (node.Expression != null && node.Expression.NodeType == ExpressionType.MemberAccess)
        {
            var container = ExtractValue(node.Expression as MemberExpression);
            if (container != null)
            {
                var member = node.Member;

                if (member is FieldInfo field)
                {
                    var value = field.GetValue(container);
                    if (value != null)
                        _values.Add(value);
                }
                else if (member is PropertyInfo property)
                {
                    var value = property.GetValue(container);
                    if (value != null)
                        _values.Add(value);
                }
            }
        }

        return node;
    }

    protected override Expression VisitConstant(ConstantExpression node)
    {
        _currentConstant = node.Value;
        if (node.Value != null)
            _values.Add(node.Value);
        return node;
    }

    private object? ExtractValue(MemberExpression? node)
    {
        if (node == null) return null;

        if (node.Expression is ConstantExpression constantExpression)
        {
            var container = constantExpression.Value;
            var member = node.Member;

            if (member is FieldInfo field)
                return field.GetValue(container);
            if (member is PropertyInfo property) return property.GetValue(container);
        }
        else if (node.Expression is MemberExpression innerMemberExpression)
        {
            var container = ExtractValue(innerMemberExpression);
            var member = node.Member;

            if (member is FieldInfo field)
                return field.GetValue(container);
            if (member is PropertyInfo property) return property.GetValue(container);
        }

        return null;
    }
}