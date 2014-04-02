using NHibernate.FlowQuery.Core;
using NHibernate.FlowQuery.Helpers;
using NHibernate.FlowQuery.Test.Setup.Entities;
using NUnit.Framework;

namespace NHibernate.FlowQuery.Test.FlowQuery.Core.IFlowQueryTest
{
    using Is = NUnit.Framework.Is;

    [TestFixture]
    public class ReadOnlyTest : BaseTest
    {
        [Test]
        public void CanSpecifyReadOnlyTrue()
        {
            var query = DummyQuery<UserEntity>();

            var queryable = (IQueryableFlowQuery)query;

            Assert.That(queryable.IsReadOnly, Is.Null);

            query.ReadOnly();

            Assert.That(queryable.IsReadOnly, Is.True);
        }

        [Test]
        public void CanSpecifyReadOnlyFale()
        {
            var query = DummyQuery<UserEntity>();

            var queryable = (IQueryableFlowQuery)query;

            Assert.That(queryable.IsReadOnly, Is.Null);

            query.ReadOnly(false);

            Assert.That(queryable.IsReadOnly, Is.False);
        }

        [Test]
        public void ReadOnlyTrueIsPopulatedOnCriteria()
        {
            var query = Query<UserEntity>()
                .ReadOnly();

            var queryable = (IQueryableFlowQuery)query;

            ICriteria criteria = CriteriaHelper.BuildCriteria<UserEntity, UserEntity>(QuerySelection.Create(queryable));

            Assert.That(criteria, Is.Not.Null);
            Assert.That(criteria.IsReadOnlyInitialized, Is.True, "Initialized");
            Assert.That(criteria.IsReadOnly, Is.True, "ReadOnly");
        }

        [Test]
        public void ReadOnlyFalseIsPopulatedOnCriteria()
        {
            var query = Query<UserEntity>()
                .ReadOnly(false);

            var queryable = (IQueryableFlowQuery)query;

            ICriteria criteria = CriteriaHelper.BuildCriteria<UserEntity, UserEntity>(QuerySelection.Create(queryable));

            Assert.That(criteria, Is.Not.Null);
            Assert.That(criteria.IsReadOnlyInitialized, Is.True, "Initialized");
            Assert.That(criteria.IsReadOnly, Is.False, "ReadOnly");
        }
    }
}