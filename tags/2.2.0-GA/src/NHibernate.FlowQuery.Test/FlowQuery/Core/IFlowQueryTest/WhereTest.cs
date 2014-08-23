// ReSharper disable ExpressionIsAlwaysNull
namespace NHibernate.FlowQuery.Test.FlowQuery.Core.IFlowQueryTest
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    using NHibernate.Criterion;
    using NHibernate.FlowQuery.Core;
    using NHibernate.FlowQuery.Test.Setup.Entities;

    using NUnit.Framework;

    using FqIs = Is;

    [TestFixture]
    public class WhereTest : BaseTest
    {
        [Test]
        public void CanCombineMultipleWhereCalls()
        {
            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(u => u.Firstname.Contains("kl")) // Matches one
                .Where(u => !u.IsOnline) // In combination with above, matches zero, otherwise 1
                .Select();

            Assert.That(users.Count(), Is.EqualTo(0));
        }

        [Test]
        public void CanRestrictByExample()
        {
            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .RestrictByExample
                (
                    new UserEntity { Firstname = "Niklas", Role = RoleEnum.Administrator },
                    x =>
                    {
                        x.ExcludeProperty(u => u.CreatedStamp);
                        x.ExcludeProperty(u => u.IsOnline);
                        x.ExcludeProperty(u => u.NumberOfLogOns);
                        x.ExcludeZeroes();
                        x.ExcludeNulls();
                    }

                )
                .Select();

            Assert.That(users, Is.Not.Null);

            Assert.That(users.Count(), Is.GreaterThan(0));

            foreach (UserEntity user in users)
            {
                Assert.That(user.Firstname, Is.EqualTo("Niklas"));
            }
        }

        [Test]
        public void CanRestrictEmptyCollection()
        {
            int count = Query<UserEntity>()
                .Where(x => x.Groups, FqIs.Empty())
                .Count();

            Assert.That(count, Is.EqualTo(1));
        }

        [Test]
        public void CanRestrictNotEmptyCollection()
        {
            int count = Query<UserEntity>()
                .Where(x => x.Groups, FqIs.Not.Empty())
                .Count();

            Assert.That(count, Is.EqualTo(3));
        }

        [Test]
        public void CanRestrictOnExistsGreedily()
        {
            IDetachedFlowQuery<UserEntity> subquery = DetachedQuery<UserEntity>()
                .Where(x => x.IsOnline)
                .Select(x => x.Id);

            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(subquery, FqIs.Not.Empty())
                .Select();

            Assert.That(users.Count(), Is.EqualTo(4));
        }

        [Test]
        public void CanRestrictOnExistsNonGreedily()
        {
            UserEntity user = null;

            IDetachedFlowQuery<UserEntity> subquery = DetachedQuery<UserEntity>()
                .SetRootAlias(() => user)
                .Where(x => x.IsOnline && x.Id == user.Id)
                .Select(x => x.Id);

            FlowQuerySelection<UserEntity> users = Session.FlowQuery(() => user)
                .Where(subquery, FqIs.Not.Empty())
                .Select();

            Assert.That(users.Count(), Is.EqualTo(3));
        }

        [Test]
        public void CanRestrictOnExistsNonGreedilyUsingDetachedCriteria()
        {
            UserEntity user = null;

            DetachedCriteria subquery = DetachedQuery<UserEntity>()
                .SetRootAlias(() => user)
                .Where(x => x.IsOnline && x.Id == user.Id)
                .Select(x => x.Id)
                .Criteria;

            FlowQuerySelection<UserEntity> users = Session.FlowQuery(() => user)
                .Where(subquery, FqIs.Not.Empty())
                .Select();

            Assert.That(users.Count(), Is.EqualTo(3));
        }

        [Test]
        public void CanRestrictOnNotExistsGreedily()
        {
            IDetachedImmutableFlowQuery subquery = DetachedQuery<UserEntity>()
                .Where(x => x.IsOnline)
                .Select(x => x.Id);

            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(subquery, FqIs.Empty())
                .Select();

            Assert.That(users.Count(), Is.EqualTo(0));
        }

        [Test]
        public void CanRestrictOnNotExistsNonGreedily()
        {
            UserEntity user = null;

            IDetachedFlowQuery<UserEntity> subquery = DetachedQuery<UserEntity>()
                .SetRootAlias(() => user)
                .Where(x => x.IsOnline && x.Id == user.Id)
                .Select(x => x.Id);

            FlowQuerySelection<UserEntity> users = Session.FlowQuery(() => user)
                .Where(subquery, FqIs.Empty())
                .Select();

            Assert.That(users.Count(), Is.EqualTo(1));
        }

        [Test]
        public void IsEqualToAllWithSubquery()
        {
            IDetachedFlowQuery<UserEntity> subquery = DetachedQuery<UserEntity>()
                .Select(x => x.Id);

            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(x => x.Id, FqIs.EqualToAll(subquery))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(0));
        }

        [Test]
        public void IsEqualToWithSubquery()
        {
            IDetachedFlowQuery<UserEntity> subquery = DetachedQuery<UserEntity>()
                .Where(x => x.Username == Usernames[0])
                .Select(x => x.Id);

            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(x => x.Id, FqIs.EqualTo(subquery))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(1));
        }

        [Test]
        public void IsGreaterThanAllWithSubquery()
        {
            IDetachedFlowQuery<UserEntity> subquery = DetachedQuery<UserEntity>()
                .Select(x => x.Id);

            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(x => x.Id, FqIs.GreaterThanAll(subquery))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(0));
        }

        [Test]
        public void IsGreaterThanOrEqualToAllWithSubquery()
        {
            IDetachedFlowQuery<UserEntity> subquery = DetachedQuery<UserEntity>()
                .Select(x => x.Id);

            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(x => x.Id, FqIs.GreaterThanOrEqualToAll(subquery))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(1));
        }

        [Test]
        public void IsGreaterThanOrEqualToSomeWithSubquery()
        {
            IDetachedFlowQuery<UserEntity> subquery = DetachedQuery<UserEntity>()
                .Select(x => x.Id);

            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(x => x.Id, FqIs.GreaterThanOrEqualToSome(subquery))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(4));
        }

        [Test]
        public void IsGreaterThanOrEqualToWithSubquery()
        {
            IDetachedFlowQuery<UserEntity> subquery = DetachedQuery<UserEntity>()
                .Where(x => x.Username == Usernames[0])
                .Select(x => x.Id);

            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(x => x.Id, FqIs.GreaterThanOrEqualTo(subquery))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(4));
        }

        [Test]
        public void IsGreaterThanSomeWithSubquery()
        {
            IDetachedFlowQuery<UserEntity> subquery = DetachedQuery<UserEntity>()
                .Select(x => x.Id);

            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(x => x.Id, FqIs.GreaterThanSome(subquery))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(3));
        }

        [Test]
        public void IsGreaterThanWithSubquery()
        {
            IDetachedFlowQuery<UserEntity> subquery = DetachedQuery<UserEntity>()
                .Where(x => x.Username == Usernames[0])
                .Select(x => x.Id);

            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(x => x.Id, FqIs.GreaterThan(subquery))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(3));
        }

        [Test]
        public void IsLessThanAllWithSubquery()
        {
            IDetachedFlowQuery<UserEntity> subquery = DetachedQuery<UserEntity>()
                .Select(x => x.Id);

            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(x => x.Id, FqIs.LessThanAll(subquery))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(0));
        }

        [Test]
        public void IsLessThanOrEqualToAllWithSubquery()
        {
            IDetachedFlowQuery<UserEntity> subquery = DetachedQuery<UserEntity>()
                .Select(x => x.Id);

            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(x => x.Id, FqIs.LessThanOrEqualToAll(subquery))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(1));
        }

        [Test]
        public void IsLessThanOrEqualToSomeWithSubquery()
        {
            IDetachedFlowQuery<UserEntity> subquery = DetachedQuery<UserEntity>()
                .Select(x => x.Id);

            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(x => x.Id, FqIs.LessThanOrEqualToSome(subquery))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(4));
        }

        [Test]
        public void IsLessThanOrEqualToWithSubquery()
        {
            IDetachedFlowQuery<UserEntity> subquery = DetachedQuery<UserEntity>()
                .Where(x => x.Username == Usernames[0])
                .Select(x => x.Id);

            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(x => x.Id, FqIs.LessThanOrEqualTo(subquery))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(1));
        }

        [Test]
        public void IsLessThanSomeWithSubquery()
        {
            IDetachedFlowQuery<UserEntity> subquery = DetachedQuery<UserEntity>()
                .Select(x => x.Id);

            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(x => x.Id, FqIs.LessThanSome(subquery))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(3));
        }

        [Test]
        public void IsLessThanWithSubquery()
        {
            IDetachedFlowQuery<UserEntity> subquery = DetachedQuery<UserEntity>()
                .Where(x => x.Username == Usernames[0])
                .Select(x => x.Id);

            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(x => x.Id, FqIs.LessThan(subquery))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(0));
        }

        [Test]
        public void LogicalAndWithConstantFalseFetchesNone()
        {
            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(u => u.Id > 0 && false)
                .Select();

            Assert.That(users.Count(), Is.EqualTo(0));
        }

        [Test]
        public void LogicalAndWithStringAndIsHelper()
        {
            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where("IsOnline", FqIs.EqualTo(true))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(3));

            foreach (UserEntity u in users)
            {
                Assert.That(u.IsOnline);
            }
        }

        [Test]
        public void LogicalOrWithConstantTrueFetchesAll()
        {
            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(u => u.Id > 6 || true)
                .Select();

            Assert.That(users.Count(), Is.EqualTo(4));
        }

        [Test]
        public void RestrictByExampleThrowsIfExampleInstanceIsNull()
        {
            Assert
                .That
                (
                    () => DummyQuery<UserEntity>().RestrictByExample(null, x => { }),
                    Throws.InstanceOf<ArgumentNullException>()
                );
        }

        [Test]
        public void RestrictByExampleThrowsIfExampleIsNull()
        {
            var example = new UserEntity
            {
                Firstname = "Niklas",
                Role = RoleEnum.Administrator
            };

            Assert
                .That
                (
                    () => DummyQuery<UserEntity>().RestrictByExample(example, null),
                    Throws.InstanceOf<ArgumentNullException>()
                );
        }

        [Test]
        public void Where()
        {
            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(u => u.IsOnline)
                .Select();

            Assert.That(users.Count(), Is.EqualTo(3));

            foreach (UserEntity user in users)
            {
                Assert.That(user.IsOnline, Is.True);
            }
        }

        [Test]
        public void WhereBetweenWithIsHelper()
        {
            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(u => u.Id, FqIs.Between(2, 3))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(2));

            foreach (UserEntity user in users)
            {
                Assert.That(user.Id, Is.InRange(2, 3));
            }
        }

        [Test]
        public void WhereConstantFalseFetchesNone()
        {
            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(u => false)
                .Select();

            Assert.That(users.Count(), Is.EqualTo(0));
        }

        [Test]
        public void WhereConstantTrueFetchesAll()
        {
            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(u => true)
                .Select();

            Assert.That(users.Count(), Is.EqualTo(4));
        }

        [Test]
        public void WhereEqualTo()
        {
            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(u => u.Id == 2)
                .Select();

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.First().Id, Is.EqualTo(2));
        }

        [Test]
        public void WhereGreaterThan()
        {
            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(u => u.Id > 1)
                .Select();

            Assert.That(users.Count(), Is.EqualTo(3));

            foreach (UserEntity user in users)
            {
                Assert.That(user.Id, Is.GreaterThan(1));
            }
        }

        [Test]
        public void WhereGreaterThanOrEqualTo()
        {
            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(u => u.Id >= 1)
                .Select();

            Assert.That(users.Count(), Is.EqualTo(4));

            foreach (UserEntity user in users)
            {
                Assert.That(user.Id, Is.GreaterThanOrEqualTo(1));
            }
        }

        [Test]
        public void WhereGreaterThanOrEqualToWithValueLeftAndProjectionRight()
        {
            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(u => 4 >= u.Id)
                .Select();

            Assert.That(users.Count(), Is.EqualTo(4));
        }

        [Test]
        public void WhereGreaterThanWithValueLeftAndProjectionRight()
        {
            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(u => 4 > u.Id)
                .Select();

            Assert.That(users.Count(), Is.EqualTo(3));
        }

        [Test]
        public void WhereInDetachedCriteriaWithIsHelper()
        {
            DetachedCriteria criteria = DetachedCriteria.For<UserEntity>()
                .Add(Restrictions.Or(Restrictions.Eq("Id", (long)1), Restrictions.Eq("Id", (long)4)))
                .SetProjection(Projections.Property("Id"));

            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(u => u.Id, FqIs.In(criteria))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(2));

            foreach (UserEntity user in users)
            {
                Assert.That(new long[] { 1, 4 }, Contains.Item(user.Id));
            }
        }

        [Test]
        public void WhereInSubqueryWithIsHelper()
        {
            IDetachedFlowQuery<UserEntity> subquery = DetachedQuery<UserEntity>()
                .Where(x => x.Id == 1 || x.Id == 4)
                .Select(x => x.Id);

            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(u => u.Id, FqIs.In(subquery))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(2));

            foreach (UserEntity user in users)
            {
                Assert.That(new long[] { 1, 4 }, Contains.Item(user.Id));
            }
        }

        [Test]
        public void WhereInWithIsHelper()
        {
            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(u => u.Id, FqIs.In(1, 3))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(2));

            foreach (UserEntity user in users)
            {
                Assert.That(new long[] { 1, 3 }, Contains.Item(user.Id));
            }
        }

        [Test]
        public void WhereLessThan()
        {
            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(u => u.Id < 4)
                .Select();

            Assert.That(users.Count(), Is.EqualTo(3));

            foreach (UserEntity user in users)
            {
                Assert.That(user.Id, Is.LessThan(4));
            }
        }

        [Test]
        public void WhereLessThanOrEqualTo()
        {
            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(u => u.Id <= 4)
                .Select();

            Assert.That(users.Count(), Is.EqualTo(4));

            foreach (UserEntity user in users)
            {
                Assert.That(user.Id, Is.LessThanOrEqualTo(4));
            }
        }

        [Test]
        public void WhereLessThanOrEqualToWithValueLeftAndProjectionRight()
        {
            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(u => 1 <= u.Id)
                .Select();

            Assert.That(users.Count(), Is.EqualTo(4));
        }

        [Test]
        public void WhereLessThanWithValueLeftAndProjectionRight()
        {
            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(u => 1 < u.Id)
                .Select();

            Assert.That(users.Count(), Is.EqualTo(3));
        }

        [Test]
        public void WhereLikeWithIsHelper()
        {
            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(u => u.Firstname, FqIs.Like("Nik%"))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.First().Firstname.StartsWith("Nik"));
        }

        [Test]
        public void WhereNot()
        {
            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(u => !u.IsOnline)
                .Select();

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.First().IsOnline, Is.False);
        }

        [Test]
        public void WhereNotEqualTo()
        {
            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(u => u.Id != 2)
                .Select();

            Assert.That(users.Count(), Is.EqualTo(3));
            Assert.That(users.First().Id, Is.Not.EqualTo(2));
        }

        [Test]
        public void WhereProjectionEqualToProjectionLeftNotMember()
        {
            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(x => x.Username.Substring(0, 1) == x.Firstname)
                .Select();

            Assert.That(users.Count(), Is.EqualTo(0));
        }

        [Test]
        public void WhereProjectionEqualToProjectionNoMembers()
        {
            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(x => x.Username.Substring(0, 1) == x.Firstname.Substring(0, 1))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(1));
        }

        [Test]
        public void WhereProjectionEqualToProjectionOnlyMembers()
        {
            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(x => x.Username == x.Firstname)
                .Select();

            Assert.That(users.Count(), Is.EqualTo(0));
        }

        [Test]
        public void WhereProjectionEqualToProjectionRightNotMember()
        {
            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(x => x.Username == x.Firstname.Substring(0, 1))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(0));
        }

        [Test]
        public void WhereProjectionGreaterThanOrEqualToProjectionLeftNotMember()
        {
            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(x => (x.Id + x.Id) >= x.Setting.Id)
                .Select();

            Assert.That(users.Count(), Is.EqualTo(2));
        }

        [Test]
        public void WhereProjectionGreaterThanOrEqualToProjectionNoMembers()
        {
            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(x => (x.Id + x.Id) >= (x.Setting.Id + x.Setting.Id))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(0));
        }

        [Test]
        public void WhereProjectionGreaterThanOrEqualToProjectionOnlyMembers()
        {
            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(x => x.Id >= x.Setting.Id)
                .Select();

            Assert.That(users.Count(), Is.EqualTo(0));
        }

        [Test]
        public void WhereProjectionGreaterThanOrEqualToProjectionRightNotMember()
        {
            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(x => x.Setting.Id >= (x.Id + x.Id))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(3));
        }

        [Test]
        public void WhereProjectionGreaterThanProjectionLeftNotMember()
        {
            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(x => (x.Id + x.Id) > x.Setting.Id)
                .Select();

            Assert.That(users.Count(), Is.EqualTo(1));
        }

        [Test]
        public void WhereProjectionGreaterThanProjectionNoMembers()
        {
            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(x => (x.Id + x.Id) > (x.Setting.Id + x.Setting.Id))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(0));
        }

        [Test]
        public void WhereProjectionGreaterThanProjectionOnlyMembers()
        {
            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(x => x.Id > x.Setting.Id)
                .Select();

            Assert.That(users.Count(), Is.EqualTo(0));
        }

        [Test]
        public void WhereProjectionGreaterThanProjectionRightNotMember()
        {
            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(x => x.Setting.Id > (x.Id + x.Id))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(2));
        }

        [Test]
        public void WhereProjectionLessThanOrEqualToProjectionLeftNotMember()
        {
            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(x => (x.Id + x.Id) <= x.Setting.Id)
                .Select();

            Assert.That(users.Count(), Is.EqualTo(3));
        }

        [Test]
        public void WhereProjectionLessThanOrEqualToProjectionNoMembers()
        {
            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(x => (x.Id + x.Id) <= (x.Setting.Id + x.Setting.Id))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(4));
        }

        [Test]
        public void WhereProjectionLessThanOrEqualToProjectionOnlyMembers()
        {
            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(x => x.Id <= x.Setting.Id)
                .Select();

            Assert.That(users.Count(), Is.EqualTo(4));
        }

        [Test]
        public void WhereProjectionLessThanOrEqualToProjectionRightNotMember()
        {
            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(x => x.Setting.Id <= (x.Id + x.Id))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(2));
        }

        [Test]
        public void WhereProjectionLessThanProjectionLeftNotMember()
        {
            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(x => (x.Id + x.Id) < x.Setting.Id)
                .Select();

            Assert.That(users.Count(), Is.EqualTo(2));
        }

        [Test]
        public void WhereProjectionLessThanProjectionNoMembers()
        {
            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(x => (x.Id + x.Id) < (x.Setting.Id + x.Setting.Id))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(4));
        }

        [Test]
        public void WhereProjectionLessThanProjectionOnlyMembers()
        {
            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(x => x.Id < x.Setting.Id)
                .Select();

            Assert.That(users.Count(), Is.EqualTo(4));
        }

        [Test]
        public void WhereProjectionLessThanProjectionRightNotMember()
        {
            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(x => x.Setting.Id < (x.Id + x.Id))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(1));
        }

        [Test]
        public void WhereProjectionNotEqualToProjectionLeftNotMember()
        {
            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(x => x.Username.Substring(0, 1) != x.Firstname)
                .Select();

            Assert.That(users.Count(), Is.EqualTo(4));
        }

        [Test]
        public void WhereProjectionNotEqualToProjectionNoMembers()
        {
            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(x => x.Username.Substring(0, 1) != x.Firstname.Substring(0, 1))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(3));
        }

        [Test]
        public void WhereProjectionNotEqualToProjectionOnlyMembers()
        {
            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(x => x.Username != x.Firstname)
                .Select();

            Assert.That(users.Count(), Is.EqualTo(4));
        }

        [Test]
        public void WhereProjectionNotEqualToProjectionRightNotMember()
        {
            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(x => x.Username != x.Firstname.Substring(0, 1))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(4));
        }

        [Test]
        public void WhereWithArithmeticOperations()
        {
            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(u => (u.Id * 2) + 8 + (10 / 5) - 1 == 11) // Id == 1
                .Select();

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That((users.First().Id * 2) + 8 + (10 / 5) - 1, Is.EqualTo(11));
        }

        [Test]
        public void WhereWithConcatenation()
        {
            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(u => u.Firstname + " " + u.Lastname == "Niklas Källander")
                .Select();

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.First().Firstname + " " + users.First().Lastname, Is.EqualTo("Niklas Källander"));
        }

        [Test]
        public void WhereWithConstantValue()
        {
            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(u => u.IsOnline != true)
                .Select();

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.First().IsOnline, Is.False);
        }

        [Test]
        public void WhereWithCriterions()
        {
            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(Restrictions.Eq("IsOnline", true), Restrictions.Like("Firstname", "%kl%"))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.Single().IsOnline);
            Assert.That(users.Single().Firstname.Contains("kl"));
        }

        [Test]
        public void WhereWithCriterionsDoesNotThrowIfArrayContainsNullItem()
        {
            ICriterion[] c =
            {
                Restrictions.Eq("IsOnline", true), 
                null, 
                Restrictions.Like("Firstname", "%kl%")
            };

            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(c)
                .Select();

            Assert.That(users, Is.Not.Null);
        }

        [Test]
        public void WhereWithCriterionsThrowsIfArrayIsNull()
        {
            ICriterion[] c = null;

            Assert.That(() => Query<UserEntity>().Where(c).Select(), Throws.InstanceOf<ArgumentNullException>());
        }

        [Test]
        public void WhereWithCustomInvocationExpression()
        {
            Func<bool> func = () => true;

            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(u => func())
                .Select();

            Assert.That(users.Count(), Is.EqualTo(4));
        }

        [Test]
        [SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1003:SymbolsMustBeSpacedCorrectly",
            Justification = "Reviewed. Suppression is OK here.")]
        public void WhereWithDoubleNegation()
        {
            // ReSharper disable once NegativeEqualityExpression, DoubleNegationOperator, RedundantBoolCompare
            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(u => !!(u.IsOnline == true))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(3));

            foreach (UserEntity user in users)
            {
                Assert.That(user.IsOnline, Is.True);
            }
        }

        [Test]
        public void WhereWithExclusiveOr()
        {
            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(u => u.IsOnline ^ (u.Firstname == "Niklas"))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(2));

            foreach (UserEntity u in users)
            {
                if (u.IsOnline)
                {
                    Assert.That(u.Firstname, Is.Not.EqualTo("Niklas"));
                }
            }
        }

        [Test]
        public void WhereWithInvalidMethodCallThrows()
        {
            Assert
                .That
                (
                    () => Query<UserEntity>().Where(u => u.Firstname.Any()).Select(),
                    Throws.InstanceOf<NotSupportedException>()
                );
        }

        [Test]
        public void WhereWithIsHelper()
        {
            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(u => u.IsOnline, FqIs.EqualTo(true))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(3));

            foreach (UserEntity u in users)
            {
                Assert.That(u.IsOnline);
            }
        }

        [Test]
        public void WhereWithMultipleBinaryExpressions()
        {
            // ReSharper disable once RedundantBoolCompare
            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(u => (u.Id == 1 || u.Id > 3) && u.IsOnline == true)
                .Select();

            Assert.That(users.Count(), Is.EqualTo(1));

            foreach (UserEntity user in users)
            {
                Assert.That((user.Id == 1 || user.Id > 3) && user.IsOnline);
            }
        }

        [Test]
        public void WhereWithNegation()
        {
            // ReSharper disable once NegativeEqualityExpression
            // ReSharper disable once RedundantBoolCompare
            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(u => !(u.IsOnline == true))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.First().IsOnline, Is.False);
        }

        [Test]
        public void WhereWithNotNullCheck()
        {
            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(u => u.LastLoggedInStamp != null)
                .Select();

            Assert.That(users.Count(), Is.EqualTo(3));

            foreach (UserEntity user in users)
            {
                Assert.That(user.LastLoggedInStamp, Is.Not.Null);
            }
        }

        [Test]
        public void WhereWithNullCheck()
        {
            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(u => u.LastLoggedInStamp == null)
                .Select();

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.First().LastLoggedInStamp, Is.Null);
        }

        [Test]
        public void WhereWithProjection1EqualProjection2()
        {
            UserGroupLinkEntity link = null;

            var users = Query<UserEntity>()
                .Inner.Join(u => u.Groups, () => link)
                .Where(u => u.Id == link.Id)
                .Select(u => new { u.Username, UserId = u.Id, LinkId = link.Id });

            Assert.That(users.Count(), Is.EqualTo(1));
        }

        [Test]
        public void WhereWithProjection1GreaterThanOrEqualToProjection2()
        {
            UserGroupLinkEntity link = null;

            var users = Query<UserEntity>()
                .Inner.Join(u => u.Groups, () => link)
                .Where(u => u.Id >= link.Id)
                .Select(u => new { u.Username, UserId = u.Id, LinkId = link.Id });

            Assert.That(users.Count(), Is.EqualTo(1));
        }

        [Test]
        public void WhereWithProjection1GreaterThanProjection2()
        {
            UserGroupLinkEntity link = null;

            var users = Query<UserEntity>()
                .Inner.Join(u => u.Groups, () => link)
                .Where(u => u.Id > link.Id)
                .Select(u => new { u.Username, UserId = u.Id, LinkId = link.Id });

            Assert.That(users.Count(), Is.EqualTo(0));
        }

        [Test]
        public void WhereWithProjection1LessThanOrEqualToProjection2()
        {
            UserGroupLinkEntity link = null;

            var users = Query<UserEntity>()
                .Inner.Join(u => u.Groups, () => link)
                .Where(u => u.Id <= link.Id)
                .Select(u => new { u.Username, UserId = u.Id, LinkId = link.Id });

            Assert.That(users.Count(), Is.EqualTo(5));
        }

        [Test]
        public void WhereWithProjection1LessThanProjection2()
        {
            UserGroupLinkEntity link = null;

            var users = Query<UserEntity>()
                .Inner.Join(u => u.Groups, () => link)
                .Where(u => u.Id < link.Id)
                .Select(u => new { u.Username, UserId = u.Id, LinkId = link.Id });

            Assert.That(users.Count(), Is.EqualTo(4));
        }

        [Test]
        public void WhereWithProjection1NotEqualProjection2()
        {
            UserGroupLinkEntity link = null;

            var users = Query<UserEntity>()
                .Inner.Join(u => u.Groups, () => link)
                .Where(u => u.Id != link.Id)
                .Select(u => new { u.Username, UserId = u.Id, LinkId = link.Id });

            Assert.That(users.Count(), Is.EqualTo(4));
        }

        [Test]
        public void WhereWithStringContains()
        {
            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(u => u.Firstname.Contains("kl"))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.First().Firstname.Contains("kl"));
        }

        [Test]
        public void WhereWithStringEndsWith()
        {
            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(u => u.Firstname.EndsWith("las"))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.First().Firstname.EndsWith("las"));
        }

        [Test]
        public void WhereWithStringStartsWith()
        {
            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(u => u.Firstname.StartsWith("Nik"))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.First().Firstname.StartsWith("Nik"));
        }

        [Test]
        public void WhereWithValueFromExternalMethod()
        {
            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(u => GetFalse())
                .Select();

            Assert.That(users.Count(), Is.EqualTo(0));
        }

        [Test]
        public void WhereWithWhereDelegateHelper()
        {
            var lastNames = new object[] { "Nilsson", "Källander" };

            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where((u, where) => u.Firstname == "Niklas"
                    && (where(u.Lastname, FqIs.In(lastNames))
                        || where(u.IsOnline, FqIs.Not.EqualTo(true))))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(1));
        }

        [Test]
        public void WhereWithWhereDelegateUsingString()
        {
            var lastNames = new object[] { "Nilsson", "Källander" };

            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where((u, where) => u.Firstname == "Niklas"
                    && (where("Lastname", FqIs.In(lastNames))
                        || where("u.IsOnline", FqIs.Not.EqualTo(true))))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(1));
        }

        protected bool GetFalse()
        {
            return false;
        }
    }
}