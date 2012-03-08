using System;
using System.Linq.Expressions;
using NHibernate.Criterion;

namespace NHibernate.FlowQuery.Core
{
    public interface IExampleWrapper<TEntity>
    {
		#region Data Members (1) 

        Example Example { get; }

		#endregion Data Members 

		#region Operations (6) 

        IExampleWrapper<TEntity> EnableLike();

        IExampleWrapper<TEntity> EnableLike(MatchMode matchMode);

        IExampleWrapper<TEntity> ExcludeNulls();

        IExampleWrapper<TEntity> ExcludeProperty(string property);

        IExampleWrapper<TEntity> ExcludeProperty(Expression<Func<TEntity, object>> expression);

        IExampleWrapper<TEntity> ExcludeZeroes();

		#endregion Operations 
    }
}