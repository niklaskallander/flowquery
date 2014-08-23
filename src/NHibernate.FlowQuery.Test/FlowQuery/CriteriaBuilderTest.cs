namespace NHibernate.FlowQuery.Test.FlowQuery
{
    using System;

    using NHibernate.Criterion;
    using NHibernate.FlowQuery.Core;
    using NHibernate.FlowQuery.Core.Implementations;
    using NHibernate.FlowQuery.Test.Setup.Entities;

    using NUnit.Framework;

    [TestFixture]
    public class CriteriaBuilderTest : BaseTest
    {
        [Test]
        public void CanSetCriteriaBuilderOnPerQueryOptionsUsingImmediateQuery()
        {
            var options = new FlowQueryOptions
            {
                CriteriaBuilder = new DummyCriteriaBuilder()
            };

            Assert
                .That
                (
                    () => Session.ImmediateFlowQuery<UserEntity>(options).Select(),
                    Throws.InstanceOf<NotImplementedException>()
                );
        }

        [Test]
        public void CanSetCriteriaBuilderOnPerQueryOptionsUsingDelayedQuery()
        {
            var options = new FlowQueryOptions
            {
                CriteriaBuilder = new DummyCriteriaBuilder()
            };

            Assert
                .That
                (
                    () => Session.DelayedFlowQuery<UserEntity>(options).Select(),
                    Throws.InstanceOf<NotImplementedException>()
                );
        }

        [Test]
        public void CanSetCriteriaBuilderOnPerQueryOptionsUsingDetachedQuery()
        {
            var options = new FlowQueryOptions
            {
                CriteriaBuilder = new DummyCriteriaBuilder()
            };

            var query = Session.DetachedFlowQuery<UserEntity>(options)
                .Where(x => x.Id == 1)
                .Select(x => x.Id);

            Assert
                .That
                (
                    () => query.Criteria,
                    Throws.InstanceOf<NotImplementedException>()
                );
        }

        [Test]
        public void CanSetCriteriaBuilderOnGlobalOptionsUsingImmediateQuery()
        {
            FlowQueryOptions.GlobalCriteriaBuilder = new DummyCriteriaBuilder();

            Assert
                .That
                (
                    () => Session.ImmediateFlowQuery<UserEntity>().Select(),
                    Throws.InstanceOf<NotImplementedException>()
                );

            FlowQueryOptions.GlobalCriteriaBuilder = null;
        }

        [Test]
        public void CanSetCriteriaBuilderOnGlobalOptionsUsingDelayedQuery()
        {
            FlowQueryOptions.GlobalCriteriaBuilder = new DummyCriteriaBuilder();

            Assert
                .That
                (
                    () => Session.DelayedFlowQuery<UserEntity>().Select(),
                    Throws.InstanceOf<NotImplementedException>()
                );

            FlowQueryOptions.GlobalCriteriaBuilder = null;
        }

        [Test]
        public void CanSetCriteriaBuilderOnGlobalOptionsUsingDetachedQuery()
        {
            FlowQueryOptions.GlobalCriteriaBuilder = new DummyCriteriaBuilder();

            var query = Session.DetachedFlowQuery<UserEntity>()
                .Where(x => x.Id == 1)
                .Select(x => x.Id);

            Assert
                .That
                (
                    () => query.Criteria,
                    Throws.InstanceOf<NotImplementedException>()
                );

            FlowQueryOptions.GlobalCriteriaBuilder = null;
        }

        private class DummyCriteriaBuilder : CriteriaBuilder
        {
            public override ICriteria Build<TSource, TDestination>(IQueryableFlowQuery query)
            {
                throw new NotImplementedException();
            }

            public override DetachedCriteria Build<TSource>(IMorphableFlowQuery query)
            {
                throw new NotImplementedException();
            }
        }
    }
}