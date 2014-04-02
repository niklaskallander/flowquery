using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using NHibernate.Criterion;
using NHibernate.Engine;
using NHibernate.Impl;
using NHibernate.SqlCommand;
using NHibernate.Type;

namespace NHibernate.FlowQuery.Core.CustomProjections
{
    public class FqProjectionList : IEnhancedProjection
    {
        private readonly IList<IProjection> _elements = new List<IProjection>();
        private readonly ProjectionList _list;

        public FqProjectionList()
        {
            _list = Projections.ProjectionList();
        }

        public IProjection this[int index]
        {
            get { return _elements[index]; }
        }

        public int Length
        {
            get { return _elements.Count; }
        }

        public string[] GetColumnAliases(string alias, int position, ICriteria criteria, ICriteriaQuery criteriaQuery)
        {
            return _list.GetColumnAliases(alias, position, criteria, criteriaQuery);
        }

        public string[] GetColumnAliases(int position, ICriteria criteria, ICriteriaQuery criteriaQuery)
        {
            return _list.GetColumnAliases(position, criteria, criteriaQuery);
        }

        public string[] Aliases
        {
            get { return _list.Aliases; }
        }

        public string[] GetColumnAliases(string alias, int loc)
        {
            return _list.GetColumnAliases(alias, loc);
        }

        public string[] GetColumnAliases(int loc)
        {
            return _list.GetColumnAliases(loc);
        }

        public TypedValue[] GetTypedValues(ICriteria criteria, ICriteriaQuery criteriaQuery)
        {
            return _list.GetTypedValues(criteria, criteriaQuery);
        }

        public IType[] GetTypes(string alias, ICriteria criteria, ICriteriaQuery criteriaQuery)
        {
            return _list.GetTypes(alias, criteria, criteriaQuery);
        }

        public IType[] GetTypes(ICriteria criteria, ICriteriaQuery criteriaQuery)
        {
            return _list.GetTypes(criteria, criteriaQuery);
        }

        public bool IsAggregate
        {
            get { return _list.IsAggregate; }
        }

        public bool IsGrouped
        {
            get { return _list.IsGrouped; }
        }

        public SqlString ToGroupSqlString(
            ICriteria criteria,
            ICriteriaQuery criteriaQuery,
            IDictionary<string, IFilter> enabledFilters)
        {
            return _list.ToGroupSqlString(criteria, criteriaQuery, enabledFilters);
        }

        public SqlString ToSqlString(
            ICriteria criteria,
            int position,
            ICriteriaQuery criteriaQuery,
            IDictionary<string, IFilter> enabledFilters)
        {
            var buffer = new SqlStringBuilder();

            bool lastHadValue = false;

            for (int i = 0; i < _list.Length; i++)
            {
                IProjection projection = this[i];

                SqlString value = projection.ToSqlString(criteria, position, criteriaQuery, enabledFilters);

                bool hasValue = value.Length > 0;

                if (hasValue)
                {
                    if (lastHadValue)
                    {
                        buffer.Add(", ");
                    }

                    string[] aliases = GetColumnAliases(position, criteria, criteriaQuery, projection);

                    position += aliases.Length;

                    buffer.Add(value);
                }

                lastHadValue = hasValue;
            }

            return buffer.ToSqlString();
        }

        public FqProjectionList Add(IProjection proj)
        {
            _elements.Add(proj);

            _list.Add(proj);

            return this;
        }

        public FqProjectionList Add(IProjection projection, String alias)
        {
            return Add(Projections.Alias(projection, alias));
        }

        public FqProjectionList Add<T>(IProjection projection, Expression<Func<T>> alias)
        {
            return Add(projection, ExpressionProcessor.FindMemberExpression(alias.Body));
        }

        private static string[] GetColumnAliases(
            int position,
            ICriteria criteria,
            ICriteriaQuery criteriaQuery,
            IProjection projection)
        {
            return projection is IEnhancedProjection
                       ? ((IEnhancedProjection)projection).GetColumnAliases(position, criteria, criteriaQuery)
                       : projection.GetColumnAliases(position);
        }
    }
}