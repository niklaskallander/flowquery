using System;
namespace NHibernate.FlowQuery.Core
{
    public class SelectDistinctSetup<TSource, TReturn> : SelectSetup<TSource, TReturn>
        where TSource : class
    {
        public SelectDistinctSetup(IFlowQuery<TSource> query)
            : base(query)
        { }

        protected override FlowQuerySelection<TReturn> Select()
        {
            if (ProjectionList.Length == 0)
            {
                throw new InvalidOperationException("No setup has been made");
            }

            return Query.SelectDistinct<TReturn>(this);
        }
    }
}