using NHibernate.Criterion;
using NHibernate.FlowQuery.Test.Setup.Entities;
using NUnit.Framework;

namespace NHibernate.FlowQuery.Test.FlowQuery.Core.ISubFlowQueryTest
{
    using System.Linq;
    using FlowQueryIs = NHibernate.FlowQuery.Is;
    using Is = NUnit.Framework.Is;

    [TestFixture]
    public class OrderTest : BaseTest
    {
        #region Methods (12)

        [Test]
        public void SkipsOrderByStatementsIfUnnecessaryInSubquery()
        {
            var detached = Query<UserEntity>()
                .Detached()
                .OrderBy(x => x.IsOnline)
                .Select(x => x.Id);

            Assert.That(() =>
                        {
                            var users = Query<UserEntity>()
                                .Where(x => x.Id, FlowQueryIs.In(detached))
                                .Select();

                            Assert.That(users.Count(), Is.EqualTo(4));

                        }, Throws.Nothing);
        }

        [Test]
        public void DoesNotSkipOrderByStatementsIfNecessaryInSubqueryTest1()
        {
            var detached = Query<UserEntity>()
                .Detached()
                .Skip(1)
                .OrderBy(x => x.IsOnline)
                .Select(x => x.Id);

            Assert.That(() =>
                        {
                            var users = Query<UserEntity>()
                                .Where(x => x.Id, FlowQueryIs.In(detached))
                                .Select();

                            Assert.That(users.Count(), Is.EqualTo(3));
                            Assert.That(users.All(x => x.IsOnline));

                        }, Throws.Nothing);
        }

        [Test]
        public void DoesNotSkipOrderByStatementsIfNecessaryInSubqueryTest2()
        {
            var detached = Query<UserEntity>()
                .Detached()
                .Take(1)
                .OrderBy(x => x.IsOnline)
                .Select(x => x.Id);

            Assert.That(() =>
            {
                var users = Query<UserEntity>()
                    .Where(x => x.Id, FlowQueryIs.In(detached))
                    .Select();

                Assert.That(users.Count(), Is.EqualTo(1));
                Assert.That(users.All(x => !x.IsOnline));

            }, Throws.Nothing);
        }

        [Test]
        public void DoesNotSkipOrderByStatementsIfNecessaryInSubqueryTest3()
        {
            var detached = Query<UserEntity>()
                .Detached()
                .Limit(2, 2)
                .OrderByDescending(x => x.IsOnline)
                .Select(x => x.Id);

            Assert.That(() =>
            {
                var users = Query<UserEntity>()
                    .Where(x => x.Id, FlowQueryIs.In(detached))
                    .Select();

                Assert.That(users.Count(), Is.EqualTo(2));
                Assert.That(users.All(x => x.IsOnline), Is.False);

            }, Throws.Nothing);
        }

        [Test]
        public void CanOrderAscending()
        {
            var id = Query<UserEntity>().Detached()
                .OrderBy(x => x.Firstname)
                .Take(1)
                .Skip(1)
                .Select(x => x.Id);

            var users = Query<UserEntity>()
                .Where(x => x.Id, FlowQueryIs.EqualTo(id))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.ElementAt(0).Firstname, Is.EqualTo("Lars"));
        }

        [Test]
        public void CanOrderAscendingUsingProjection()
        {
            var id = Query<UserEntity>()
                .Detached()
                .OrderBy(Projections.Property("Firstname"))
                .Take(1)
                .Skip(1)
                .Select(x => x.Id);

            var users = Query<UserEntity>()
                .Where(x => x.Id, FlowQueryIs.EqualTo(id))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.ElementAt(0).Firstname, Is.EqualTo("Lars"));
        }

        [Test]
        public void CanOrderAscendingUsingString()
        {
            var id = Query<UserEntity>().Detached()
                .OrderBy("Firstname")
                .Take(1)
                .Skip(1)
                .Select(x => x.Id);

            var users = Query<UserEntity>()
                .Where(x => x.Id, FlowQueryIs.EqualTo(id))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.ElementAt(0).Firstname, Is.EqualTo("Lars"));
        }

        [Test]
        public void CanOrderDescending()
        {
            var id = Query<UserEntity>().Detached()
                .OrderByDescending(u => u.Firstname)
                .Take(1)
                .Skip(1)
                .Select(x => x.Id);

            var users = Query<UserEntity>()
                .Where(x => x.Id, FlowQueryIs.EqualTo(id))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.ElementAt(0).Firstname, Is.EqualTo("Lotta"));
        }

        [Test]
        public void CanOrderDescendingUsingProjection()
        {
            var id = Query<UserEntity>().Detached()
                .OrderByDescending(Projections.Property("Firstname"))
                .Take(1)
                .Skip(1)
                .Select(x => x.Id);

            var users = Query<UserEntity>()
                .Where(x => x.Id, FlowQueryIs.EqualTo(id))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.ElementAt(0).Firstname, Is.EqualTo("Lotta"));
        }

        [Test]
        public void CanOrderDescendingUsingString()
        {
            var id = Query<UserEntity>().Detached()
                .OrderByDescending("Firstname")
                .Take(1)
                .Skip(1)
                .Select(x => x.Id);

            var users = Query<UserEntity>()
                .Where(x => x.Id, FlowQueryIs.EqualTo(id))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.ElementAt(0).Firstname, Is.EqualTo("Lotta"));
        }

        #endregion Methods
    }
}