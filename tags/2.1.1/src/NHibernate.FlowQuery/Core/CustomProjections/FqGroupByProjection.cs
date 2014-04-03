using System.Collections.Generic;
using NHibernate.Criterion;
using NHibernate.SqlCommand;
using NHibernate.Type;

namespace NHibernate.FlowQuery.Core.CustomProjections
{
    public class FqGroupByProjection : GroupedProjection
    {
        private readonly bool _includeInSelectList;

        public FqGroupByProjection(IProjection projection, bool includeInSelectList = true) 
            : base(projection)
        {
            _includeInSelectList = includeInSelectList;
        }

        public override SqlString ToSqlString(ICriteria criteria, int position, ICriteriaQuery criteriaQuery, IDictionary<string, IFilter> enabledFilters)
        {
            if (_includeInSelectList)
            {
                return base.ToSqlString(criteria, position, criteriaQuery, enabledFilters);
            }

            return new SqlString(string.Empty);
        }

        public override IType[] GetTypes(ICriteria criteria, ICriteriaQuery criteriaQuery)
        {
            if (_includeInSelectList)
            {
                return base.GetTypes(criteria, criteriaQuery);
            }

            return new IType[0];
        }

        public override IType[] GetTypes(string alias, ICriteria criteria, ICriteriaQuery criteriaQuery)
        {
            if (_includeInSelectList)
            {
                return base.GetTypes(alias, criteria, criteriaQuery);
            }

            return new IType[0];
        }

        public override string[] Aliases
        {
            get
            {
                if (_includeInSelectList)
                {
                    return base.Aliases;
                }

                return new string[0];
            }
        }

        public override string[] GetColumnAliases(int loc)
        {
            if (_includeInSelectList)
            {
                return base.GetColumnAliases(loc);
            }

            return new string[0];
        }

        public override string[] GetColumnAliases(string alias, int loc)
        {
            if (_includeInSelectList)
            {
                return base.GetColumnAliases(alias, loc);
            }

            return new string[0];
        }
    }
}