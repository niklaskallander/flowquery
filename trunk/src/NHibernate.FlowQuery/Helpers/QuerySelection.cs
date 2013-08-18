using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NHibernate.Criterion;
using NHibernate.FlowQuery.Core;
using NHibernate.FlowQuery.Core.Joins;
using NHibernate.Transform;

namespace NHibernate.FlowQuery.Helpers
{
    public class QuerySelection : IQueryableFlowQuery
    {
        #region Constructor

        protected QuerySelection(IQueryableFlowQuery query)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }

            Alias = query.Alias;
            Aliases = query.Aliases.ToDictionary(x => x.Key, x => x.Value);
            Constructor = query.Constructor;
            CriteriaFactory = query.CriteriaFactory;
            Criterions = query.Criterions.ToList();
            IsDelayed = query.IsDelayed;
            IsDistinct = query.IsDistinct;
            Joins = query.Joins.ToList();
            Mappings = query.Mappings == null
                ? null
                : query.Mappings.ToDictionary(x => x.Key, x => x.Value);
            Options = query.Options;
            Orders = query.Orders.ToList();
            Projection = query.Projection;
            ResultsToSkip = query.ResultsToSkip;
            ResultsToTake = query.ResultsToTake;
            ResultTransformer = query.ResultTransformer;
        }

        public static QuerySelection Create(IQueryableFlowQuery query)
        {
            return new QuerySelection(query);
        }

        #endregion

        public virtual string Alias { get; private set; }

        public virtual Dictionary<string, string> Aliases { get; private set; }

        public virtual LambdaExpression Constructor { get; private set; }

        public virtual Func<System.Type, string, ICriteria> CriteriaFactory { get; private set; }

        public virtual List<ICriterion> Criterions { get; private set; }

        public virtual bool IsDelayed { get; private set; }

        public virtual bool IsDistinct { get; private set; }

        public virtual List<Join> Joins { get; private set; }

        public virtual Dictionary<string, IProjection> Mappings { get; private set; }

        public virtual FlowQueryOptions Options { get; private set; }

        public virtual List<OrderByStatement> Orders { get; private set; }

        public virtual IProjection Projection { get; private set; }

        public virtual int? ResultsToSkip { get; private set; }

        public virtual int? ResultsToTake { get; private set; }

        public virtual IResultTransformer ResultTransformer { get; private set; }
    }
}
