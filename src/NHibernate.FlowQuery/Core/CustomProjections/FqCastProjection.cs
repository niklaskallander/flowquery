using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Criterion;
using NHibernate.Type;

namespace NHibernate.FlowQuery.Core.CustomProjections
{
    [Serializable]
    public class FqCastProjection : CastProjection
    {
        private readonly IProjection _projection;

        public FqCastProjection(IType type, IProjection projection)
            : base(type, projection)
        {
            _projection = projection;
        }

        public override bool IsAggregate
        {
            get
            {
                return _projection.IsAggregate;
            }
        }
    }
}