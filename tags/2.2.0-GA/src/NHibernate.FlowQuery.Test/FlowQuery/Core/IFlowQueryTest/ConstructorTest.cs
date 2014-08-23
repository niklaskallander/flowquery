// ReSharper disable ExpressionIsAlwaysNull
namespace NHibernate.FlowQuery.Test.FlowQuery.Core.IFlowQueryTest
{
    using NHibernate.Criterion;
    using NHibernate.FlowQuery.Core;
    using NHibernate.FlowQuery.Core.Implementations;
    using NHibernate.FlowQuery.Helpers;
    using NHibernate.FlowQuery.Test.Setup.Entities;

    using NUnit.Framework;

    [TestFixture]
    public class ConstructorTest : BaseTest
    {
        [Test]
        public void CanCreateDelayedFlowQuery()
        {
            IDelayedFlowQuery<UserEntity> q = Session.DelayedFlowQuery<UserEntity>();

            Assert.That(q, Is.Not.Null);
        }

        [Test]
        public void CanCreateDelayedFlowQueryWithAlias()
        {
            UserEntity user = null;

            IDelayedFlowQuery<UserEntity> q = Session.DelayedFlowQuery(() => user);

            Assert.That(q, Is.Not.Null);
        }

        [Test]
        public void CanCreateDelayedFlowQueryWithAliasAndOptions()
        {
            UserEntity user = null;

            IDelayedFlowQuery<UserEntity> q = Session.DelayedFlowQuery(() => user, new FlowQueryOptions());

            Assert.That(q, Is.Not.Null);
        }

        [Test]
        public void CanCreateDelayedFlowQueryWithOptions()
        {
            FlowQueryOptions options = new FlowQueryOptions()
                .Add(c => c.SetMaxResults(5));

            IDelayedFlowQuery<UserEntity> q = Session.DelayedFlowQuery<UserEntity>(options);

            Assert.That(q, Is.Not.Null);

            ICriteria criteria = new CriteriaBuilder()
                .Build<UserEntity, UserEntity>(QuerySelection.Create(q as IQueryableFlowQuery));

            Assert.That(criteria.GetRootEntityTypeIfAvailable(), Is.EqualTo(typeof(UserEntity)));
        }

        [Test]
        public void CanCreateDetachedFlowQuery()
        {
            IDetachedFlowQuery<UserEntity> q = Session.DetachedFlowQuery<UserEntity>();

            Assert.That(q, Is.Not.Null);
        }

        [Test]
        public void CanCreateDetachedFlowQueryWithAlias()
        {
            UserEntity user = null;

            IDetachedFlowQuery<UserEntity> q = Session.DetachedFlowQuery(() => user);

            Assert.That(q, Is.Not.Null);
        }

        [Test]
        public void CanCreateDetachedFlowQueryWithAliasAndOptions()
        {
            UserEntity user = null;

            IDetachedFlowQuery<UserEntity> q = Session.DetachedFlowQuery(() => user, new FlowQueryOptions());

            Assert.That(q, Is.Not.Null);
        }

        [Test]
        public void CanCreateDetachedFlowQueryWithOptions()
        {
            FlowQueryOptions options = new FlowQueryOptions()
                .Add(c => c.SetMaxResults(5));

            var q = Session.DetachedFlowQuery<UserEntity>(options) as DetachedFlowQuery<UserEntity>;

            Assert.That(q, Is.Not.Null);

            DetachedCriteria criteria = new CriteriaBuilder()
                .Build<UserEntity>(q);

            Assert.That(criteria.GetRootEntityTypeIfAvailable(), Is.EqualTo(typeof(UserEntity)));
        }

        [Test]
        public void CanCreateFlowQuery()
        {
            IImmediateFlowQuery<UserEntity> q = Session.FlowQuery<UserEntity>();

            Assert.That(q, Is.Not.Null);
        }

        [Test]
        public void CanCreateFlowQueryWithAlias()
        {
            UserEntity user = null;

            IImmediateFlowQuery<UserEntity> q = Session.FlowQuery(() => user);

            Assert.That(q, Is.Not.Null);
        }

        [Test]
        public void CanCreateFlowQueryWithAliasAndOptions()
        {
            UserEntity user = null;

            IImmediateFlowQuery<UserEntity> q = Session.FlowQuery(() => user, new FlowQueryOptions());

            Assert.That(q, Is.Not.Null);
        }

        [Test]
        public void CanCreateFlowQueryWithOptions()
        {
            FlowQueryOptions options = new FlowQueryOptions()
                .Add(c => c.SetMaxResults(5));

            IImmediateFlowQuery<UserEntity> q = Session.FlowQuery<UserEntity>(options);

            Assert.That(q, Is.Not.Null);

            ICriteria criteria = new CriteriaBuilder()
                .Build<UserEntity, UserEntity>(QuerySelection.Create(q as IQueryableFlowQuery));

            Assert.That(criteria.GetRootEntityTypeIfAvailable(), Is.EqualTo(typeof(UserEntity)));
        }

        [Test]
        public void CanCreateImmediateFlowQuery()
        {
            IImmediateFlowQuery<UserEntity> q = Session.ImmediateFlowQuery<UserEntity>();

            Assert.That(q, Is.Not.Null);
        }

        [Test]
        public void CanCreateImmediateFlowQueryWithAlias()
        {
            UserEntity user = null;

            IImmediateFlowQuery<UserEntity> q = Session.ImmediateFlowQuery(() => user);

            Assert.That(q, Is.Not.Null);
        }

        [Test]
        public void CanCreateImmediateFlowQueryWithAliasAndOptions()
        {
            UserEntity user = null;

            IImmediateFlowQuery<UserEntity> q = Session.ImmediateFlowQuery(() => user, new FlowQueryOptions());

            Assert.That(q, Is.Not.Null);
        }

        [Test]
        public void CanCreateImmediateFlowQueryWithOptions()
        {
            FlowQueryOptions options = new FlowQueryOptions()
                .Add(c => c.SetMaxResults(5));

            IImmediateFlowQuery<UserEntity> q = Session.ImmediateFlowQuery<UserEntity>(options);

            Assert.That(q, Is.Not.Null);

            ICriteria criteria = new CriteriaBuilder()
                .Build<UserEntity, UserEntity>(QuerySelection.Create(q as IQueryableFlowQuery));

            Assert.That(criteria.GetRootEntityTypeIfAvailable(), Is.EqualTo(typeof(UserEntity)));
        }

        [Test]
        public void DelayedDoesNotThrowWhenOptionsIsNull()
        {
            FlowQueryOptions options = null;

            Assert.That(() => Session.DelayedFlowQuery<UserEntity>(options), Throws.Nothing);
        }

        [Test]
        public void DetachedDoesNotThrowWhenOptionsIsNull()
        {
            FlowQueryOptions options = null;

            Assert.That(() => Session.DetachedFlowQuery<UserEntity>(options), Throws.Nothing);
        }

        [Test]
        public void DoesNotThrowWhenOptionsIsNull()
        {
            FlowQueryOptions options = null;

            Assert.That(() => Session.FlowQuery<UserEntity>(options), Throws.Nothing);
        }

        [Test]
        public void ImmediateDoesNotThrowWhenOptionsIsNull()
        {
            FlowQueryOptions options = null;

            Assert.That(() => Session.ImmediateFlowQuery<UserEntity>(options), Throws.Nothing);
        }
    }
}