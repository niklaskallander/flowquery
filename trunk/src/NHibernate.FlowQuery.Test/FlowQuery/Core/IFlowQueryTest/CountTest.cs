namespace NHibernate.FlowQuery.Test.FlowQuery.Core.IFlowQueryTest
{
    using NHibernate.Criterion;
    using NHibernate.FlowQuery.Test.Setup.Entities;

    using NUnit.Framework;

    [TestFixture]
    public class CountTest : BaseTest
    {
        [Test]
        public void CanCountDistinctOnProperty()
        {
            int count = Query<UserEntity>()
                .Distinct()
                .Count(u => u.IsOnline);

            Assert.That(count, Is.EqualTo(2));
        }

        [Test]
        public void CanCountDistinctOnPropertyUsingString()
        {
            int count = Query<UserEntity>()
                .Distinct()
                .Count("IsOnline");

            Assert.That(count, Is.EqualTo(2));
        }

        [Test]
        public void CanCountLongOnStar()
        {
            long count = Query<UserEntity>()
                .CountLong();

            Assert.That(count, Is.EqualTo(4));
        }

        [Test]
        public void CanCountOnProjection()
        {
            int count = Query<UserEntity>()
                .Count(Projections.Distinct(Projections.Property("IsOnline")));

            Assert.That(count, Is.EqualTo(2));
        }

        [Test]
        public void CanCountOnProperty()
        {
            int count = Query<UserEntity>()
                .Count(u => u.IsOnline);

            Assert.That(count, Is.EqualTo(4));
        }

        [Test]
        public void CanCountOnPropertyUsingString()
        {
            int count = Query<UserEntity>()
                .Count("IsOnline");

            Assert.That(count, Is.EqualTo(4));
        }

        [Test]
        public void CanCountOnStar()
        {
            int count = Query<UserEntity>()
                .Count();

            Assert.That(count, Is.EqualTo(4));
        }
    }
}