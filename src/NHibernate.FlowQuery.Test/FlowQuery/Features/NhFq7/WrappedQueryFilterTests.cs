namespace NHibernate.FlowQuery.Test.FlowQuery.Features.NhFq7
{
    using System;

    using NHibernate.FlowQuery.Core;
    using NHibernate.FlowQuery.Core.Implementations;
    using NHibernate.FlowQuery.Test.Setup.Entities;
    using NHibernate.SqlCommand;
    using NHibernate.Util;

    using NUnit.Framework;

    [TestFixture]
    public class WrappedQueryFilterTests : NhFq7TestBase
    {
        [Test]
        public void Given_UserFilter_Then_ReturnsUsersMatchingFilter()
        {
            UserEntity[] users = Query<UserEntity>()
                .ApplyFilter(Where.Id_Of_User_Is(between: 1, and: 2))
                .Select();

            Assert.That(users.Length, Is.EqualTo(2));

            users.ForEach(user => Assert.That(user.Id, Is.InRange(1, 2)));
        }

        [Test]
        public void Given_UserFilter_When_IdIsSpecfiedByMagicString_Then_ReturnsUsersMatchingFilter()
        {
            UserEntity[] users = Query<UserEntity>()
                .ApplyFilter(Where.Id_SpecifedByMagicString_Of_User_Is(between: 1, and: 2))
                .Select();

            Assert.That(users.Length, Is.EqualTo(2));

            users.ForEach(user => Assert.That(user.Id, Is.InRange(1, 2)));
        }

        [Test]
        public void Given_UserFilter_Where_UsingWhereHelper_Then_ReturnsUsersMatchingFilter()
        {
            UserEntity[] users = Query<UserEntity>()
                .ApplyFilter(Where.Id_Of_User_UsingWhereHelper_Is(between: 1, and: 2))
                .Select();

            Assert.That(users.Length, Is.EqualTo(2));

            users.ForEach(user => Assert.That(user.Id, Is.InRange(1, 2)));
        }

        [Test]
        public void Given_UserFilter_When_FilterJoinsAssociationByMagicString_And_FilterAddsFilterOnAddedJoin_Then_ReturnsUsersMatchingJoinsAndInnerFilter()
        {
            TestUserEntityFilter(Where.Id_Of_JoinedSetting_AliasedByMagicString_Is);
        }

        [Test]
        public void Given_UserFilter_When_FilterJoinsAssociationByMagicString_Then_ReturnsUsersMatchingJoinsAndInnerFilter()
        {
            TestUserEntityFilter(Where.Id_Of_JoinedSetting_AliasedByMagicString_FilteredInJoin_Is);
        }

        [Test]
        public void Given_UserFilter_When_FilterJoinsAssociation_And_FilterAddsFilterOnAddedJoin_Then_ReturnsUsersMatchingJoinsAndInnerFilter()
        {
            TestUserEntityFilter(Where.Id_Of_JoinedSetting_Is);
        }

        [Test]
        public void Given_UserFilter_When_FilterJoinsAssociation_Then_ReturnsUsersMatchingJoinsAndInnerFilter()
        {
            TestUserEntityFilter(Where.Id_Of_JoinedSetting_FilteredInJoin_Is);
        }

        [Test]
        public void Given_UserFilter_When_FilterJoinsAssociationUsingRevealConvention_And_FilterAddsFilterOnAddedJoin_Then_ReturnsUsersMatchingJoinsAndInnerFilter()
        {
            TestUserEntityFilter(Where.Id_Of_JoinedSetting_UsingRevealer_Is);
        }

        [Test]
        public void Given_UserFilter_When_FilterJoinsAssociationUsingRevealConvention_Then_ReturnsUsersMatchingJoinsAndInnerFilter()
        {
            TestUserEntityFilter(Where.Id_Of_JoinedSetting_FilteredInJoin_UsingRevealer_Is);
        }

        [Test]
        public void Given_UserFilter_And_Subquery_When_FilterChecksWhetherSubqueryReturnsNothing_Then_ReturnsUsersMatchingFilter()
        {
            var subquery = Query<Setting>()
                .Detached()
                .Where(x => x.Id == 6)
                .Select(x => x.Id);

            var filter = Where.Subquery_Returns_Nothing<UserEntity>(subquery);

            UserEntity[] users = Query<UserEntity>()
                .ApplyFilter(filter)
                .Select();

            Assert.That(users.Length, Is.EqualTo(0));

            subquery = Query<Setting>()
                .Detached()
                .Where(x => x.Id == 0)
                .Select(x => x.Id);

            filter = Where.Subquery_Returns_Nothing<UserEntity>(subquery);

            users = Query<UserEntity>()
                .ApplyFilter(filter)
                .Select();

            Assert.That(users.Length, Is.EqualTo(4));
        }

        [Test]
        public void Given_UserFilter_And_DetachedCriteria_When_FilterChecksWhetherDetachedCriteriaReturnsNothing_Then_ReturnsUsersMatchingFilter()
        {
            var subquery = Query<Setting>()
                .Detached()
                .Where(x => x.Id == 6)
                .Select(x => x.Id);

            var filter = Where.Subquery_Returns_Nothing<UserEntity>(subquery.Criteria);

            UserEntity[] users = Query<UserEntity>()
                .ApplyFilter(filter)
                .Select();

            Assert.That(users.Length, Is.EqualTo(0));

            subquery = Query<Setting>()
                .Detached()
                .Where(x => x.Id == 0)
                .Select(x => x.Id);

            filter = Where.Subquery_Returns_Nothing<UserEntity>(subquery.Criteria);

            users = Query<UserEntity>()
                .ApplyFilter(filter)
                .Select();

            Assert.That(users.Length, Is.EqualTo(4));
        }

        [Test]
        public void Given_UserFilter_When_FilterWrapsOtherFilter_Then_ReturnsUsersMatchingInnerFilter()
        {
            var wrappedFilter =
                new QueryFilter<UserEntity>(query => query.ApplyFilter(Where.Id_Of_User_Is(between: 1, and: 2)));

            UserEntity[] users = Query<UserEntity>()
                .ApplyFilter(wrappedFilter)
                .Select();

            Assert.That(users.Length, Is.EqualTo(2));

            users.ForEach(x => Assert.That(x.Id, Is.InRange(1, 2)));
        }

        [Test]
        public void Given_UserQuery_And_JoinBuilder_When_JoinBuilderIsNullReference_Then_WrappedJoinBuilderThrows()
        {
            Assert
                .That
                (
                    () => CreateWrappedJoinBuilder(null, DummyQuery<UserEntity>()),
                    Throws.InstanceOf<ArgumentNullException>()
                );
        }

        [Test]
        public void Given_UserQuery_And_JoinBuilder_When_UserQueryIsNullReference_Then_WrappedJoinBuilderThrows()
        {
            Assert
                .That
                (
                    () => CreateWrappedJoinBuilder(DummyQuery<UserEntity>().Inner, (IFilterableQuery<UserEntity>)null),
                    Throws.InstanceOf<ArgumentNullException>()
                );
        }

        [Test]
        public void Given_UserQuery_When_UserQueryIsNullReference_Then_WrappedFilterableQueryThrows()
        {
            Assert
                .That
                (
                    () => new WrappedFilterableQuery<UserEntity, IFilterableQuery<UserEntity>>(null),
                    Throws.InstanceOf<ArgumentNullException>()
                );
        }

        [Test]
        public void InnerJoinsWrapCorrectJoinBuilder()
        {
            JoinType type = GetJoinTypeFrom(query => query.Inner);

            Assert.That(type, Is.EqualTo(JoinType.InnerJoin));
        }

        [Test]
        public void FullJoinsWrapCorrectJoinBuilder()
        {
            JoinType type = GetJoinTypeFrom(query => query.Full);

            Assert.That(type, Is.EqualTo(JoinType.FullJoin));
        }

        [Test]
        public void RightOuterJoinsWrapCorrectJoinBuilder()
        {
            JoinType type = GetJoinTypeFrom(query => query.RightOuter);

            Assert.That(type, Is.EqualTo(JoinType.RightOuterJoin));
        }

        [Test]
        public void LeftOuterJoinsWrapCorrectJoinBuilder()
        {
            JoinType type = GetJoinTypeFrom(query => query.LeftOuter);

            Assert.That(type, Is.EqualTo(JoinType.LeftOuterJoin));
        }

        private static WrappedJoinBuilder<T, IImmediateFlowQuery<T>> CreateWrappedJoinBuilder<T>
            (
            IJoinBuilder<T, IImmediateFlowQuery<T>> innerJoinBuilder,
            IFilterableQuery<T> query
            )
        {
            return new WrappedJoinBuilder<T, IImmediateFlowQuery<T>>(innerJoinBuilder, query);
        }

        private static WrappedJoinBuilder<T, IImmediateFlowQuery<T>> CreateWrappedJoinBuilder<T>
            (
            IJoinBuilder<T, IImmediateFlowQuery<T>> innerJoinBuilder,
            IImmediateFlowQuery<T> innerQuery
            )
        {
            return CreateWrappedJoinBuilder(innerJoinBuilder, CreateWrappedQuery(innerQuery));
        }

        private static WrappedFilterableQuery<T, IImmediateFlowQuery<T>> CreateWrappedQuery<T>
            (
            IImmediateFlowQuery<T> innerQuery
            )
        {
            return new WrappedFilterableQuery<T, IImmediateFlowQuery<T>>(innerQuery);
        }

        private JoinType GetJoinTypeFrom(Func<WrappedFilterableQuery<UserEntity, IImmediateFlowQuery<UserEntity>>, IJoinBuilder<UserEntity, IFilterableQuery<UserEntity>>> builder)
        {
            var query =
                new WrappedFilterableQuery<UserEntity, IImmediateFlowQuery<UserEntity>>(DummyQuery<UserEntity>());

            var helper = builder(query);

            return GetJoinTypeFrom(helper);
        }

        private void TestUserEntityFilter
            (
            Action<IFilterableQuery<UserEntity>, int> filter
            )
        {
            UserEntity[] users = Query<UserEntity>()
                .ApplyFilter(For(filter, 1))
                .Select();

            Assert.That(users.Length, Is.EqualTo(0));

            users = Query<UserEntity>()
                .ApplyFilter(For(filter, 6))
                .Select();

            Assert.That(users.Length, Is.EqualTo(4));
        }
    }
}