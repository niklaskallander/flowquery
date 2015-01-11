namespace NHibernate.FlowQuery.Test.FlowQuery.Core.IFlowQueryTest
{
    using NHibernate.FlowQuery.Core;
    using NHibernate.FlowQuery.Core.Implementations;
    using NHibernate.FlowQuery.Helpers;
    using NHibernate.FlowQuery.Test.Setup.Entities;
    using NHibernate.Impl;

    using NUnit.Framework;

    [TestFixture]
    public class CommentTest : BaseTest
    {
        [Test]
        public void CanClearCommentUsingNull()
        {
            const string Comment = "Should fetch all users";

            IImmediateFlowQuery<UserEntity> query = DummyQuery<UserEntity>()
                .Comment(Comment);

            var queryable = (IQueryableFlowQuery)query;

            Assert.That(queryable.CommentValue, Is.EqualTo(Comment));

            query.Comment(null);

            Assert.That(queryable.CommentValue, Is.Null);
        }

        [Test]
        public void CanSetComment()
        {
            const string Comment = "Should fetch all users";

            IImmediateFlowQuery<UserEntity> query = DummyQuery<UserEntity>()
                .Comment(Comment);

            var queryable = (IQueryableFlowQuery)query;

            Assert.That(queryable.CommentValue, Is.EqualTo(Comment));
        }

        [Test]
        public void CommentIsPopulatedOnCriteria()
        {
            const string Comment = "Should fetch all users";

            IImmediateFlowQuery<UserEntity> query = Query<UserEntity>()
                .Comment(Comment);

            var queryable = (IQueryableFlowQuery)query;

            Assert.That(queryable.CommentValue, Is.EqualTo(Comment));

            ICriteria criteria = new CriteriaBuilder()
                .Build<UserEntity, UserEntity>(QuerySelection.Create((IQueryableFlowQuery)query));

            Assert.That(criteria, Is.Not.Null);

            var impl = (CriteriaImpl)criteria;

            Assert.That(impl.Comment, Is.EqualTo(Comment));
        }
    }
}