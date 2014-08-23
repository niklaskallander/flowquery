namespace NHibernate.FlowQuery.Test.FlowQuery.Core.IFlowQueryTest
{
    using NHibernate.FlowQuery.Core;
    using NHibernate.FlowQuery.Core.Implementations;
    using NHibernate.FlowQuery.Helpers;
    using NHibernate.FlowQuery.Test.Setup.Entities;

    using NUnit.Framework;

    [TestFixture]
    public class ReadOnlyTest : BaseTest
    {
        [Test]
        public void CanSpecifyReadOnlyFale()
        {
            IImmediateFlowQuery<UserEntity> query = DummyQuery<UserEntity>();

            var queryable = (IQueryableFlowQuery)query;

            Assert.That(queryable.IsReadOnly, Is.Null);

            query.ReadOnly(false);

            Assert.That(queryable.IsReadOnly, Is.False);
        }

        [Test]
        public void CanSpecifyReadOnlyTrue()
        {
            IImmediateFlowQuery<UserEntity> query = DummyQuery<UserEntity>();

            var queryable = (IQueryableFlowQuery)query;

            Assert.That(queryable.IsReadOnly, Is.Null);

            query.ReadOnly();

            Assert.That(queryable.IsReadOnly, Is.True);
        }

        [Test]
        public void ReadOnlyFalseIsPopulatedOnCriteria()
        {
            IImmediateFlowQuery<UserEntity> query = Query<UserEntity>()
                .ReadOnly(false);

            var queryable = (IQueryableFlowQuery)query;

            ICriteria criteria = new CriteriaBuilder()
                .Build<UserEntity, UserEntity>(QuerySelection.Create(queryable));

            Assert.That(criteria, Is.Not.Null);
            Assert.That(criteria.IsReadOnlyInitialized, Is.True, "Initialized");
            Assert.That(criteria.IsReadOnly, Is.False, "ReadOnly");
        }

        [Test]
        public void ReadOnlyTrueIsPopulatedOnCriteria()
        {
            IImmediateFlowQuery<UserEntity> query = Query<UserEntity>()
                .ReadOnly();

            var queryable = (IQueryableFlowQuery)query;

            ICriteria criteria = new CriteriaBuilder()
                .Build<UserEntity, UserEntity>(QuerySelection.Create(queryable));

            Assert.That(criteria, Is.Not.Null);
            Assert.That(criteria.IsReadOnlyInitialized, Is.True, "Initialized");
            Assert.That(criteria.IsReadOnly, Is.True, "ReadOnly");
        }
    }
}