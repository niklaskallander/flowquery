using System.Collections;
using System.Linq;
using NHibernate.FlowQuery.Test.Setup.Entities;
using NUnit.Framework;

namespace NHibernate.FlowQuery.Test.FlowQuery
{
    using FqIs = Is;
    using Is = NUnit.Framework.Is;

    [TestFixture]
    public class IsTest : BaseTest
    {
        [Test]
        public void CanFilterBetween()
        {
            var users = Query<UserEntity>()
                .Where(x => x.Id, FqIs.Between(2, 4))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(3));

            foreach (var u in users)
            {
                Assert.That(u.Id, Is.InRange(2, 4));
            }
        }

        [Test]
        public void CanFilterEqualTo()
        {
            var users = Query<UserEntity>()
                .Where(x => x.Id, FqIs.EqualTo(2))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.First().Id, Is.EqualTo(2));
        }

        [Test]
        public void CanFilterGreaterThan()
        {
            var users = Query<UserEntity>()
                .Where(x => x.Id, FqIs.GreaterThan(2))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(2));
            Assert.That(users.First().Id, Is.GreaterThan(2));
            Assert.That(users.ElementAt(1).Id, Is.GreaterThan(2));
        }

        [Test]
        public void CanFilterGreaterThanOrEqualTo()
        {
            var users = Query<UserEntity>()
                .Where(x => x.Id, FqIs.GreaterThanOrEqualTo(2))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(3));
            Assert.That(users.First().Id, Is.GreaterThanOrEqualTo(2));
            Assert.That(users.ElementAt(1).Id, Is.GreaterThanOrEqualTo(2));
            Assert.That(users.ElementAt(2).Id, Is.GreaterThanOrEqualTo(2));
        }

        [Test]
        public void CanFilterInWithEnumerableCollection()
        {
            var users = Query<UserEntity>()
                .Where(x => x.Id, FqIs.In(new long[] { 1, 3 }))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(2));
            Assert.That(users.First().Id, Is.EqualTo(1));
            Assert.That(users.ElementAt(1).Id, Is.EqualTo(3));
        }

        [Test]
        public void CanFilterInWithStrictEnumerable()
        {
            var users = Query<UserEntity>()
                .Where(x => x.Id, FqIs.In(new TestEnumerable(3, 1, 4)))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(3));
            Assert.That(users.ElementAt(0).Id, Is.EqualTo(1));
            Assert.That(users.ElementAt(1).Id, Is.EqualTo(3));
            Assert.That(users.ElementAt(2).Id, Is.EqualTo(4));
        }

        [Test]
        public void CanFilterInWithSubquery()
        {
            var users = Query<UserEntity>()
                .Where(x => x.Id, FqIs.In(DetachedQuery<UserEntity>().Where(x => x.IsOnline).Select(x => x.Id)))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(3));

            foreach (var u in users)
            {
                Assert.That(u.IsOnline);
            }
        }

        [Test]
        public void CanFilterInWithValues()
        {
            var users = Query<UserEntity>()
                .Where(x => x.Id, FqIs.In(2, 4))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(2));
            Assert.That(users.First().Id, Is.EqualTo(2));
            Assert.That(users.ElementAt(1).Id, Is.EqualTo(4));
        }

        [Test]
        public void CanFilterLessThan()
        {
            var users = Query<UserEntity>()
                .Where(x => x.Id, FqIs.LessThan(2))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.First().Id, Is.LessThan(2));
        }

        [Test]
        public void CanFilterLessThanOrEqualTo()
        {
            var users = Query<UserEntity>()
                .Where(x => x.Id, FqIs.LessThanOrEqualTo(2))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(2));
            Assert.That(users.First().Id, Is.LessThanOrEqualTo(2));
            Assert.That(users.ElementAt(1).Id, Is.LessThanOrEqualTo(2));
        }

        [Test]
        public void CanFilterLike()
        {
            var users = Query<UserEntity>()
                .Where(x => x.Firstname, FqIs.Like("%i%"))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.First().Firstname.Contains("i"));
        }

        [Test]
        public void CanFilterNotNull()
        {
            var users = Query<UserEntity>()
                .Where(x => x.LastLoggedInStamp, FqIs.Not.Null())
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(3));

            foreach (var u in users)
            {
                Assert.That(u.LastLoggedInStamp, Is.Not.Null);
            }
        }

        [Test]
        public void CanFilterNull()
        {
            var users = Query<UserEntity>()
                .Where(x => x.LastLoggedInStamp, FqIs.Null())
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.First().LastLoggedInStamp, Is.Null);
        }

        [Test]
        public void CanNegateFilter()
        {
            var users = Query<UserEntity>()
                .Where(x => x.Id, FqIs.Not.EqualTo(2))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(3));

            foreach (var u in users)
            {
                Assert.That(u.Id, Is.Not.EqualTo(2));
            }
        }

        public class TestEnumerable : IEnumerable
        {
            private readonly object[] _values;

            public TestEnumerable(params object[] values)
            {
                _values = values;
            }

            public IEnumerator GetEnumerator()
            {
                return _values.GetEnumerator();
            }
        }
    }
}