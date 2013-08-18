using System.Collections.Generic;
using System.Linq.Expressions;
using NHibernate.Criterion;
using NHibernate.Transform;

namespace NHibernate.FlowQuery.Core
{
    public interface IMorphableFlowQuery : IFlowQuery
    {
        LambdaExpression Constructor { get; }

        bool IsDistinct { get; }

        Dictionary<string, IProjection> Mappings { get; }

        IProjection Projection { get; }

        IResultTransformer ResultTransformer { get; }
    }
}