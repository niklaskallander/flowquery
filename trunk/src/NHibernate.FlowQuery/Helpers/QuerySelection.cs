using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NHibernate.Criterion;
using NHibernate.FlowQuery.Core;
using NHibernate.FlowQuery.Core.CustomProjections;
using NHibernate.FlowQuery.Core.Fetches;
using NHibernate.FlowQuery.Core.Joins;
using NHibernate.FlowQuery.Core.Locks;
using NHibernate.Metadata;
using NHibernate.Transform;

namespace NHibernate.FlowQuery.Helpers
{
    public class QuerySelection : IQueryableFlowQuery
    {
        protected QuerySelection(IQueryableFlowQuery query)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }

            Alias = query.Alias;
            Aliases = query.Aliases.ToDictionary(x => x.Key, x => x.Value);
            CacheMode = query.CacheMode;
            CacheRegion = query.CacheRegion;
            CommentValue = query.CommentValue;
            Constructor = query.Constructor;
            CriteriaFactory = query.CriteriaFactory;
            Criterions = query.Criterions.ToList();
            Fetches = query.Fetches.ToList();
            FetchSizeValue = query.FetchSizeValue;
            GroupBys = query.GroupBys.ToList();
            IsCacheable = query.IsCacheable;
            IsDelayed = query.IsDelayed;
            IsDistinct = query.IsDistinct;
            IsReadOnly = query.IsReadOnly;
            Joins = query.Joins.ToList();
            Locks = query.Locks.ToList();
            Mappings = query.Mappings == null
                ? null
                : query.Mappings.ToDictionary(x => x.Key, x => x.Value);
            MetaDataFactory = query.MetaDataFactory;
            Options = query.Options;
            Orders = query.Orders.ToList();
            Projection = query.Projection;
            ResultsToSkip = query.ResultsToSkip;
            ResultsToTake = query.ResultsToTake;
            ResultTransformer = query.ResultTransformer;
            TimeoutValue = query.TimeoutValue;
        }

        public static QuerySelection Create(IQueryableFlowQuery query)
        {
            return new QuerySelection(query);
        }

        public virtual string Alias { get; private set; }

        public bool IsCacheable { get; private set; }

        public CacheMode? CacheMode { get; private set; }

        public string CacheRegion { get; private set; }

        public virtual Dictionary<string, string> Aliases { get; private set; }

        public virtual string CommentValue { get; private set; }

        public virtual LambdaExpression Constructor { get; private set; }

        public virtual Func<System.Type, string, ICriteria> CriteriaFactory { get; private set; }

        public virtual List<ICriterion> Criterions { get; private set; }

        public virtual List<Fetch> Fetches { get; private set; }

        public int FetchSizeValue { get; private set; }

        public virtual bool IsDelayed { get; private set; }

        public virtual bool IsDistinct { get; private set; }

        public virtual bool? IsReadOnly { get; private set; }

        public virtual List<FqGroupByProjection> GroupBys { get; private set; }

        public virtual List<Join> Joins { get; private set; }

        public virtual List<Lock> Locks { get; private set; }

        public virtual Dictionary<string, IProjection> Mappings { get; private set; }

        public virtual Func<System.Type, IClassMetadata> MetaDataFactory { get; private set; }

        public virtual FlowQueryOptions Options { get; private set; }

        public virtual List<OrderByStatement> Orders { get; private set; }

        public virtual IProjection Projection { get; private set; }

        public virtual int? ResultsToSkip { get; private set; }

        public virtual int? ResultsToTake { get; private set; }

        public virtual IResultTransformer ResultTransformer { get; private set; }

        public virtual int? TimeoutValue { get; private set; }
    }
}
