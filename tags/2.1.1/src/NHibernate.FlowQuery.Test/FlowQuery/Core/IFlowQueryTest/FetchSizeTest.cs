using NHibernate.FlowQuery.Core;
using NHibernate.FlowQuery.Helpers;
using NHibernate.FlowQuery.Test.Setup.Entities;
using NHibernate.Impl;
using NUnit.Framework;

namespace NHibernate.FlowQuery.Test.FlowQuery.Core.IFlowQueryTest
{
    using Is = NUnit.Framework.Is;

    [TestFixture]
    public class FetchSizeTest : BaseTest
    {
        [Test]
        public void CanSetFetchSize()
        {
            var query = DummyQuery<UserEntity>()
                .FetchSize(10);

            var queryable = (IQueryableFlowQuery)query;

            Assert.That(queryable.FetchSizeValue, Is.EqualTo(10));
        }

        [Test]
        public void CanResetFetchSizeUsingZero()
        {
            var query = DummyQuery<UserEntity>()
                .FetchSize(10);

            var queryable = (IQueryableFlowQuery)query;

            Assert.That(queryable.FetchSizeValue, Is.EqualTo(10));

            query.FetchSize(0);

            Assert.That(queryable.FetchSizeValue, Is.EqualTo(0));
        }

        [Test]
        public void FetchSizeIsPopulatedOnCriteria()
        {
            var query = Query<UserEntity>()
                .FetchSize(10);

            var queryable = (IQueryableFlowQuery)query;

            Assert.That(queryable.FetchSizeValue, Is.EqualTo(10));

            ICriteria criteria = CriteriaHelper.BuildCriteria<UserEntity, UserEntity>(QuerySelection.Create((IQueryableFlowQuery)query));

            Assert.That(criteria, Is.Not.Null);

            var impl = (CriteriaImpl)criteria;

            Assert.That(impl.FetchSize, Is.EqualTo(10));
        }
    }
}