using System;
using NHibernate.Criterion;
using NHibernate.FlowQuery.Core;
using NHibernate.FlowQuery.Helpers;
using NHibernate.FlowQuery.Test.FlowQuery.Helpers;
using NHibernate.FlowQuery.Test.Setup.Entities;
using NUnit.Framework;

// ReSharper disable ExpressionIsAlwaysNull
namespace NHibernate.FlowQuery.Test.FlowQuery.Core.IFlowQueryTest
{
    using Is = NUnit.Framework.Is;

    [TestFixture]
    public class ConstructorTest : BaseTest
    {
        [Test]
        public void ThrowsIfMetaDataFactoryIsNull()
        {
            Assert.That(() => new DummyQuery2(Session.CreateCriteria, null), Throws.InstanceOf<ArgumentNullException>());
        }

        [Test]
        public void CanCreateFlowQuery()
        {
            var q = Session.FlowQuery<UserEntity>();

            Assert.That(q, Is.Not.Null);
        }

        [Test]
        public void CanCreateFlowQueryWithAlias()
        {
            UserEntity user = null;

            var q = Session.FlowQuery(() => user);

            Assert.That(q, Is.Not.Null);
        }

        [Test]
        public void CanCreateFlowQueryWithOptions()
        {
            var options = new FlowQueryOptions()
                .Add(c => c.SetMaxResults(5));

            var q = Session.FlowQuery<UserEntity>(options);

            Assert.That(q, Is.Not.Null);

            ICriteria criteria = CriteriaHelper.BuildCriteria<UserEntity, UserEntity>(QuerySelection.Create(q as IQueryableFlowQuery));

            Assert.That(criteria.GetRootEntityTypeIfAvailable(), Is.EqualTo(typeof(UserEntity)));
        }

        [Test]
        public void CanCreateFlowQueryWithAliasAndOptions()
        {
            UserEntity user = null;

            var q = Session.FlowQuery(() => user, new FlowQueryOptions());

            Assert.That(q, Is.Not.Null);
        }

        [Test]
        public void DoesNotThrowWhenOptionsIsNull()
        {
            FlowQueryOptions options = null;

            Assert.That(() => Session.FlowQuery<UserEntity>(options), Throws.Nothing);
        }

        [Test]
        public void CanCreateImmediateFlowQuery()
        {
            var q = Session.ImmediateFlowQuery<UserEntity>();

            Assert.That(q, Is.Not.Null);
        }

        [Test]
        public void CanCreateImmediateFlowQueryWithAlias()
        {
            UserEntity user = null;

            var q = Session.ImmediateFlowQuery(() => user);

            Assert.That(q, Is.Not.Null);
        }

        [Test]
        public void CanCreateImmediateFlowQueryWithOptions()
        {
            var options = new FlowQueryOptions()
                .Add(c => c.SetMaxResults(5));

            var q = Session.ImmediateFlowQuery<UserEntity>(options);

            Assert.That(q, Is.Not.Null);

            ICriteria criteria = CriteriaHelper.BuildCriteria<UserEntity, UserEntity>(QuerySelection.Create(q as IQueryableFlowQuery));

            Assert.That(criteria.GetRootEntityTypeIfAvailable(), Is.EqualTo(typeof(UserEntity)));
        }

        [Test]
        public void CanCreateImmediateFlowQueryWithAliasAndOptions()
        {
            UserEntity user = null;

            var q = Session.ImmediateFlowQuery(() => user, new FlowQueryOptions());

            Assert.That(q, Is.Not.Null);
        }

        [Test]
        public void ImmediateDoesNotThrowWhenOptionsIsNull()
        {
            FlowQueryOptions options = null;

            Assert.That(() => Session.ImmediateFlowQuery<UserEntity>(options), Throws.Nothing);
        }

        [Test]
        public void CanCreateDelayedFlowQuery()
        {
            var q = Session.DelayedFlowQuery<UserEntity>();

            Assert.That(q, Is.Not.Null);
        }

        [Test]
        public void CanCreateDelayedFlowQueryWithAlias()
        {
            UserEntity user = null;

            var q = Session.DelayedFlowQuery(() => user);

            Assert.That(q, Is.Not.Null);
        }

        [Test]
        public void CanCreateDelayedFlowQueryWithOptions()
        {
            var options = new FlowQueryOptions()
                .Add(c => c.SetMaxResults(5));

            var q = Session.DelayedFlowQuery<UserEntity>(options);

            Assert.That(q, Is.Not.Null);

            ICriteria criteria = CriteriaHelper.BuildCriteria<UserEntity, UserEntity>(QuerySelection.Create(q as IQueryableFlowQuery));

            Assert.That(criteria.GetRootEntityTypeIfAvailable(), Is.EqualTo(typeof(UserEntity)));
        }

        [Test]
        public void CanCreateDelayedFlowQueryWithAliasAndOptions()
        {
            UserEntity user = null;

            var q = Session.DelayedFlowQuery(() => user, new FlowQueryOptions());

            Assert.That(q, Is.Not.Null);
        }

        [Test]
        public void DelayedDoesNotThrowWhenOptionsIsNull()
        {
            FlowQueryOptions options = null;

            Assert.That(() => Session.DelayedFlowQuery<UserEntity>(options), Throws.Nothing);
        }

        [Test]
        public void CanCreateDetachedFlowQuery()
        {
            var q = Session.DetachedFlowQuery<UserEntity>();

            Assert.That(q, Is.Not.Null);
        }

        [Test]
        public void CanCreateDetachedFlowQueryWithAlias()
        {
            UserEntity user = null;

            var q = Session.DetachedFlowQuery(() => user);

            Assert.That(q, Is.Not.Null);
        }

        [Test]
        public void CanCreateDetachedFlowQueryWithOptions()
        {
            var options = new FlowQueryOptions()
                .Add(c => c.SetMaxResults(5));

            var q = Session.DetachedFlowQuery<UserEntity>(options);

            Assert.That(q, Is.Not.Null);

            DetachedCriteria criteria = CriteriaHelper.BuildDetachedCriteria(q);

            Assert.That(criteria.GetRootEntityTypeIfAvailable(), Is.EqualTo(typeof(UserEntity)));
        }

        [Test]
        public void CanCreateDetachedFlowQueryWithAliasAndOptions()
        {
            UserEntity user = null;

            var q = Session.DetachedFlowQuery(() => user, new FlowQueryOptions());

            Assert.That(q, Is.Not.Null);
        }

        [Test]
        public void DetachedDoesNotThrowWhenOptionsIsNull()
        {
            FlowQueryOptions options = null;

            Assert.That(() => Session.DetachedFlowQuery<UserEntity>(options), Throws.Nothing);
        }
    }
}