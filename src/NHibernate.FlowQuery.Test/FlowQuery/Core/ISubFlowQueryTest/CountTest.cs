using NHibernate.Criterion;
using NHibernate.FlowQuery.Test.Setup.Entities;
using NUnit.Framework;

namespace NHibernate.FlowQuery.Test.FlowQuery.Core.ISubFlowQueryTest
{
    using FqIs = Is;
    using Is = NUnit.Framework.Is;

    [TestFixture]
    public class CountTest : BaseTest
    {
        [Test]
        public void CanCountDistinctOnProperty()
        {
            var query = DetachedQuery<UserEntity>()
                .Distinct()
                .Count(x => x.Id);

            var users = Query<UserEntity>()
                .Where(x => x.Id, FqIs.EqualTo(query));

            Assert.That(users.Count(), Is.EqualTo(1));
        }

        [Test]
        public void CanCountDistinctOnPropertyUsingString()
        {
            var query = DetachedQuery<UserEntity>()
                .Distinct()
                .Count("Id");

            var users = Query<UserEntity>()
                .Where(x => x.Id, FqIs.EqualTo(query));

            Assert.That(users.Count(), Is.EqualTo(1));
        }

        [Test]
        public void CanCountLongOnStar()
        {
            var query = DetachedQuery<UserEntity>()
                .CountLong();

            var users = Query<UserEntity>()
                .Where(x => x.Id, FqIs.EqualTo(query));

            Assert.That(users.Count(), Is.EqualTo(1));
        }

        [Test]
        public void CanCountOnProjection()
        {
            var query = DetachedQuery<UserEntity>()
                .Count(Projections.Distinct(Projections.Property("Id")));

            var users = Query<UserEntity>()
                .Where(x => x.Id, FqIs.EqualTo(query));

            Assert.That(users.Count(), Is.EqualTo(1));
        }

        [Test]
        public void CanCountOnProperty()
        {
            var query = DetachedQuery<UserEntity>()
                .Count(x => x.Id);

            var users = Query<UserEntity>()
                .Where(x => x.Id, FqIs.EqualTo(query));

            Assert.That(users.Count(), Is.EqualTo(1));
        }

        [Test]
        public void CanCountOnPropertyUsingString()
        {
            var query = DetachedQuery<UserEntity>()
                .Count("Id");

            var users = Query<UserEntity>()
                .Where(x => x.Id, FqIs.EqualTo(query));

            Assert.That(users.Count(), Is.EqualTo(1));
        }

        [Test]
        public void CanCountOnStar()
        {
            var query = DetachedQuery<UserEntity>()
                .Count();

            var users = Query<UserEntity>()
                .Where(x => x.Id, FqIs.EqualTo(query));

            Assert.That(users.Count(), Is.EqualTo(1));
        }
    }
}