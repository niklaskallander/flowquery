using NHibernate.FlowQuery.Core;
using NHibernate.FlowQuery.Helpers;
using NHibernate.FlowQuery.Test.Setup.Entities;
using NUnit.Framework;

namespace NHibernate.FlowQuery.Test
{
    using Is = NUnit.Framework.Is;

    [TestFixture]
    public class SessionExtensionTest : BaseTest
    {
        #region Methods (2)

        [Test]
        public void CanCreateFlowQuery()
        {
            Assert.That(() => { Session.FlowQuery<UserEntity>(); }, Throws.Nothing);

            IImmediateFlowQuery<UserEntity> query = Session.FlowQuery<UserEntity>();

            Assert.That(query != null);
        }

        [Test]
        public void CanCreateFlowQueryWithAlias()
        {
            UserEntity alias = null;

            Assert.That(() => { Session.FlowQuery<UserEntity>(() => alias); }, Throws.Nothing);

            IImmediateFlowQuery<UserEntity> query = Session.FlowQuery<UserEntity>(() => alias);

            Assert.That(query != null);

            ICriteria criteria = CriteriaHelper.BuildCriteria<UserEntity, UserEntity>(QuerySelection.Create(query as IQueryableFlowQuery));

            Assert.That(criteria.Alias, Is.EqualTo("alias"));
        }

        [Test]
        public void CanCreateFlowQueryWithOptions()
        {
            Assert.That(() => { Session.FlowQuery<UserEntity>(new FlowQueryOptions()); }, Throws.Nothing);

            IImmediateFlowQuery<UserEntity> query = Session.FlowQuery<UserEntity>(new FlowQueryOptions());

            Assert.That(query != null);
        }

        [Test]
        public void CanCreateStatelessFlowQuery()
        {
            Assert.That(() => { StatelessSession.FlowQuery<UserEntity>(); }, Throws.Nothing);

            IImmediateFlowQuery<UserEntity> query = StatelessSession.FlowQuery<UserEntity>();

            Assert.That(query != null);
        }

        [Test]
        public void CanCreateStatelessFlowQueryWithAlias()
        {
            UserEntity alias = null;

            Assert.That(() => { StatelessSession.FlowQuery<UserEntity>(() => alias); }, Throws.Nothing);

            IImmediateFlowQuery<UserEntity> query = StatelessSession.FlowQuery<UserEntity>(() => alias);

            Assert.That(query != null);

            ICriteria criteria = CriteriaHelper.BuildCriteria<UserEntity, UserEntity>(QuerySelection.Create(query as IQueryableFlowQuery));

            Assert.That(criteria.Alias, Is.EqualTo("alias"));
        }

        [Test]
        public void CanCreateStatelessFlowQueryWithOptions()
        {
            Assert.That(() => { StatelessSession.FlowQuery<UserEntity>(new FlowQueryOptions()); }, Throws.Nothing);

            IImmediateFlowQuery<UserEntity> query = StatelessSession.FlowQuery<UserEntity>(new FlowQueryOptions());

            Assert.That(query != null);
        }

        #endregion Methods
    }
}