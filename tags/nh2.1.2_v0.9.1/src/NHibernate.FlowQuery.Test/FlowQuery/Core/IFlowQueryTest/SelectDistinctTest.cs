using System;
using System.Linq;
using System.Linq.Expressions;
using NHibernate.Criterion;
using NHibernate.FlowQuery.Test.Setup.Dtos;
using NHibernate.FlowQuery.Test.Setup.Entities;
using NUnit.Framework;

namespace NHibernate.FlowQuery.Test.FlowQuery.Core.IFlowQueryTest
{
    using Is = NUnit.Framework.Is;
    using Property = NHibernate.FlowQuery.Property;

    [TestFixture]
    public class SelectDistinctTest : BaseTest
    {
        #region Methods (29)

        [Test]
        public void CanAutomapDistinctlyUsingExpressions()
        {
            var users = Query<UserEntity>().SelectDistinct(x => x.IsOnline).ToMap<UserDto>();

            Assert.That(users.Count(), Is.EqualTo(2));
            foreach (var u in users)
            {
                Assert.That(u.Id, Is.EqualTo(0));
                Assert.That(u.Fullname, Is.Null);
                Assert.That(u.Username, Is.Null);
            }
        }

        [Test]
        public void CanAutomapDistinctlyUsingStrings()
        {
            var users = Query<UserEntity>().SelectDistinct("IsOnline").ToMap<UserDto>();

            Assert.That(users.Count(), Is.EqualTo(2));
            foreach (var u in users)
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
                .SelectDistinct(u => new
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
                .SelectDistinct(x => new { avg = Aggregate.Average(x.Id) });

            Assert.That(aggregations.Count(), Is.EqualTo(1));
        }

        [Test]
        public void CanSelectDistinctOnMultipleProperties()
        {
            var users = Query<UserEntity>().SelectDistinct(x => x.Role, x => x.IsOnline, x => x.Username);

            Assert.That(users.Count(), Is.EqualTo(4));
        }

        [Test]
        public void CanSelectDistinctOnSingleProperty()
        {
            var roles = Query<UserEntity>().SelectDistinct(x => x.Role);

            Assert.That(roles.Count(), Is.EqualTo(3));
        }

        [Test]
        public void CanSelectDistinctToAnonymous()
        {
            var news = Query<UserEntity>()
                .SelectDistinct(x => new { x.Role });

            Assert.That(news.Count(), Is.EqualTo(3));
        }

        [Test]
        public void CanSelectDistinctToMemberInitExpression()
        {
            var members = Query<UserEntity>()
                .SelectDistinct(x => new UserDto("Niklas"));

            Assert.That(members.Count(), Is.EqualTo(1));
        }

        [Test]
        public void CanSelectDistinctToMemberInitNestedInAnonymous()
        {
            var anonymous = Query<UserEntity>()
                .SelectDistinct(x => new { x.IsOnline, Member = new UserDto("Niklas") });

            Assert.That(anonymous.Count(), Is.EqualTo(2));
        }

        [Test]
        public void CanSelectDistinctToNestedAnonymous()
        {
            var anonymous = Query<UserEntity>()
                .SelectDistinct(x => new { Something = new { x.Role } });

            Assert.That(anonymous.Count(), Is.EqualTo(3));
        }

        [Test]
        public void CanSelectDistinctToNestedMemberInits()
        {
            var users = Query<UserEntity>()
                .SelectDistinct(x => new UserEntity()
                                {
                                    Id = x.Id,
                                    Setting = new Setting()
                                });

            Assert.That(users.Count(), Is.EqualTo(4));

            foreach (var user in users)
            {
                Assert.That(user.Id, Is.GreaterThan(0));
                Assert.That(user.Setting, Is.Not.Null);
                Assert.That(user.Setting.Id, Is.EqualTo(0));
            }
        }

        [Test]
        public void CanSelectDistinctUsingPropertyHelper()
        {
            var roles = Query<UserEntity>()
                .SelectDistinct(x => Property.As<RoleEnum>("x.Role"));

            Assert.That(roles.Count(), Is.EqualTo(3));
        }

        [Test]
        public void CanSelectDistinctUsingPropertyProjectionTyped()
        {
            var bools = Query<UserEntity>().SelectDistinct<bool>(Projections.Property("IsOnline"));

            Assert.That(bools.Count(), Is.EqualTo(2));
            Assert.That(bools.ElementAt(0), Is.False);
            Assert.That(bools.ElementAt(1), Is.True);
        }

        [Test]
        public void CanSelectDistinctUsingPropertyProjectionUntyped()
        {
            var bools = Query<UserEntity>().SelectDistinct(Projections.Property("IsOnline"));

            Assert.That(bools.Count(), Is.EqualTo(2));
            Assert.That(bools.ElementAt(0), Is.False);
            Assert.That(bools.ElementAt(1), Is.True);
        }

        [Test]
        public void CanSelectDistinctUsingSelectSetup()
        {
            var users = Query<UserEntity>()
                            .SelectDistinct<UserDto>()
                                .For(x => x.IsOnline).Use(x => x.IsOnline)
                                .Select();

            Assert.That(users.Count(), Is.EqualTo(2));
            Assert.That(users.First().IsOnline, Is.False);
            Assert.That(users.Last().IsOnline, Is.True);
        }

        [Test]
        public void CanSelectDistinctUsingSelectSetupWithProjections()
        {
            var users = Query<UserEntity>()
                            .SelectDistinct()
                                .For(x => x.IsOnline).Use(Projections.Property("IsOnline"))
                                .Select();

            Assert.That(users.Count(), Is.EqualTo(2));
            Assert.That(users.First().IsOnline, Is.False);
            Assert.That(users.Last().IsOnline, Is.True);
        }

        [Test]
        public void CanSelectDistinctUsingSelectSetupWithStrings()
        {
            var users = Query<UserEntity>()
                            .SelectDistinct<UserDto>()
                                .For("IsOnline").Use(x => x.IsOnline)
                                .Select();

            Assert.That(users.Count(), Is.EqualTo(2));
            Assert.That(users.First().IsOnline, Is.False);
            Assert.That(users.Last().IsOnline, Is.True);
        }

        [Test]
        public void CanSelectDistinctUsingString()
        {
            var roles = Query<UserEntity>().SelectDistinct("Role");

            Assert.That(roles.Count(), Is.EqualTo(3));
        }

        [Test]
        public void CanSelectDistinctUsingProjection()
        {
            var users = Query<UserEntity>().SelectDistinct<UserDto>(Projections.ProjectionList().AddProperty("Username").AddProperty("Firstname", "SomeValue"));

            Assert.That(users.Count(), Is.EqualTo(4));
        }

        [Test]
        public void CanSelectDistinctWithArithmeticOperations()
        {
            var results = Query<UserEntity>()
                .SelectDistinct(x => x.Id * 2 + 1 / 3 - 1);

            Assert.That(results.Count(), Is.EqualTo(4));
        }

        [Test]
        public void SelectDistinctUsingDistinctSetupThrowsWhenProvidingNullToFor()
        {
            Expression<Func<UserEntity, object>> x = null;

            Assert.That(() => { Query<UserEntity>().SelectDistinct<UserDto>().For(c => c.Fullname).Use(x); }, Throws.InstanceOf<ArgumentNullException>());
        }

        [Test]
        public void SelectDistinctUsingDistinctSetupThrowsWhenProvidingNullToUse()
        {
            Expression<Func<UserDto, object>> x = null;

            Assert.That(() => { Query<UserEntity>().SelectDistinct<UserDto>().For(x); }, Throws.InstanceOf<ArgumentNullException>());
        }

        [Test]
        public void SelectDistinctUsingDistinctSetupThrowsWhenSelectingEmediately()
        {
            Assert.That(() => { Query<UserEntity>().SelectDistinct<UserDto>().Select(); }, Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void SelectDistinctUsingExpressionsThrowsIfExpressionArrayIsNull()
        {
            Expression<Func<UserEntity, object>>[] e = null;

            Assert.That(() =>
            {
                Query<UserEntity>().SelectDistinct(e);

            }, Throws.InstanceOf<ArgumentNullException>());
        }

        [Test]
        public void SelectDistinctUsingExpressionThrowsWhenExpressionDoesNotContainAnyProjections()
        {
            Assert.That(() => { Query<UserEntity>().SelectDistinct(x => new { }); }, Throws.InstanceOf<NotSupportedException>());
        }

        [Test]
        public void SelectDistinctUsingExpressionThrowsWhenExpressionIsNull()
        {
            Expression<Func<UserEntity, UserDto>> e = null;

            Assert.That(() => { Query<UserEntity>().SelectDistinct(e); }, Throws.InstanceOf<ArgumentNullException>());
        }

        [Test]
        public void SelectDistinctUsingProjectionThrowsWhenProjectionIsNull()
        {
            IProjection p = null;

            Assert.That(() => { Query<UserEntity>().SelectDistinct(p); }, Throws.InstanceOf<ArgumentNullException>());
            Assert.That(() => { Query<UserEntity>().SelectDistinct<UserDto>(p); }, Throws.InstanceOf<ArgumentNullException>());
        }

        [Test]
        public void SelectDistinctUsingPropertyProjectionThrowsWhenProjectionIsNull()
        {
            PropertyProjection p = null;

            Assert.That(() => { Query<UserEntity>().SelectDistinct(p); }, Throws.InstanceOf<ArgumentNullException>());
            Assert.That(() => { Query<UserEntity>().SelectDistinct<UserDto>(p); }, Throws.InstanceOf<ArgumentNullException>());
        }

        [Test]
        public void SelectDistinctUsingSelectSetupThrowsIfNoSetupIsMade()
        {
            Assert.That(() =>
                        {
                            Query<UserEntity>()
                                .SelectDistinct()
                                    .Select();

                        }, Throws.InvalidOperationException);
        }

        [Test]
        public void SelectUsingStringsThrowsIfStringArrayIsNull()
        {
            string[] strings = null;

            Assert.That(() =>
            {
                Query<UserEntity>().SelectDistinct(strings);

            }, Throws.InstanceOf<ArgumentNullException>());
        }

        #endregion Methods
    }
}