namespace NHibernate.FlowQuery.Test.FlowQuery.Core
{
    using System;

    using NHibernate.FlowQuery.Core;
    using NHibernate.FlowQuery.Core.Implementations;
    using NHibernate.FlowQuery.Test.Setup.Entities;

    using NUnit.Framework;

    [TestFixture]
    public class MorphabilityTest : BaseTest
    {
        [Test]
        public void CanMorphDelayedToDetached()
        {
            IDelayedFlowQuery<UserEntity> query = DummyQuery<UserEntity>()
                .Delayed();

            Assert.That(query, Is.Not.Null);

            IDetachedFlowQuery<UserEntity> morphed = query
                .Detached();

            Assert.That(morphed, Is.Not.Null);
        }

        [Test]
        public void CanMorphDelayedToImmediate()
        {
            IDelayedFlowQuery<UserEntity> query = DummyQuery<UserEntity>()
                .Delayed();

            Assert.That(query, Is.Not.Null);

            IImmediateFlowQuery<UserEntity> morphed = query
                .Immediate();

            Assert.That(morphed, Is.Not.Null);
        }

        [Test]
        public void CanMorphDelayedToStreamed()
        {
            IDelayedFlowQuery<UserEntity> query = DummyQuery<UserEntity>()
                .Delayed();

            Assert.That(query, Is.Not.Null);

            IStreamedFlowQuery<UserEntity> morphed = query
                .Streamed();

            Assert.That(morphed, Is.Not.Null);
        }

        [Test]
        public void CanMorphDetachedToDelayed()
        {
            IDetachedFlowQuery<UserEntity> query = DummyQuery<UserEntity>()
                .Detached();

            Assert.That(query, Is.Not.Null);

            IDelayedFlowQuery<UserEntity> morphed = query
                .Delayed();

            Assert.That(morphed, Is.Not.Null);
        }

        [Test]
        public void CanMorphDetachedToImmediate()
        {
            IDetachedFlowQuery<UserEntity> query = DummyQuery<UserEntity>()
                .Detached();

            Assert.That(query, Is.Not.Null);

            IImmediateFlowQuery<UserEntity> morphed = query
                .Immediate();

            Assert.That(morphed, Is.Not.Null);
        }

        [Test]
        public void CanMorphDetachedToStreamed()
        {
            IDetachedFlowQuery<UserEntity> query = DummyQuery<UserEntity>()
                .Detached();

            Assert.That(query, Is.Not.Null);

            IStreamedFlowQuery<UserEntity> morphed = query
                .Streamed();

            Assert.That(morphed, Is.Not.Null);
        }

        [Test]
        public void CanMorphImmediateToDelayed()
        {
            IImmediateFlowQuery<UserEntity> query = DummyQuery<UserEntity>();

            Assert.That(query, Is.Not.Null);

            IDelayedFlowQuery<UserEntity> morphed = query
                .Delayed();

            Assert.That(morphed, Is.Not.Null);
        }

        [Test]
        public void CanMorphImmediateToDetached()
        {
            IImmediateFlowQuery<UserEntity> query = DummyQuery<UserEntity>();

            Assert.That(query, Is.Not.Null);

            IDetachedFlowQuery<UserEntity> morphed = query
                .Detached();

            Assert.That(morphed, Is.Not.Null);
        }

        [Test]
        public void CanMorphImmediateToStreamed()
        {
            IImmediateFlowQuery<UserEntity> query = DummyQuery<UserEntity>();

            Assert.That(query, Is.Not.Null);

            IStreamedFlowQuery<UserEntity> morphed = query
                .Streamed();

            Assert.That(morphed, Is.Not.Null);
        }

        [Test]
        public void CanMorphStreamedToDelayed()
        {
            IStreamedFlowQuery<UserEntity> query = DummyQuery<UserEntity>()
                .Streamed();

            Assert.That(query, Is.Not.Null);

            IDelayedFlowQuery<UserEntity> morphed = query
                .Delayed();

            Assert.That(morphed, Is.Not.Null);
        }

        [Test]
        public void CanMorphStreamedToDetached()
        {
            IStreamedFlowQuery<UserEntity> query = DummyQuery<UserEntity>()
                .Streamed();

            Assert.That(query, Is.Not.Null);

            IDetachedFlowQuery<UserEntity> morphed = query
                .Detached();

            Assert.That(morphed, Is.Not.Null);
        }

        [Test]
        public void CanMorphStreamedToImmediate()
        {
            IStreamedFlowQuery<UserEntity> query = DummyQuery<UserEntity>()
                .Streamed();

            Assert.That(query, Is.Not.Null);

            IImmediateFlowQuery<UserEntity> morphed = query
                .Immediate();

            Assert.That(morphed, Is.Not.Null);
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

            if (queryInfo == null)
            {
                Assert.Fail("queryInfo was null");
            }
            else
            {
                Assert.That(queryInfo.CriteriaFactory, Is.Not.Null);
            }
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

            if (queryInfo == null)
            {
                Assert.Fail("queryInfo was null");
            }
            else
            {
                Assert.That(queryInfo.CriteriaFactory, Is.Not.Null);
            }
        }

