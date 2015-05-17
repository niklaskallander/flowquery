namespace NHibernate.FlowQuery.Test.FlowQuery.Features.NhFq7
{
    using System;

    using NHibernate.FlowQuery.Core;
    using NHibernate.FlowQuery.Core.Implementations;
    using NHibernate.FlowQuery.Test.Setup.Dtos;
    using NHibernate.FlowQuery.Test.Setup.Entities;
    using NHibernate.SqlCommand;

    using NUnit.Framework;

    [TestFixture]
    public class DelegatedQueryFilterTests : NhFq7TestBase
    {
        [Test]
        public void Given_SettingFilter_And_UserQuery_When_SettingIsJoined_And_FilterUsesMagicStrings_Then_ReturnsUsersWithMatchingSettings()
        {
            Setting setting = null;

            IImmediateFlowQuery<UserEntity> query = Query<UserEntity>()
                .Inner.Join(x => x.Setting, () => setting);

            UserEntity[] users = query
                .Copy()
                .ApplyFilterOn(() => setting, Where.Id_SpecifiedByMagicString_Of_Setting_Is(1))
                .Select();

            Assert.That(users.Length, Is.EqualTo(0));

            users = query
                .Copy()
                .ApplyFilterOn(() => setting, Where.Id_SpecifiedByMagicString_Of_Setting_Is(6))
                .Select();

            Assert.That(users.Length, Is.EqualTo(4));
        }

        [Test]
        public void Given_SettingFilter_And_UserQuery_When_SettingIsJoined_Then_ReturnsUsersWithMatchingSettings()
        {
            Setting setting = null;

            IImmediateFlowQuery<UserEntity> query = Query<UserEntity>()
                .Inner.Join(x => x.Setting, () => setting);

            UserEntity[] users = query
                .Copy()
                .ApplyFilterOn(() => setting, Where.Id_Of_Setting_Is(1))
                .Select();

            Assert.That(users.Length, Is.EqualTo(0));

            users = query
                .Copy()
                .ApplyFilterOn(() => setting, Where.Id_Of_Setting_Is(6))
                .Select();

            Assert.That(users.Length, Is.EqualTo(4));
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
        public void Given_UserFilter_When_FilterJoinsAssociation_And_UsingIsHelper_Then_ReturnsUsersMatchingJoinsAndInnerFilter()
        {
            TestUserEntityFilter(Where.Id_Of_JoinedSetting_UsingIsHelper_Is);
        }

        [Test]
        public void Given_UserFilter_When_FilterJoinsAssociation_And_UsingWhereHelper_Then_ReturnsUsersMatchingJoinsAndInnerFilter()
        {
            TestUserEntityFilter(Where.Id_Of_JoinedSetting_UsingWhereHelper_Is);
        }

        [Test]
        public void Given_UserFilter_When_FilterWrapsOtherFilter_Then_ReturnsUsersMatchingInnerFilter()
        {
            Setting setting = null;

            var wrappedFilter =
                new QueryFilter<Setting>(query => query.ApplyFilter(Where.Id_Of_Setting_Is(0)));

            UserEntity[] users = Query<UserEntity>()
                .Inner.Join(x => x.Setting, () => setting)
                .ApplyFilterOn(() => setting, wrappedFilter)
                .Select();

            Assert.That(users.Length, Is.EqualTo(0));

            wrappedFilter =
                new QueryFilter<Setting>(query => query.ApplyFilter(Where.Id_Of_Setting_Is(6)));

            users = Query<UserEntity>()
                .Inner.Join(x => x.Setting, () => setting)
                .ApplyFilterOn(() => setting, wrappedFilter)
                .Select();

            Assert.That(users.Length, Is.EqualTo(4));
        }

        [Test]
        public void Given_SettingFilter_And_Subquery_When_FilterChecksWhetherSubqueryReturnsNothing_Then_ReturnsUsersMatchingFilter()
        {
            Setting setting = null;

            var subquery = Query<Setting>()
                .Detached()
                .Where(x => x.Id == 6)
                .Select(x => x.Id);

            var filter = Where.Subquery_Returns_Nothing<Setting>(subquery);

            UserEntity[] users = Query<UserEntity>()
                .Inner.Join(x => x.Setting, () => setting)
                .ApplyFilterOn(() => setting, filter)
                .Select();

            Assert.That(users.Length, Is.EqualTo(0));

            subquery = Query<Setting>()
                .Detached()
                .Where(x => x.Id == 0)
                .Select(x => x.Id);

            filter = Where.Subquery_Returns_Nothing<Setting>(subquery);

            users = Query<UserEntity>()
                .Inner.Join(x => x.Setting, () => setting)
                .ApplyFilterOn(() => setting, filter)
                .Select();

            Assert.That(users.Length, Is.EqualTo(4));
        }

        [Test]
        public void Given_SettingFilter_And_DetachedCriteria_When_FilterChecksWhetherDetachedCriteriaReturnsNothing_Then_ReturnsUsersMatchingFilter()
        {
            Setting setting = null;

            var subquery = Query<Setting>()
                .Detached()
                .Where(x => x.Id == 6)
                .Select(x => x.Id);

            var filter = Where.Subquery_Returns_Nothing<Setting>(subquery.Criteria);

            UserEntity[] users = Query<UserEntity>()
                .Inner.Join(x => x.Setting, () => setting)
                .ApplyFilterOn(() => setting, filter)
                .Select();

            Assert.That(users.Length, Is.EqualTo(0));

            subquery = Query<Setting>()
                .Detached()
                .Where(x => x.Id == 0)
                .Select(x => x.Id);

            filter = Where.Subquery_Returns_Nothing<Setting>(subquery.Criteria);

            users = Query<UserEntity>()
                .Inner.Join(x => x.Setting, () => setting)
                .ApplyFilterOn(() => setting, filter)
                .Select();

            Assert.That(users.Length, Is.EqualTo(4));
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

        [Test]
        public void Given_UserQuery_And_JoinBuilder_When_JoinBuilderIsNullReference_Then_DelegatedJoinBuilderThrows()
        {
            Assert
                .That
                (
                    () => CreateDelegatedJoinBuilder<Setting, UserEntity>(null, DummyQuery<UserEntity>(), "alias"),
                    Throws.InstanceOf<ArgumentNullException>()
                );
        }

        [Test]
        public void Given_UserQuery_And_JoinBuilder_When_UserQueryIsNullReference_Then_DelegatedJoinBuilderThrows()
        {
            Assert
                .That
                (
                    () => CreateDelegatedJoinBuilder(DummyQuery<UserEntity>().Inner, (IFilterableQuery<Setting>)null, "alias"),
                    Throws.InstanceOf<ArgumentNullException>()
                );
        }

        [Test]
        public void Given_UserQuery_And_Alias_When_UserQueryIsNullReference_Then_DelegatedFilterableQueryThrows()
        {
            Assert
                .That
                (
                    () => new DelegatedFilterableQuery<Setting, UserEntity, IImmediateFlowQuery<UserEntity>>(null, "alias"),
                    Throws.InstanceOf<ArgumentNullException>()
                );
        }

        [Test]
        public void Given_UserQuery_And_Alias_When_AliasIsNull_Then_DelegatedFilterableQueryThrows()
        {
            Assert
                .That
                (
                    () => new DelegatedFilterableQuery<Setting, UserEntity, IImmediateFlowQuery<UserEntity>>(DummyQuery<UserEntity>(), null),
                    Throws.ArgumentException
                );
        }

        [Test]
        public void Given_UserQuery_And_Alias_When_AliasIsEmpty_Then_DelegatedFilterableQueryThrows()
        {
            Assert
                .That
                (
                    () => new DelegatedFilterableQuery<Setting, UserEntity, IImmediateFlowQuery<UserEntity>>(DummyQuery<UserEntity>(), string.Empty),
                    Throws.ArgumentException
                );
        }

        [Test]
        public void Given_UserQuery_And_Alias_When_AliasIsOnlyWhiteSpace_Then_DelegatedFilterableQueryThrows()
        {
            Assert
                .That
                (
                    () => new DelegatedFilterableQuery<Setting, UserEntity, IImmediateFlowQuery<UserEntity>>(DummyQuery<UserEntity>(), "    "),
                    Throws.ArgumentException
                );
        }

        private static DelegatedJoinBuilder<T, TSource, IImmediateFlowQuery<TSource>> CreateDelegatedJoinBuilder<T, TSource>
            (
            IJoinBuilder<TSource, IImmediateFlowQuery<TSource>> innerJoinBuilder,
            IFilterableQuery<T> query,
            string alias
            )
        {
            return new DelegatedJoinBuilder<T, TSource, IImmediateFlowQuery<TSource>>(innerJoinBuilder, query, alias);
        }

        private static DelegatedJoinBuilder<T, TSource, IImmediateFlowQuery<TSource>> CreateDelegatedJoinBuilder<T, TSource>
            (
            IJoinBuilder<TSource, IImmediateFlowQuery<TSource>> innerJoinBuilder,
            IImmediateFlowQuery<TSource> innerQuery,
            string alias
            )
        {
            return CreateDelegatedJoinBuilder(innerJoinBuilder, CreateDelegatedQuery<T, TSource>(innerQuery, alias), alias);
        }

        private static DelegatedFilterableQuery<T, TSource, IImmediateFlowQuery<TSource>> CreateDelegatedQuery<T, TSource>
            (
            IImmediateFlowQuery<TSource> innerQuery,
            string alias
            )
        {
            return new DelegatedFilterableQuery<T, TSource, IImmediateFlowQuery<TSource>>(innerQuery, alias);
        }

        private UserDto[] FetchUsersInGroupA1ByFilter(IQueryFilter<UserEntity> userFilter)
        {
            UserGroupLinkEntity userGroupLink = null;
            UserEntity user = null;

            return Query<GroupEntity>()
                .Inner.Join(x => x.Users, () => userGroupLink)
                .Inner.Join(x => userGroupLink.User, () => user)
                .And(x => x.Name == "A1")
                .ApplyFilterOn(() => user, userFilter)
                .Select(x => new UserDto
                {
                    Id = user.Id
                });
        }

        private JoinType GetJoinTypeFrom(Func<DelegatedFilterableQuery<Setting, UserEntity, IImmediateFlowQuery<UserEntity>>, IJoinBuilder<Setting, IFilterableQuery<Setting>>> builder)
        {
            var query =
                new DelegatedFilterableQuery<Setting, UserEntity, IImmediateFlowQuery<UserEntity>>(DummyQuery<UserEntity>(), "setting");

            var helper = builder(query);

            return GetJoinTypeFrom(helper);
        }

        private void TestUserEntityFilter
            (
            Action<IFilterableQuery<UserEntity>, int> filter
            )
        {
            UserDto[] users = FetchUsersInGroupA1ByFilter(For(filter, 1));

            Assert.That(users.Length, Is.EqualTo(0));

            users = FetchUsersInGroupA1ByFilter(For(filter, 6));

            Assert.That(users.Length, Is.EqualTo(2));
        }
    }
}