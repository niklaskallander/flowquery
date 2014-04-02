using NHibernate.Criterion;
using NHibernate.FlowQuery.Test.Setup.Entities;
using NUnit.Framework;

namespace NHibernate.FlowQuery.Test.FlowQuery.Core.IFlowQueryTest
{
    using Is = NUnit.Framework.Is;

    [TestFixture]
    public class CountTest : BaseTest
    {
        [Test]
        public void CanCountDistinctOnProperty()
        {
            var count = Query<UserEntity>()
                .Distinct()
                .Count(u => u.IsOnline)
                ;

            Assert.That(count, Is.EqualTo(2));
        }

        [Test]
        public void CanCountDistinctOnPropertyUsingString()
        {
            var count = Query<UserEntity>()
                .Distinct()
                .Count("IsOnline")
                ;

            Assert.That(count, Is.EqualTo(2));
        }

        [Test]
        public void CanCountLongOnStar()
        {
            var count = Query<UserEntity>()
                .CountLong()
                ;

            Assert.That(count, Is.EqualTo(4));
        }

        [Test]
        public void CanCountOnProjection()
        {
            var count = Query<UserEntity>()
                .Count(Projections.Distinct(Projections.Property("IsOnline")))
                ;

            Assert.That(count, Is.EqualTo(2));
        }

        [Test]
        public void CanCountOnProperty()
        {
            var count = Query<UserEntity>()
                .Count(u => u.IsOnline)
                ;

            Assert.That(count, Is.EqualTo(4));
        }

        [Test]
        public void CanCountOnPropertyUsingString()
        {
            var count = Query<UserEntity>()
                .Count("IsOnline")
                ;

            Assert.That(count, Is.EqualTo(4));
        }

        [Test]
        public void CanCountOnStar()
        {
            var count = Query<UserEntity>()
                .Count()
                ;

            Assert.That(count, Is.EqualTo(4));
        }
    }
}