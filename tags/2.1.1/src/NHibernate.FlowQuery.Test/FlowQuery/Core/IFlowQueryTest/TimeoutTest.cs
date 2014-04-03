using NHibernate.FlowQuery.Core;
using NHibernate.FlowQuery.Helpers;
using NHibernate.FlowQuery.Test.Setup.Entities;
using NHibernate.Impl;
using NUnit.Framework;

namespace NHibernate.FlowQuery.Test.FlowQuery.Core.IFlowQueryTest
{
    using Is = NUnit.Framework.Is;

    [TestFixture]
    public class TimeoutTest : BaseTest
    {
        [Test]
        public void CanSetTimeoutOnQuery()
        {
            var query = DummyQuery<UserEntity>()
                .Timeout(10);

            var queryable = (IQueryableFlowQuery)query;

            Assert.That(queryable.TimeoutValue, Is.EqualTo(10));
        }

        [Test]
        public void CanClearTimeoutOnQuery()
        {
            var query = DummyQuery<UserEntity>()
                .Timeout(10);

            var queryable = (IQueryableFlowQuery)query;

            Assert.That(queryable.TimeoutValue, Is.EqualTo(10));

            query.ClearTimeout();

            Assert.That(queryable.TimeoutValue, Is.Null);
        }

        [Test]
        public void SpecifiedTimeoutIsUsedOnCriteria()
        {
            var query = Query<UserEntity>()
                .Timeout(10);

            ICriteria criteria = CriteriaHelper.BuildCriteria<UserEntity, UserEntity>(QuerySelection.Create((IQueryableFlowQuery)query));

            Assert.That(criteria, Is.Not.Null);

            var impl = (CriteriaImpl)criteria;

            Assert.That(impl.Timeout, Is.EqualTo(10));
        }
    }
}