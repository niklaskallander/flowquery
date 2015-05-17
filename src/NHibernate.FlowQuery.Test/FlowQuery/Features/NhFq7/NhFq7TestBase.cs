namespace NHibernate.FlowQuery.Test.FlowQuery.Features.NhFq7
{
    using System;
    using System.Reflection;

    using NHibernate.FlowQuery.Core;
    using NHibernate.SqlCommand;

    using NUnit.Framework;

    public class NhFq7TestBase : BaseTest
    {
        protected static IQueryFilter<T> For<T>
            (
            Action<IFilterableQuery<T>, int> action,
            int value
            )
        {
            return new QueryFilter<T>(query => action(query, value));
        }

        protected static JoinType GetJoinTypeFrom<T>(IJoinBuilder<T, IFilterableQuery<T>> builder)
        {
            var type = builder.GetType();

            var field = type.GetField("_joinBuilder", BindingFlags.NonPublic | BindingFlags.Instance);

            Assert.That(field != null);

            var innerBuilder = field.GetValue(builder);

            Assert.That(innerBuilder != null);

            var innerType = innerBuilder.GetType();

            var innerField = innerType.GetField("_joinType", BindingFlags.NonPublic | BindingFlags.Instance);

            Assert.That(innerField != null);

            var joinType = innerField.GetValue(innerBuilder);

            Assert.That(joinType != null);

            return (JoinType)joinType;
        }
    }
}