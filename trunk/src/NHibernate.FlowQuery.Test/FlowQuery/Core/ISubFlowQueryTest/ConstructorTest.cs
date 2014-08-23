namespace NHibernate.FlowQuery.Test.FlowQuery.Core.ISubFlowQueryTest
{
    using System;

    using NHibernate.Criterion;
    using NHibernate.FlowQuery.Core;
    using NHibernate.FlowQuery.Core.Implementations;
    using NHibernate.FlowQuery.Test.Setup.Entities;

    using NUnit.Framework;

    [TestFixture]
    public class ConstructorTest : BaseTest
    {
        [Test]
        public void DoesNotThrowIfCriteriaFactoryIsNull()
        {
            Assert
                .That
                (
                    () => new DummyDetachedQuery(null),
                    Throws.Nothing
                );
        }

        [Test]
        public void DoesNotThrowIfCriteriaFactoryIsNotNull()
        {
            Assert
                .That
                (
                    () => new DummyDetachedQuery(Session.CreateCriteria),
                    Throws.Nothing
                );
        }

        [Test]
        public void CanConstruct()
        {
            IDetachedFlowQuery<UserEntity> query = DetachedDummyQuery<UserEntity>();

            Assert.That(query, Is.Not.Null);
        }

        [Test]
        public void CanConstructFromDetachedCriteria()
        {
            IDetachedImmutableFlowQuery query = DetachedCriteria.For<UserEntity>()
                .DetachedFlowQuery();

            Assert.That(query, Is.Not.Null);
        }

        [Test]
        public void ConstructorThrowsIfDetachedCriteriaIsNull()
        {
            Assert
                .That(() => (null as DetachedCriteria).DetachedFlowQuery(), Throws.InstanceOf<ArgumentNullException>());
        }

        [Test]
        public void SetRootAliasThrowsIfExpressionIsNull()
        {
            Assert
                .That
                (
                    () => DetachedDummyQuery<UserEntity>().SetRootAlias<UserEntity>(null),
                    Throws.InstanceOf<ArgumentNullException>()
                );
        }

        [Test]
        public void DetachedImmutableThrowsIfCriteriaIsNull()
        {
            Assert
                .That
                (
                    () => new DummyDetachedImmutableQuery(null),
                    Throws.InstanceOf<ArgumentNullException>()
                );
        }

        private class DummyDetachedQuery : DetachedFlowQuery<UserEntity>
        {
            protected internal DummyDetachedQuery(Func<Type, string, ICriteria> criteriaFactory)
                : base(criteriaFactory)
            {
            }
        }

        private class DummyDetachedImmutableQuery : DetachedImmutableFlowQuery
        {
            protected internal DummyDetachedImmutableQuery(DetachedCriteria criteria)
                : base(criteria)
            {
            }
        }
    }
}