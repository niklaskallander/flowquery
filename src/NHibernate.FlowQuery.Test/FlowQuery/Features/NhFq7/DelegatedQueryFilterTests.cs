namespace NHibernate.FlowQuery.Test.FlowQuery.Features.NhFq7
{
    using System;

    using NHibernate.FlowQuery.Core;
    using NHibernate.FlowQuery.Core.Implementations;
    using NHibernate.FlowQuery.Test.Setup.Dtos;
    using NHibernate.FlowQuery.Test.Setup.Entities;

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
                .Where(x => x.Name == "A1")
                .ApplyFilterOn(() => user, userFilter)
                .Select(x => new UserDto
                {
                    Id = user.Id
                });
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