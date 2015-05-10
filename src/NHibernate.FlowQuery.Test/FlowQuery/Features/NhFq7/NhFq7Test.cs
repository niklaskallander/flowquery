namespace NHibernate.FlowQuery.Test.FlowQuery.Features.NhFq7
{
    using System;
    using System.Linq.Expressions;

    using NHibernate.FlowQuery.Core;
    using NHibernate.FlowQuery.Helpers;
    using NHibernate.FlowQuery.Test.Setup.Entities;

    using NUnit.Framework;

    [TestFixture]
    public class NhFq7Test : BaseTest
    {
        [Test]
        public void ApplyingNullReferenceFilterOnRootThrows()
        {
            var query = DummyQuery<UserEntity>();

            Assert.That(() => query.ApplyFilter(null), Throws.InstanceOf<ArgumentNullException>());
        }

        [Test]
        public void CanApplyQueryFilterOnSource()
        {
            UserEntity[] users = Query<UserEntity>()
                .ApplyFilter(new UserIdIsBetween1And2Filter())
                .Select();

            Assert.That(users.Length, Is.EqualTo(2));

            foreach (UserEntity user in users)
            {
                Assert.That(user.Id, Is.InRange(1, 2));
            }
        }

        [Test]
        public void CanApplyQueryFilterOnSourceThatAppliesAnotherQueryFilterOnSource()
        {
            UserEntity[] users = Query<UserEntity>()
                .ApplyFilter(new WrappedUserIdIsBetween1And2Filter())
                .Select();

            Assert.That(users.Length, Is.EqualTo(2));

            foreach (UserEntity user in users)
            {
                Assert.That(user.Id, Is.InRange(1, 2));
            }
        }

        [Test]
        public void CanApplyFilterOnAlias()
        {
            Setting setting = null;

            var query = Query<UserEntity>()
                .Inner.Join(x => x.Setting, () => setting);

            UserEntity[] users = query
                .Copy()
                .ApplyFilterOn(() => setting, new SettingIdIs(1))
                .Select();

            Assert.That(users.Length, Is.EqualTo(0));

            users = query
                .Copy()
                .ApplyFilterOn(() => setting, new SettingIdIs(6))
                .Select();

            Assert.That(users.Length, Is.EqualTo(4));
        }

        [Test]
        public void CanApplyFilterUsingMagicStringsOnAlias()
        {
            Setting setting = null;

            var query = Query<UserEntity>()
                .Inner.Join(x => x.Setting, () => setting);

            UserEntity[] users = query
                .Copy()
                .ApplyFilterOn(() => setting, new SettingIdByMagicStringIs(1))
                .Select();

            Assert.That(users.Length, Is.EqualTo(0));

            users = query
                .Copy()
                .ApplyFilterOn(() => setting, new SettingIdByMagicStringIs(6))
                .Select();

            Assert.That(users.Length, Is.EqualTo(4));
        }

        [Test]
        public void CanApplyFilterOnSourceSpecifyingAdditionalJoins()
        {
            UserEntity[] users = Query<UserEntity>()
                .ApplyFilter(new JoinedSettingIdIs(1))
                .Select();

            Assert.That(users.Length, Is.EqualTo(0));

            users = Query<UserEntity>()
                .ApplyFilter(new JoinedSettingIdIs(6))
                .Select();

            Assert.That(users.Length, Is.EqualTo(4));
        }

        [Test]
        public void CanApplyFilterOnAliasSpecifyingAdditionalJoins()
        {
            UserGroupLinkEntity groupLink = null;

            var query = Query<UserEntity>()
                .Inner.Join(x => x.Groups, () => groupLink);

            UserEntity[] users = query
                .Copy()
                .ApplyFilterOn(() => groupLink, new GroupLinkFilter())
                .Select();

            Assert.That(users.Length, Is.EqualTo(2));
        }

        [Test]
        public void ExpresionRebaserThrowsIfExpressionIsNull()
        {
            var visitor = new ExpressionRebaser(typeof(Setting), "setting");

            Assert.That(() => visitor.RebaseTo<UserEntity, bool>(null), Throws.InstanceOf<ArgumentNullException>());
        }

        [Test]
        public void ExpresionRebaserThrowsIfExpressionHasMoreThanTwoParameters()
        {
            Expression<Func<Setting, string, string, bool>> filter = (x, y, z) => x.Id == 1;

            var visitor = new ExpressionRebaser(typeof(Setting), "setting");

            Assert.That(() => visitor.RebaseTo<UserEntity, bool>(filter), Throws.InstanceOf<ArgumentException>());
        }

        [Test]
        public void ExpresionRebaserThrowsIfExpressionHasTwoParametersAndSecondParamterIsNotWhereDelegate()
        {
            Expression<Func<Setting, string, bool>> filter = (x, y) => x.Id == 1;

            var visitor = new ExpressionRebaser(typeof(Setting), "setting");

            Assert.That(() => visitor.RebaseTo<UserEntity, bool>(filter), Throws.InstanceOf<ArgumentException>());
        }

        [Test]
        public void ExpresionRebaserThrowsNothingIfExpressionHasTwoParametersAndSecondParamterIsWhereDelegate()
        {
            Expression<Func<Setting, WhereDelegate, bool>> filter = (x, y) => x.Id == 1;

            var visitor = new ExpressionRebaser(typeof(Setting), "setting");

            Assert.That(() => visitor.RebaseTo<UserEntity, WhereDelegate, bool>(filter), Throws.Nothing);
        }

        [Test]
        public void ExpresionRebaserTest()
        {
            Expression<Func<Setting, bool>> filter = x => x.Id == 1;

            var visitor = new ExpressionRebaser(typeof(Setting), "setting");

            var visited = visitor.RebaseTo<UserEntity, bool>(filter);

            Assert.That(visited, Is.Not.Null);
            Assert.That(visited.ToString(), Is.EqualTo("x => (setting.Id == 1)"));
        }
    }

    public class GroupLinkFilter : IQueryFilter<UserGroupLinkEntity>
    {
        /// <summary>
        ///     Applies this <see cref="IQueryFilter{T}" /> to the given <see cref="IFilterableQuery{TSource}" />.
        /// </summary>
        /// <param name="query">
        ///     The <see cref="IFilterableQuery{TSource}" /> query to which this <see cref="IQueryFilter{T}" /> filter
        ///     should be applied.
        /// </param>
        public void Apply(IFilterableQuery<UserGroupLinkEntity> query)
        {
            GroupEntity groupAlias = null;

            query.Inner.Join(x => x.Group, () => groupAlias)
                .Where(x => groupAlias.Name == "A1");
        }
    }

    public class UserIdIsBetween1And2Filter : IQueryFilter<UserEntity>
    {
        public void Apply(IFilterableQuery<UserEntity> query)
        {
            query.Where(x => x.Id, NHibernate.FlowQuery.Is.Between(1, 2));
        }
    }

    public class WrappedUserIdIsBetween1And2Filter : IQueryFilter<UserEntity>
    {
        public void Apply(IFilterableQuery<UserEntity> query)
        {
            query.ApplyFilter(new UserIdIsBetween1And2Filter());
        }
    }

    public class JoinedSettingIdIs : IQueryFilter<UserEntity>
    {
        private readonly int _id;

        public JoinedSettingIdIs(int id)
        {
            _id = id;
        }

        public void Apply(IFilterableQuery<UserEntity> query)
        {
            Setting setting = null;

            query.Inner.Join(x => x.Setting, () => setting)
                .ApplyFilterOn(() => setting, new SettingIdIs(_id));
        }
    }

    public class SettingIdIs : IQueryFilter<Setting>
    {
        private readonly int _id;

        public SettingIdIs(int id)
        {
            _id = id;
        }

        public void Apply(IFilterableQuery<Setting> query)
        {
            query.Where(x => x.Id == _id);
        }
    }

    public class SettingIdByMagicStringIs : IQueryFilter<Setting>
    {
        private readonly int _id;

        public SettingIdByMagicStringIs(int id)
        {
            _id = id;
        }

        public void Apply(IFilterableQuery<Setting> query)
        {
            query.Where("Id", NHibernate.FlowQuery.Is.EqualTo(_id));
        }
    }
}