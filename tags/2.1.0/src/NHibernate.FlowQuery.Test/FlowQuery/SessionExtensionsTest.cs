using NHibernate.Criterion;
using NHibernate.FlowQuery.Core;
using NHibernate.FlowQuery.Helpers;
using NHibernate.FlowQuery.Test.Setup.Entities;
using NUnit.Framework;

namespace NHibernate.FlowQuery.Test.FlowQuery
{
    [TestFixture]
    public class SessionExtensionTest : BaseTest
    {
        [Test]
        public void CanCreateFlowQuery()
        {
            Assert.That(() => Session.FlowQuery<UserEntity>(), Throws.Nothing);

            IImmediateFlowQuery<UserEntity> query = Session.FlowQuery<UserEntity>();

            Assert.That(query != null);
        }

        [Test]
        public void CanCreateFlowQueryWithAlias()
        {
            UserEntity alias = null;

            Assert.That(() => Session.FlowQuery(() => alias), Throws.Nothing);

            IImmediateFlowQuery<UserEntity> query = Session.FlowQuery(() => alias);

            Assert.That(query != null);

            ICriteria criteria = CriteriaHelper.BuildCriteria<UserEntity, UserEntity>(QuerySelection.Create(query as IQueryableFlowQuery));

            Assert.That(criteria.Alias, NUnit.Framework.Is.EqualTo("alias"));
        }

        [Test]
        public void CanCreateFlowQueryWithOptions()
        {
            Assert.That(() => Session.FlowQuery<UserEntity>(new FlowQueryOptions()), Throws.Nothing);

            IImmediateFlowQuery<UserEntity> query = Session.FlowQuery<UserEntity>(new FlowQueryOptions());

            Assert.That(query != null);
        }

        [Test]
        public void CanCreateStatelessFlowQuery()
        {
            Assert.That(() => StatelessSession.FlowQuery<UserEntity>(), Throws.Nothing);

            IImmediateFlowQuery<UserEntity> query = StatelessSession.FlowQuery<UserEntity>();

            Assert.That(query != null);
        }

        [Test]
        public void CanCreateStatelessFlowQueryWithAlias()
        {
            UserEntity alias = null;

            Assert.That(() => StatelessSession.FlowQuery(() => alias), Throws.Nothing);

            IImmediateFlowQuery<UserEntity> query = StatelessSession.FlowQuery(() => alias);

            Assert.That(query != null);

            ICriteria criteria = CriteriaHelper.BuildCriteria<UserEntity, UserEntity>(QuerySelection.Create(query as IQueryableFlowQuery));

            Assert.That(criteria.Alias, NUnit.Framework.Is.EqualTo("alias"));
        }

        [Test]
        public void CanCreateStatelessFlowQueryWithOptions()
        {
            Assert.That(() => StatelessSession.FlowQuery<UserEntity>(new FlowQueryOptions()), Throws.Nothing);

            IImmediateFlowQuery<UserEntity> query = StatelessSession.FlowQuery<UserEntity>(new FlowQueryOptions());

            Assert.That(query != null);
        }

        [Test]
        public void CanCreateStatelessImmediateFlowQuery()
        {
            Assert.That(() => StatelessSession.ImmediateFlowQuery<UserEntity>(), Throws.Nothing);

            IImmediateFlowQuery<UserEntity> query = StatelessSession.ImmediateFlowQuery<UserEntity>();

            Assert.That(query != null);
        }

        [Test]
        public void CanCreateStatelessImmediateFlowQueryWithAlias()
        {
            UserEntity alias = null;

            Assert.That(() => StatelessSession.ImmediateFlowQuery(() => alias), Throws.Nothing);

            IImmediateFlowQuery<UserEntity> query = StatelessSession.ImmediateFlowQuery(() => alias);

            Assert.That(query != null);

            ICriteria criteria = CriteriaHelper.BuildCriteria<UserEntity, UserEntity>(QuerySelection.Create(query as IQueryableFlowQuery));

            Assert.That(criteria.Alias, NUnit.Framework.Is.EqualTo("alias"));
        }

        [Test]
        public void CanCreateStatelessImmediateFlowQueryWithOptions()
        {
            Assert.That(() => StatelessSession.ImmediateFlowQuery<UserEntity>(new FlowQueryOptions()), Throws.Nothing);

            IImmediateFlowQuery<UserEntity> query = StatelessSession.ImmediateFlowQuery<UserEntity>(new FlowQueryOptions());

            Assert.That(query != null);
        }

        [Test]
        public void CanCreateDelayedStatelessFlowQuery()
        {
            Assert.That(() => StatelessSession.DelayedFlowQuery<UserEntity>(), Throws.Nothing);

            IDelayedFlowQuery<UserEntity> query = StatelessSession.DelayedFlowQuery<UserEntity>();

            Assert.That(query != null);
        }

        [Test]
        public void CanCreateStatelessDelayedFlowQueryWithAlias()
        {
            UserEntity alias = null;

            Assert.That(() => StatelessSession.DelayedFlowQuery(() => alias), Throws.Nothing);

            IDelayedFlowQuery<UserEntity> query = StatelessSession.DelayedFlowQuery(() => alias);

            Assert.That(query != null);

            ICriteria criteria = CriteriaHelper.BuildCriteria<UserEntity, UserEntity>(QuerySelection.Create(query as IQueryableFlowQuery));

            Assert.That(criteria.Alias, NUnit.Framework.Is.EqualTo("alias"));
        }

        [Test]
        public void CanCreateStatelessDelayedFlowQueryWithOptions()
        {
            Assert.That(() => StatelessSession.DelayedFlowQuery<UserEntity>(new FlowQueryOptions()), Throws.Nothing);

            IDelayedFlowQuery<UserEntity> query = StatelessSession.DelayedFlowQuery<UserEntity>(new FlowQueryOptions());

            Assert.That(query != null);
        }

        [Test]
        public void CanCreateStatelessDetachedFlowQuery()
        {
            Assert.That(() => StatelessSession.DetachedFlowQuery<UserEntity>(), Throws.Nothing);

            IDetachedFlowQuery<UserEntity> query = StatelessSession.DetachedFlowQuery<UserEntity>();

            Assert.That(query != null);
        }

        [Test]
        public void CanCreateStatelessDetachedFlowQueryWithAlias()
        {
            UserEntity alias = null;

            Assert.That(() => StatelessSession.DetachedFlowQuery(() => alias), Throws.Nothing);

            IDetachedFlowQuery<UserEntity> query = StatelessSession.DetachedFlowQuery(() => alias);

            Assert.That(query != null);

            DetachedCriteria criteria = CriteriaHelper.BuildDetachedCriteria(query);

            Assert.That(criteria.Alias, NUnit.Framework.Is.EqualTo("alias"));
        }

        [Test]
        public void CanCreateStatelessDetachedFlowQueryWithOptions()
        {
            Assert.That(() => StatelessSession.DetachedFlowQuery<UserEntity>(new FlowQueryOptions()), Throws.Nothing);

            IDetachedFlowQuery<UserEntity> query = StatelessSession.DetachedFlowQuery<UserEntity>(new FlowQueryOptions());

            Assert.That(query != null);
        }
    }
}