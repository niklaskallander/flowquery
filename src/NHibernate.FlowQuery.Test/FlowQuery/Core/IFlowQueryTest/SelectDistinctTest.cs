// ReSharper disable ExpressionIsAlwaysNull
namespace NHibernate.FlowQuery.Test.FlowQuery.Core.IFlowQueryTest
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using NHibernate.Criterion;
    using NHibernate.FlowQuery.Core;
    using NHibernate.FlowQuery.Test.Setup.Dtos;
    using NHibernate.FlowQuery.Test.Setup.Entities;

    using NUnit.Framework;

    using Property = NHibernate.FlowQuery.Property;

    [TestFixture]
    public class SelectDistinctTest : BaseTest
    {
        [Test]
        public void CanAutomapDistinctlyUsingExpressions()
        {
            FlowQuerySelection<UserDto> users = Query<UserEntity>()
                .Distinct()
                .Select(x => x.IsOnline, x => x.Role)
                .ToMap<UserDto>();

            Assert.That(users.Count(), Is.EqualTo(3));
            Assert.That(users.Any(x => x.IsOnline));

            foreach (UserDto u in users)
            {
                Assert.That(u.Id, Is.EqualTo(0));
                Assert.That(u.Fullname, Is.Null);
                Assert.That(u.Username, Is.Null);
            }
        }

        [Test]
        public void CanAutomapDistinctlyUsingStrings()
        {
            FlowQuerySelection<UserDto> users = Query<UserEntity>()
                .Distinct()
                .Select("IsOnline")
                .ToMap<UserDto>();

            Assert.That(users.Count(), Is.EqualTo(2));

            foreach (UserDto u in users)
            {
                Assert.That(u.Id, Is.EqualTo(0));
                Assert.That(u.Fullname, Is.Null);
                Assert.That(u.Username, Is.Null);
            }
        }

        [Test]
        public void CanSelectDistinctAggregationInGroupByUsingAggregateHelper()
        {
            var aggregations = Query<UserEntity>()
                .Distinct()
                .Select(u => new
                {
                    Count = Aggregate.Count(u.Id),
                    Role = Aggregate.GroupBy(u.Role)
                });

            Assert.That(aggregations.Count(), Is.EqualTo(3));

            foreach (var a in aggregations)
            {
                switch (a.Role)
                {
                    case RoleEnum.Administrator:
                        Assert.That(a.Count, Is.EqualTo(2));
                        break;
                    default:
                        Assert.That(a.Count, Is.EqualTo(1));
                        break;
                }
            }
        }

        [Test]
        public void CanSelectDistinctAggregationUserAggregateHelper()
        {
            var aggregations = Query<UserEntity>()
                .Distinct()
                .Select(x => new { avg = Aggregate.Average(x.Id) });

            Assert.That(aggregations.Count(), Is.EqualTo(1));
        }

        [Test]
        public void CanSelectDistinctOnMultipleProperties()
        {
            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Distinct()
                .Select(x => x.Role, x => x.IsOnline, x => x.Username);

            Assert.That(users.Count(), Is.EqualTo(4));
        }

        [Test]
        public void CanSelectDistinctOnSingleProperty()
        {
            FlowQuerySelection<RoleEnum> roles = Query<UserEntity>()
                .Distinct()
                .Select(x => x.Role);

            Assert.That(roles.Count(), Is.EqualTo(3));
        }

        [Test]
        public void CanSelectDistinctToAnonymous()
        {
            var news = Query<UserEntity>()
                .Distinct()
                .Select(x => new { x.Role });

            Assert.That(news.Count(), Is.EqualTo(3));
        }

        [Test]
        public void CanSelectDistinctToMemberInitExpression()
        {
            FlowQuerySelection<UserDto> members = Query<UserEntity>()
                .Distinct()
                .Select(x => new UserDto("Niklas"));

            Assert.That(members.Count(), Is.EqualTo(1));
        }

        [Test]
        public void CanSelectDistinctToMemberInitNestedInAnonymous()
        {
            var anonymous = Query<UserEntity>()
                .Distinct()
                .Select(x => new { x.IsOnline, Member = new UserDto("Niklas") });

            Assert.That(anonymous.Count(), Is.EqualTo(2));
        }

        [Test]
        public void CanSelectDistinctToNestedAnonymous()
        {
            var anonymous = Query<UserEntity>()
                .Distinct()
                .Select(x => new { Something = new { x.Role } });

            Assert.That(anonymous.Count(), Is.EqualTo(3));
        }

        [Test]
        public void CanSelectDistinctToNestedMemberInits()
        {
            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Distinct()
                .Select(x => new UserEntity
                {
                    Id = x.Id,
                    Setting = new Setting()
                });

            Assert.That(users.Count(), Is.EqualTo(4));

            foreach (UserEntity user in users)
            {
                Assert.That(user.Id, Is.GreaterThan(0));
                Assert.That(user.Setting, Is.Not.Null);
                Assert.That(user.Setting.Id, Is.EqualTo(0));
            }
        }

        [Test]
        public void CanSelectDistinctUsingBasicProjection()
        {
            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .OrderBy("Id")
                .Distinct().Select
                (
                    Projections
                        .ProjectionList()
                        .AddProperty("Id")
                        .AddProperty("Username")
                );

            Assert.That(users.Count(), Is.EqualTo(4));
            Assert.That(users.ElementAt(0).Id, Is.EqualTo(1));
            Assert.That(users.ElementAt(1).Id, Is.EqualTo(2));
            Assert.That(users.ElementAt(2).Id, Is.EqualTo(3));
            Assert.That(users.ElementAt(3).Id, Is.EqualTo(4));
            Assert.That(users.ElementAt(0).Username, Is.EqualTo("Wimpy"));
            Assert.That(users.ElementAt(1).Username, Is.EqualTo("Izmid"));
            Assert.That(users.ElementAt(2).Username, Is.EqualTo("Empor"));
            Assert.That(users.ElementAt(3).Username, Is.EqualTo("Lajsa"));
            Assert.That(users.ElementAt(0).Firstname, Is.Null);
            Assert.That(users.ElementAt(1).Firstname, Is.Null);
            Assert.That(users.ElementAt(2).Firstname, Is.Null);
            Assert.That(users.ElementAt(3).Firstname, Is.Null);
        }

        [Test]
        public void CanSelectDistinctUsingProjection()
        {
            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Distinct()
                .Select(Projections.ProjectionList().AddProperty("Username").AddProperty("Firstname"));

            Assert.That(users.Count(), Is.EqualTo(4));
        }

        [Test]
        public void CanSelectDistinctUsingPropertyHelper()
        {
            FlowQuerySelection<RoleEnum> roles = Query<UserEntity>()
                .Distinct()
                .Select(x => Property.As<RoleEnum>("x.Role"));

            Assert.That(roles.Count(), Is.EqualTo(3));
        }

        [Test]
        public void CanSelectDistinctUsingPropertyProjectionTyped()
        {
            FlowQuerySelection<bool> bools = Query<UserEntity>()
                .Distinct()
                .Select<bool>(Projections.Property("IsOnline"));

            Assert.That(bools.Count(), Is.EqualTo(2));
            Assert.That(bools.ElementAt(0), Is.False);
            Assert.That(bools.ElementAt(1), Is.True);
        }

        [Test]
        public void CanSelectDistinctUsingSelectSetup()
        {
            FlowQuerySelection<UserDto> users = Query<UserEntity>()
                .Distinct().Select<UserDto>()
                .For(x => x.IsOnline).Use(x => x.IsOnline)
                .Select();

            Assert.That(users.Count(), Is.EqualTo(2));
            Assert.That(users.First().IsOnline, Is.False);
            Assert.That(users.Last().IsOnline, Is.True);
        }

        [Test]
        public void CanSelectDistinctUsingSelectSetupWithProjections()
        {
            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Distinct()
                .Select<UserEntity>()
                .For(x => x.IsOnline).Use(Projections.Property("IsOnline"))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(2));
            Assert.That(users.First().IsOnline, Is.False);
            Assert.That(users.Last().IsOnline, Is.True);
        }

        [Test]
        public void CanSelectDistinctUsingSelectSetupWithStrings()
        {
            FlowQuerySelection<UserDto> users = Query<UserEntity>()
                .Distinct().Select<UserDto>()
                .For("IsOnline").Use(x => x.IsOnline)
                .Select();

            Assert.That(users.Count(), Is.EqualTo(2));
            Assert.That(users.First().IsOnline, Is.False);
            Assert.That(users.Last().IsOnline, Is.True);
        }

        [Test]
        public void CanSelectDistinctUsingString()
        {
            FlowQuerySelection<UserEntity> roles = Query<UserEntity>()
                .Distinct()
                .Select("Role");

            Assert.That(roles.Count(), Is.EqualTo(3));
        }

        [Test]
        public void CanSelectDistinctWithArithmeticOperations()
        {
            FlowQuerySelection<long> results = Query<UserEntity>()
                .Distinct()
                .Select(x => (x.Id * 2) + (1 / 3) - 1);

            Assert.That(results.Count(), Is.EqualTo(4));
        }

        [Test]
        public void SelectDistinctUsingDistinctSetupThrowsWhenProvidingNullToFor()
        {
            Expression<Func<UserEntity, object>> x = null;

            Assert
                .That
                (
                    () =>
                    {
                        DummyQuery<UserEntity>()
                            .Distinct().Select<UserDto>()
                            .For(c => c.Fullname).Use(x);
                    },
                    Throws.InstanceOf<ArgumentNullException>()
                );
        }

        [Test]
        public void SelectDistinctUsingDistinctSetupThrowsWhenProvidingNullToUse()
        {
            Expression<Func<UserDto, object>> x = null;

            Assert
                .That
                (
                    () =>
                    {
                        DummyQuery<UserEntity>()
                            .Distinct().Select<UserDto>()
                            .For(x);
                    },
                    Throws.InstanceOf<ArgumentNullException>()
                );
        }

        [Test]
        public void SelectDistinctUsingDistinctSetupThrowsWhenSelectingEmediately()
        {
            Assert
                .That
                (
                    () =>
                    {
                        DummyQuery<UserEntity>()
                            .Distinct().Select<UserDto>()
                            .Select();
                    },
                    Throws.InstanceOf<InvalidOperationException>()
                );
        }

        [Test]
        public void SelectDistinctUsingExpressionThrowsWhenExpressionDoesNotContainAnyProjections()
        {
            Assert
                .That
                (
                    () =>
                    {
                        DummyQuery<UserEntity>()
                            .Distinct().Select(x => new { });
                    },
                    Throws.InstanceOf<NotSupportedException>()
                );
        }

        [Test]
        public void SelectDistinctUsingExpressionThrowsWhenExpressionIsNull()
        {
            Expression<Func<UserEntity, UserDto>> e = null;

            Assert
                .That
                (
                    () =>
                    {
                        DummyQuery<UserEntity>()
                            .Distinct().Select(e);
                    },
                    Throws.InstanceOf<ArgumentNullException>()
                );
        }

        [Test]
        public void SelectDistinctUsingExpressionsThrowsIfExpressionArrayIsNull()
        {
            Expression<Func<UserEntity, object>>[] e = null;

            Assert
                .That
                (
                    () =>
                    {
                        DummyQuery<UserEntity>()
                            .Distinct().Select(e);
                    },
                    Throws.InstanceOf<ArgumentNullException>()
                );
        }

        [Test]
        public void SelectDistinctUsingProjectionThrowsWhenProjectionIsNull()
        {
            IProjection p = null;

            Assert
                .That
                (
                    () =>
                    {
                        DummyQuery<UserEntity>()
                            .Distinct().Select(p);
                    },
                    Throws.InstanceOf<ArgumentNullException>()
                );

            Assert
                .That
                (
                    () =>
                    {
                        DummyQuery<UserEntity>()
                            .Distinct().Select<UserDto>(p);
                    },
                    Throws.InstanceOf<ArgumentNullException>()
                );
        }

        [Test]
        public void SelectDistinctUsingPropertyProjectionThrowsWhenProjectionIsNull()
        {
            PropertyProjection p = null;

            Assert
                .That
                (
                    () =>
                    {
                        DummyQuery<UserEntity>()
                            .Distinct().Select(p);
                    },
                    Throws.InstanceOf<ArgumentNullException>()
                );

            Assert
                .That
                (
                    () =>
                    {
                        DummyQuery<UserEntity>()
                            .Distinct()
                            .Select<UserDto>(p);
                    },
                    Throws.InstanceOf<ArgumentNullException>()
                );
        }

        [Test]
        public void SelectDistinctUsingSelectSetupThrowsIfNoSetupIsMade()
        {
            Assert
                .That
                (
                    () =>
                    {
                        DummyQuery<UserEntity>()
                            .Distinct()
                            .Select<UserEntity>()
                            .Select();
                    },
                    Throws.InvalidOperationException
                );
        }

        [Test]
        public void SelectUsingStringsThrowsIfStringIsNull()
        {
            Assert
                .That
                (
                    () =>
                    {
                        DummyQuery<UserEntity>()
                            .Distinct().Select((string)null);
                    },
                    Throws.InstanceOf<ArgumentNullException>()
                );
        }
    }
}