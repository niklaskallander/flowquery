using NHibernate.Criterion;

namespace NHibernate.FlowQuery.Core.CustomProjections
{
    public class FqAliasProjection : AliasedProjection
    {
        public virtual IProjection Projection { get; private set; }

        public FqAliasProjection(IProjection projection, string alias) 
            : base(projection, alias)
        {
            Projection = projection;
        }
    }
}