using NHibernate.Criterion;
using NHibernate.FlowQuery.Test.Setup.Entities;
using NUnit.Framework;

namespace NHibernate.FlowQuery.Test.FlowQuery.Core.ISubFlowQueryTest
{
    using FlowQueryIs = NHibernate.FlowQuery.Is;
    using Is = NUnit.Framework.Is;

    [TestFixture]
    public class CountTest : BaseTest
    {
        #region Methods (7)

        [Test]
        public void CanCountDistinctOnProperty()
        {
            var query = SubQuery.For<UserEntity>()
                .CountDistinct(x => x.Id);

            var users = Query<UserEntity>()
                .Where(x => x.Id, FlowQueryIs.EqualTo(query));

            Assert.That(users.Count(), Is.EqualTo(1));
        }

        [Test]
        public void CanCountDistinctOnPropertyUsingString()
        {
            var query = SubQuery.For<UserEntity>()
                .CountDistinct("Id");

            var users = Query<UserEntity>()
                .Where(x => x.Id, FlowQueryIs.EqualTo(query));

            Assert.That(users.Count(), Is.EqualTo(1));
        }

        [Test]
        public void CanCountLongOnStar()
        {
            var query = SubQuery.For<UserEntity>()
                .CountLong();

            var users = Query<UserEntity>()
                .Where(x => x.Id, FlowQueryIs.EqualTo(query));

            Assert.That(users.Count(), Is.EqualTo(1));
        }

        [Test]
        public void CanCountOnProjection()
        {
            var query = SubQuery.For<UserEntity>()
                .Count(Projections.Distinct(Projections.Property("Id")));

            var users = Query<UserEntity>()
                .Where(x => x.Id, FlowQueryIs.EqualTo(query));

            Assert.That(users.Count(), Is.EqualTo(1));
        }

        [Test]
        public void CanCountOnProperty()
        {
            var query = SubQuery.For<UserEntity>()
                .Count(x => x.Id);

            var users = Query<UserEntity>()
                .Where(x => x.Id, FlowQueryIs.EqualTo(query));

            Assert.That(users.Count(), Is.EqualTo(1));
        }

        [Test]
        public void CanCountOnPropertyUsingString()
        {
            var query = SubQuery.For<UserEntity>()
                .Count("Id");

            var users = Query<UserEntity>()
                .Where(x => x.Id, FlowQueryIs.EqualTo(query));

            Assert.That(users.Count(), Is.EqualTo(1));
        }

        [Test]
        public void CanCountOnStar()
        {
            var query = SubQuery.For<UserEntity>()
                .Count();

            var users = Query<UserEntity>()
                .Where(x => x.Id, FlowQueryIs.EqualTo(query));

            Assert.That(users.Count(), Is.EqualTo(1));
        }

        #endregion Methods
    }
}