        [Test]
        public void TransformingTrulyDetachedQueryToDelayedThrowsWhenNotProvidingNullSession()
        {
            var query = new DummyDetachedQuery();

            Assert.That(query.CriteriaFactory, Is.Null);

            Assert.That(() => query.Delayed((ISession)null), Throws.InstanceOf<ArgumentNullException>());
        }

        [Test]
        public void TransformingTrulyDetachedQueryToDelayedThrowsWhenNotProvidingNullStatelessSession()
        {
            var query = new DummyDetachedQuery();

            Assert.That(query.CriteriaFactory, Is.Null);

            Assert.That(() => query.Delayed((IStatelessSession)null), Throws.InstanceOf<ArgumentNullException>());
        }

        [Test]
        public void TransformingTrulyDetachedQueryToDelayedThrowsWhenNotProvidingSession()
        {
            var query = new DummyDetachedQuery();

            Assert.That(query.CriteriaFactory, Is.Null);

            Assert.That(() => query.Delayed(), Throws.InvalidOperationException);
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

            if (queryInfo == null)
            {
                Assert.Fail("queryInfo was null");
            }
            else
            {
                Assert.That(queryInfo.CriteriaFactory, Is.Not.Null);
            }
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

            if (queryInfo == null)
            {
                Assert.Fail("queryInfo was null");
            }
            else
            {
                Assert.That(queryInfo.CriteriaFactory, Is.Not.Null);
            }
        }

        [Test]
        public void TransformingTrulyDetachedQueryToImmediateThrowsWhenNotProvidingNullSession()
        {
            var query = new DummyDetachedQuery();

            Assert.That(query.CriteriaFactory, Is.Null);

            Assert.That(() => query.Immediate((ISession)null), Throws.InstanceOf<ArgumentNullException>());
        }

        [Test]
        public void TransformingTrulyDetachedQueryToImmediateThrowsWhenNotProvidingNullStatelessSession()
        {
            var query = new DummyDetachedQuery();

            Assert.That(query.CriteriaFactory, Is.Null);

            Assert.That(() => query.Immediate((IStatelessSession)null), Throws.InstanceOf<ArgumentNullException>());
        }

        [Test]
        public void TransformingTrulyDetachedQueryToImmediateThrowsWhenNotProvidingSession()
        {
            var query = new DummyDetachedQuery();

            Assert.That(query.CriteriaFactory, Is.Null);

            Assert.That(() => query.Immediate(), Throws.InvalidOperationException);
        }

        [Test]
        public void TransformingTrulyDetachedQueryToStreamedDoesNotThrowWhenProvidingSession()
        {
            var query = new DummyDetachedQuery();

            Assert.That(query.CriteriaFactory, Is.Null);

            IStreamedFlowQuery<UserEntity> streamed = null;

            Assert.That(() => streamed = query.Streamed(Session), Throws.Nothing);

            Assert.That(streamed, Is.Not.Null);

            var queryInfo = streamed as IFlowQuery;

            if (queryInfo == null)
            {
                Assert.Fail("queryInfo was null");
            }
            else
            {
                Assert.That(queryInfo.CriteriaFactory, Is.Not.Null);
            }
        }

        [Test]
        public void TransformingTrulyDetachedQueryToStreamedDoesNotThrowWhenProvidingStatelessSession()
        {
            var query = new DummyDetachedQuery();

            Assert.That(query.CriteriaFactory, Is.Null);

            IStreamedFlowQuery<UserEntity> streamed = null;

            Assert.That(() => streamed = query.Streamed(StatelessSession), Throws.Nothing);

            Assert.That(streamed, Is.Not.Null);

            var queryInfo = streamed as IFlowQuery;

            if (queryInfo == null)
            {
                Assert.Fail("queryInfo was null");
            }
            else
            {
                Assert.That(queryInfo.CriteriaFactory, Is.Not.Null);
            }
        }

        [Test]
        public void TransformingTrulyDetachedQueryToStreamedThrowsWhenNotProvidingSession()
        {
            var query = new DummyDetachedQuery();

            Assert.That(query.CriteriaFactory, Is.Null);

            Assert.That(() => query.Streamed(), Throws.InvalidOperationException);
        }

        [Test]
        public void TransformingTrulyDetachedQueryToStreamedThrowsWhenProvidingNullSession()
        {
            var query = new DummyDetachedQuery();

            Assert.That(query.CriteriaFactory, Is.Null);

            Assert.That(() => query.Streamed((ISession)null), Throws.InstanceOf<ArgumentNullException>());
        }

        [Test]
        public void TransformingTrulyDetachedQueryToStreamedThrowsWhenProvidingNullStatelessSession()
        {
            var query = new DummyDetachedQuery();

            Assert.That(query.CriteriaFactory, Is.Null);

            Assert.That(() => query.Streamed((IStatelessSession)null), Throws.InstanceOf<ArgumentNullException>());
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