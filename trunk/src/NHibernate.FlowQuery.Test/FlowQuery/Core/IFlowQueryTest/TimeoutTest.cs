namespace NHibernate.FlowQuery.Test.FlowQuery.Core.IFlowQueryTest
{
    using NHibernate.FlowQuery.Core;
    using NHibernate.FlowQuery.Core.Implementations;
    using NHibernate.FlowQuery.Helpers;
    using NHibernate.FlowQuery.Test.Setup.Entities;
    using NHibernate.Impl;

    using NUnit.Framework;

    [TestFixture]
    public class TimeoutTest : BaseTest
    {
        [Test]
        public void CanClearTimeoutOnQuery()
        {
            IImmediateFlowQuery<UserEntity> query = DummyQuery<UserEntity>()
                .Timeout(10);

            var queryable = (IQueryableFlowQuery)query;

            Assert.That(queryable.TimeoutValue, Is.EqualTo(10));

            query.ClearTimeout();

            Assert.That(queryable.TimeoutValue, Is.Null);
        }

        [Test]
        public void CanSetTimeoutOnQuery()
        {
            IImmediateFlowQuery<UserEntity> query = DummyQuery<UserEntity>()
                .Timeout(10);

            var queryable = (IQueryableFlowQuery)query;

            Assert.That(queryable.TimeoutValue, Is.EqualTo(10));
        }

        [Test]
        public void SpecifiedTimeoutIsUsedOnCriteria()
        {
            IImmediateFlowQuery<UserEntity> query = Query<UserEntity>()
                .Timeout(10);

            ICriteria criteria = new CriteriaBuilder()
                .Build<UserEntity, UserEntity>(QuerySelection.Create((IQueryableFlowQuery)query));

            Assert.That(criteria, Is.Not.Null);

            var impl = (CriteriaImpl)criteria;

            Assert.That(impl.Timeout, Is.EqualTo(10));
        }
    }
}