using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using NHibernate.FlowQuery.Revealing.Conventions;

namespace NHibernate.FlowQuery.Core.Joins
{
    public interface IJoinBuilder<TSource, TFlowQuery>
        where TSource : class
        where TFlowQuery : class, IFlowQuery<TSource, TFlowQuery>
    {
        TFlowQuery Join<TAlias>(string property, Expression<Func<TAlias>> alias);

        TFlowQuery Join<TAlias>(string property, Expression<Func<TAlias>> alias, Expression<Func<bool>> joinOnClause);

        TFlowQuery Join<TAlias>(Expression<Func<TSource, TAlias>> projection, Expression<Func<TAlias>> alias);

        TFlowQuery Join<TAlias>(Expression<Func<TSource, TAlias>> projection, Expression<Func<TAlias>> alias, Expression<Func<bool>> joinOnClause);

        TFlowQuery Join<TAlias>(Expression<Func<TSource, IEnumerable<TAlias>>> projection, Expression<Func<TAlias>> alias);

        TFlowQuery Join<TAlias>(Expression<Func<TSource, IEnumerable<TAlias>>> projection, Expression<Func<TAlias>> alias, Expression<Func<bool>> joinOnClause);

        TFlowQuery Join<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias);

        TFlowQuery Join<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias, Expression<Func<bool>> joinOnClause);

        TFlowQuery Join<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias, Expression<Func<bool>> joinOnClause, IRevealConvention revealConvention);

        TFlowQuery Join<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias, IRevealConvention revealConvention);
    }
}