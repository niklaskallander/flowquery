namespace NHibernate.FlowQuery.Test.FlowQuery.Core
{
    using System;

    using NHibernate.FlowQuery.Core;
    using NHibernate.FlowQuery.Core.Implementations;
    using NHibernate.FlowQuery.Test.Setup.Entities;

    using NUnit.Framework;

    using Is = NUnit.Framework.Is;

    [TestFixture]
    public class MorphabilityTest : BaseTest
    {
        [Test]
        public void CanMorphQueries()
        {
            IImmediateFlowQuery<UserEntity> immediateQuery = DummyQuery<UserEntity>();

            Assert.That(immediateQuery, Is.Not.Null);

            IDelayedFlowQuery<UserEntity> delayedQuery = immediateQuery
                .Delayed();

            Assert.That(delayedQuery, Is.Not.Null);

            IDetachedFlowQuery<UserEntity> detachedQuery = delayedQuery
                .Detached();

            Assert.That(detachedQuery, Is.Not.Null);

            immediateQuery = delayedQuery
                .Immediate();

            Assert.That(immediateQuery, Is.Not.Null);

            delayedQuery = detachedQuery
                .Delayed();

            Assert.That(delayedQuery, Is.Not.Null);

            immediateQuery = detachedQuery
                .Immediate();

            Assert.That(immediateQuery, Is.Not.Null);
        }

        [Test]
        public void TransformingTrulyDetachedQueryToImmediateThrowsWhenNotProvidingSession()
        {
            var query = new DummyDetachedQuery();

            Assert.That(query.CriteriaFactory, Is.Null);

            Assert.That(() => query.Immediate(), Throws.InvalidOperationException);
        }

        [Test]
        public void TransformingTrulyDetachedQueryToDelayedThrowsWhenNotProvidingSession()
        {
            var query = new DummyDetachedQuery();

            Assert.That(query.CriteriaFactory, Is.Null);

            Assert.That(() => query.Delayed(), Throws.InvalidOperationException);
        }

        [Test]
        public void TransformingTrulyDetachedQueryToImmediateThrowsWhenNotProvidingNullSession()
        {
            var query = new DummyDetachedQuery();

            Assert.That(query.CriteriaFactory, Is.Null);

            Assert.That(() => query.Immediate((ISession)null), Throws.InstanceOf<ArgumentNullException>());
        }

        [Test]
        public void TransformingTrulyDetachedQueryToDelayedThrowsWhenNotProvidingNullSession()
        {
            var query = new DummyDetachedQuery();

            Assert.That(query.CriteriaFactory, Is.Null);

            Assert.That(() => query.Delayed((ISession)null), Throws.InstanceOf<ArgumentNullException>());
        }

        [Test]
        public void TransformingTrulyDetachedQueryToImmediateThrowsWhenNotProvidingNullStatelessSession()
        {
            var query = new DummyDetachedQuery();

            Assert.That(query.CriteriaFactory, Is.Null);

            Assert.That(() => query.Immediate((IStatelessSession)null), Throws.InstanceOf<ArgumentNullException>());
        }

        [Test]
        public void TransformingTrulyDetachedQueryToDelayedThrowsWhenNotProvidingNullStatelessSession()
        {
            var query = new DummyDetachedQuery();

            Assert.That(query.CriteriaFactory, Is.Null);

            Assert.That(() => query.Delayed((IStatelessSession)null), Throws.InstanceOf<ArgumentNullException>());
        }

        [Test]
        public void TransformingTrulyDetachedQueryToImmediateDoesNotThrowWhenProvidingSession()
        {
            var query = new DummyDetachedQuery();

            Assert.That(query.CriteriaFactory, Is.Null);

            IImmediateFlowQuery<UserEntity> immediate = null;

            Assert.That(() => immediate = query.Immediate(Session), Throws.Nothing);

            Assert.That(immediate, Is.Not.Null);

            var queryInfo = immediate as IFlowQuery;

            Assert.That(queryInfo, Is.Not.Null);

            Assert.That(queryInfo.CriteriaFactory, Is.Not.Null);
        }

        [Test]
        public void TransformingTrulyDetachedQueryToDelayedDoesNotThrowWhenProvidingSession()
        {
            var query = new DummyDetachedQuery();

            Assert.That(query.CriteriaFactory, Is.Null);

            IDelayedFlowQuery<UserEntity> delayed = null;

            Assert.That(() => delayed = query.Delayed(Session), Throws.Nothing);

            Assert.That(delayed, Is.Not.Null);

            var queryInfo = delayed as IFlowQuery;

            Assert.That(queryInfo, Is.Not.Null);

            Assert.That(queryInfo.CriteriaFactory, Is.Not.Null);
        }

        [Test]
        public void TransformingTrulyDetachedQueryToImmediateDoesNotThrowWhenProvidingStatelessSession()
        {
            var query = new DummyDetachedQuery();

            Assert.That(query.CriteriaFactory, Is.Null);

            IImmediateFlowQuery<UserEntity> immediate = null;

            Assert.That(() => immediate = query.Immediate(StatelessSession), Throws.Nothing);

            Assert.That(immediate, Is.Not.Null);

            var queryInfo = immediate as IFlowQuery;

            Assert.That(queryInfo, Is.Not.Null);

            Assert.That(queryInfo.CriteriaFactory, Is.Not.Null);
        }

        [Test]
        public void TransformingTrulyDetachedQueryToDelayedDoesNotThrowWhenProvidingStatelessSession()
        {
            var query = new DummyDetachedQuery();

            Assert.That(query.CriteriaFactory, Is.Null);

            IDelayedFlowQuery<UserEntity> delayed = null;

            Assert.That(() => delayed = query.Delayed(StatelessSession), Throws.Nothing);

            Assert.That(delayed, Is.Not.Null);

            var queryInfo = delayed as IFlowQuery;

            Assert.That(queryInfo, Is.Not.Null);

            Assert.That(queryInfo.CriteriaFactory, Is.Not.Null);
        }

        private class DummyDetachedQuery : DetachedFlowQuery<UserEntity>
        {
            protected internal DummyDetachedQuery()
                : base(null)
            {
            }
        }
    }
}