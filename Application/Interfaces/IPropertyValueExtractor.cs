using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace dotnetcoreproject.Application.Interfaces;

public interface IPropertyValueExtractor
{
    List<object> ExtractValues<T>(Expression<Func<T, bool>> expression);
}