namespace NHibernate.FlowQuery.Test.FlowQuery.Core.ISubFlowQueryTest
{
    using System.Linq;

    using NHibernate.Criterion;
    using NHibernate.FlowQuery.Core;
    using NHibernate.FlowQuery.Test.Setup.Entities;

    using NUnit.Framework;

    using FqIs = Is;

    [TestFixture]
    public class OrderTest : BaseTest
    {
        [Test]
        public void CanOrderAscending()
        {
            IDetachedFlowQuery<UserEntity> id = DetachedQuery<UserEntity>()
                .OrderBy(x => x.Firstname)
                .Take(1)
                .Skip(1)
                .Select(x => x.Id);

            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(x => x.Id, FqIs.EqualTo(id))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.ElementAt(0).Firstname, Is.EqualTo("Lars"));
        }

        [Test]
        public void CanOrderAscendingUsingProjection()
        {
            IDetachedFlowQuery<UserEntity> id = Query<UserEntity>()
                .Detached()
                .OrderBy(Projections.Property("Firstname"))
                .Take(1)
                .Skip(1)
                .Select(x => x.Id);

            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(x => x.Id, FqIs.EqualTo(id))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.ElementAt(0).Firstname, Is.EqualTo("Lars"));
        }

        [Test]
        public void CanOrderAscendingUsingString()
        {
            IDetachedFlowQuery<UserEntity> id = DetachedQuery<UserEntity>()
                .OrderBy("Firstname")
                .Take(1)
                .Skip(1)
                .Select(x => x.Id);

            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(x => x.Id, FqIs.EqualTo(id))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.ElementAt(0).Firstname, Is.EqualTo("Lars"));
        }

        [Test]
        public void CanOrderDescending()
        {
            IDetachedFlowQuery<UserEntity> id = DetachedQuery<UserEntity>()
                .OrderByDescending(u => u.Firstname)
                .Take(1)
                .Skip(1)
                .Select(x => x.Id);

            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(x => x.Id, FqIs.EqualTo(id))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.ElementAt(0).Firstname, Is.EqualTo("Lotta"));
        }

        [Test]
        public void CanOrderDescendingUsingProjection()
        {
            IDetachedFlowQuery<UserEntity> id = DetachedQuery<UserEntity>()
                .OrderByDescending(Projections.Property("Firstname"))
                .Take(1)
                .Skip(1)
                .Select(x => x.Id);

            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(x => x.Id, FqIs.EqualTo(id))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.ElementAt(0).Firstname, Is.EqualTo("Lotta"));
        }

        [Test]
        public void CanOrderDescendingUsingString()
        {
            IDetachedFlowQuery<UserEntity> id = DetachedQuery<UserEntity>()
                .OrderByDescending("Firstname")
                .Take(1)
                .Skip(1)
                .Select(x => x.Id);

            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(x => x.Id, FqIs.EqualTo(id))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.ElementAt(0).Firstname, Is.EqualTo("Lotta"));
        }

        [Test]
        public void DoesNotSkipOrderByStatementsIfNecessaryInSubqueryTest1()
        {
            IDetachedFlowQuery<UserEntity> detached = Query<UserEntity>()
                .Detached()
                .Skip(1)
                .OrderBy(x => x.IsOnline)
                .Select(x => x.Id);

            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(x => x.Id, FqIs.In(detached))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(3));
            Assert.That(users.All(x => x.IsOnline));
        }

        [Test]
        public void DoesNotSkipOrderByStatementsIfNecessaryInSubqueryTest2()
        {
            IDetachedFlowQuery<UserEntity> detached = Query<UserEntity>()
                .Detached()
                .Take(1)
                .OrderBy(x => x.IsOnline)
                .Select(x => x.Id);

            Assert
                .That
                (
                    () =>
                    {
                        FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                            .Where(x => x.Id, FqIs.In(detached))
                            .Select();

                        Assert.That(users.Count(), Is.EqualTo(1));
                        Assert.That(users.All(x => !x.IsOnline));
                    }, 
                    Throws.Nothing
                );
        }

        [Test]
        public void DoesNotSkipOrderByStatementsIfNecessaryInSubqueryTest3()
        {
            IDetachedFlowQuery<UserEntity> detached = Query<UserEntity>()
                .Detached()
                .Limit(2, 2)
                .OrderByDescending(x => x.IsOnline)
                .Select(x => x.Id);

            Assert
                .That
                (
                    () =>
                    {
                        FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                            .Where(x => x.Id, FqIs.In(detached))
                            .Select();

                        Assert.That(users.Count(), Is.EqualTo(2));
                        Assert.That(users.All(x => x.IsOnline), Is.False);
                    }, 
                    Throws.Nothing
                );
        }

        [Test]
        public void SkipsOrderByStatementsIfUnnecessaryInSubquery()
        {
            IDetachedFlowQuery<UserEntity> detached = Query<UserEntity>()
                .Detached()
                .OrderBy(x => x.IsOnline)
                .Select(x => x.Id);

            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(x => x.Id, FqIs.In(detached))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(4));
        }
    }
}