﻿using System.Linq.Expressions;

namespace Core.Interfaces;

public interface ISpecification<T>
{
    // Where
    Expression<Func<T, bool>>? Criteria { get; }
    Expression<Func<T, object>>? OrderBy { get; }
    List<Expression<Func<T, object>>> Includes { get; }
    List<string> IncludeStrings { get; } //For ThenInclude
    Expression<Func<T, object>>? OrderByDescending { get; }
    bool IsDistinct { get; }

    int Take { get; }
    int Skip { get; }
    bool IsPagingEnabled { get; }

    IQueryable<T> ApplyCriteria(IQueryable<T> query);

}


public interface ISpecification<T, TResult> : ISpecification<T>
{
    Expression<Func<T, TResult>>? Select { get; }
}