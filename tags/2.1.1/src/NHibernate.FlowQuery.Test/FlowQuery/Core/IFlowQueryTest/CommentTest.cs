using NHibernate.FlowQuery.Core;
using NHibernate.FlowQuery.Helpers;
using NHibernate.FlowQuery.Test.Setup.Entities;
using NHibernate.Impl;
using NUnit.Framework;

namespace NHibernate.FlowQuery.Test.FlowQuery.Core.IFlowQueryTest
{
    using Is = NUnit.Framework.Is;

    [TestFixture]
    public class CommentTest : BaseTest
    {
        [Test]
        public void CanSetComment()
        {
            const string comment = "Should fetch all users";

            var query = DummyQuery<UserEntity>()
                .Comment(comment);

            var queryable = (IQueryableFlowQuery)query;

            Assert.That(queryable.CommentValue, Is.EqualTo(comment));
        }

        [Test]
        public void CanClearCommentUsingNull()
        {
            const string comment = "Should fetch all users";

            var query = DummyQuery<UserEntity>()
                .Comment(comment);

            var queryable = (IQueryableFlowQuery)query;

            Assert.That(queryable.CommentValue, Is.EqualTo(comment));

            query.Comment(null);

            Assert.That(queryable.CommentValue, Is.Null);
        }

        [Test]
        public void CommentIsPopulatedOnCriteria()
        {
            const string comment = "Should fetch all users";

            var query = Query<UserEntity>()
                .Comment(comment);

            var queryable = (IQueryableFlowQuery)query;

            Assert.That(queryable.CommentValue, Is.EqualTo(comment));

            ICriteria criteria = CriteriaHelper.BuildCriteria<UserEntity, UserEntity>(QuerySelection.Create((IQueryableFlowQuery)query));

            Assert.That(criteria, Is.Not.Null);

            var impl = (CriteriaImpl)criteria;

            Assert.That(impl.Comment, Is.EqualTo(comment));
        }
    }
}